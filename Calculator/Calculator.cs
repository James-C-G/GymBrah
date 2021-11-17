using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace Calculator
{
    public class Calculator : Parse
    {
        public Calculator(List<Token> tokens) : base(tokens){}

        private CalculatorNode _factor()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Negate:
                {
                    ScanToken();
                    return new NegateNode(_factor());
                }
                case TokenType.Integer:
                {
                    return new IntegerNode(ScanToken().IntegerContent);
                }
                case TokenType.Id:
                {
                    string varName = ScanToken().StringContent;
                    if (true)
                    {
                        //hardcode for now
                        return new IdNode(5);
                    }

                    throw new Exception("ID Error");
                }
                case TokenType.OpenBracket:
                {
                    ScanToken();
                    
                    CalculatorNode node = Expression();
                    
                    if(CurrentToken.Type == TokenType.CloseBracket)
                    {
                        ScanToken();
                        return node;
                    }

                    throw new Exception("Bracket Error");
                }
                default:
                {
                    throw new Exception("Error"); //TODO Make detailed
                }
            }
        }
        
        private CalculatorNode _term()
        {
            CalculatorNode nodeOne = _factor();

            while (true)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Multiply:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _factor();
                        
                        nodeOne = new MultiplyNode(nodeOne, nodeTwo);
                        break;
                    }
                    case TokenType.Divide:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _factor();
                        
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
        
        public CalculatorNode Expression()
        {
            CalculatorNode nodeOne = _term();
            
            while (true)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Addition:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _term();
                    
                        nodeOne = new AddNode(nodeOne, nodeTwo);

                        break;
                    }
                    case TokenType.Subtraction:
                    {
                        ScanToken();
                        CalculatorNode nodeTwo = _term();
                    
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
            Lexer lex = new Lexer("2 plates?");

            Calculator calculator = new Calculator(lex.Tokens);
            CalculatorNode result = calculator.Expression();
            int e = result.Evaluate();
            Console.Out.WriteLine(e);
        }
    }
}