using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace Calculator
{
    public class Calculator : Parse<int>
    {
        public Calculator(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        private CalculatorNode _parseFactor()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Negate:
                {
                    ScanToken();
                    return new NegateNode(_parseFactor());
                }
                case TokenType.Integer:
                {
                    return new IntegerNode(ScanToken().IntegerContent);
                }
                case TokenType.Id:
                {
                    if (VariableTable.TryGetValue(ScanToken().StringContent, out Value result))
                    {
                        if (result.Type == ValueType.Integer)
                        {
                            // Check that variable exists and is of the right type
                            return new IdNode(((IntegerValue)result).Content);
                        }

                        throw new Exception("Variable is not an integer.");
                    }

                    throw new Exception("Variable is not defined.");
                }
                case TokenType.OpenBracket: //TODO Doesn't handle close bracket errors
                {
                    ScanToken();
                    
                    CalculatorNode node = ParseTree();
                    
                    if(CurrentToken.Type == TokenType.CloseBracket)
                    {
                        ScanToken();
                        return node;
                    }

                    throw new Exception("Bracket not closed properly.");
                }
                default:
                {
                    throw new Exception("Invalid token in calculation."); //TODO Make detailed
                }
            }
        }
        
        private CalculatorNode _parseTerm()
        {
            CalculatorNode nodeOne = _parseFactor();

            while (true)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Multiply:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _parseFactor();
                        
                        nodeOne = new MultiplyNode(nodeOne, nodeTwo);
                        break;
                    }
                    case TokenType.Divide:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _parseFactor();
                        
                        nodeOne = new DivideNode(nodeOne, nodeTwo);
                        break;
                    }
                    default:
                    {
                        return nodeOne;
                    }
                }
            }
        }
        
        public override CalculatorNode ParseTree()
        {
            CalculatorNode nodeOne = _parseTerm();
            
            while (true)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Addition:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _parseTerm();
                    
                        nodeOne = new AddNode(nodeOne, nodeTwo);

                        break;
                    }
                    case TokenType.Subtraction:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _parseTerm();
                    
                        nodeOne = new SubtractNode(nodeOne, nodeTwo);

                        break;
                    }
                    default:
                    {
                        return nodeOne;
                    }
                }
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lex = new Lexer("x 3?");
            Dictionary<String, Value> var = new Dictionary<String, Value>();
            var.Add("x", new IntegerValue(10));
            
            Calculator calculator = new Calculator(lex.Tokens, ref var);
            CalculatorNode result = calculator.ParseTree();
            int e = result.Evaluate();
            Console.Out.WriteLine(e);
        }
    }
}