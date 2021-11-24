using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace GymBrah
{
    public class Calculator : Parse
    {
        private readonly bool _parseMaths;

        public Calculator(List<Token> tokens, ref Dictionary<String, Value> variableTable, bool parseMaths = true) : 
            base(tokens, ref variableTable)
        {
            _parseMaths = parseMaths;
        }

        private Node _parseFactor()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Negate:
                {
                    ScanToken();
                    if (_parseMaths) return new NegateNode(_parseFactor());
                    return new NegateNodeString(_parseFactor());
                }
                case TokenType.Integer:
                {
                    return new TerminalNode(ScanToken());
                }
                case TokenType.Id:
                {
                    Token idToken = ScanToken();
                    if (VariableTable.TryGetValue(idToken.Content, out Value result))
                    {
                        if (result.Type == ValueType.Integer)
                        {
                            // Check that variable exists and is of the right type
                            if (_parseMaths) return new TerminalNode(new Token(TokenType.Id, ((IntegerValue)result).Content));
                            return new TerminalNode(idToken);
                        }

                        throw new Exception("Variable is not an integer.");
                    }

                    throw new Exception("Variable is not defined.");
                }
                case TokenType.OpenBracket: //TODO Doesn't handle close bracket errors
                {
                    ScanToken();
                    
                    Node node = ParseTree();
                    
                    if(CurrentToken.Type == TokenType.CloseBracket)
                    {
                        ScanToken();
                        return _parseMaths ? node : new BracketNodeString(node);
                    }

                    throw new Exception("Bracket not closed properly.");
                }
                default:
                {
                    throw new Exception("Invalid token in calculation " + CurrentToken.Content); 
                }
            }
        }
        
        private Node _parseTerm()
        {
            Node nodeOne = _parseFactor();

            while (true)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Multiply:
                    {
                        ScanToken();
                        Node nodeTwo = _parseFactor();
                        
                        nodeOne = _parseMaths ? new MultiplyNode(nodeOne, nodeTwo) : new MultiplyNodeString(nodeOne, nodeTwo);
                        break;
                    }
                    case TokenType.Divide:
                    {
                        ScanToken();
                        Node nodeTwo = _parseFactor();
                        
                        nodeOne = _parseMaths ? new DivideNode(nodeOne, nodeTwo) : new DivideNodeString(nodeOne, nodeTwo);
                        break;
                    }
                    default:
                    {
                        return nodeOne;
                    }
                }
            }
        }
        
        public override Node ParseTree()
        {
            Node nodeOne = _parseTerm();
            
            while (true)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Addition:
                    {
                        ScanToken();
                        Node nodeTwo = _parseTerm();
                    
                        nodeOne = _parseMaths ? new AddNode(nodeOne, nodeTwo) : new AddNodeString(nodeOne, nodeTwo);

                        break;
                    }
                    case TokenType.Subtraction:
                    {
                        ScanToken();
                        Node nodeTwo = _parseTerm();
                    
                        nodeOne = _parseMaths ? new SubtractNode(nodeOne, nodeTwo) : new SubtractNodeString(nodeOne, nodeTwo);

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
    
    // class Program
    // {
    //     static void Main(string[] args)
    //     {
    //         Lexer lex = new Lexer("(x * 3 + 4) / 2?");
    //         Dictionary<String, Value> var = new Dictionary<String, Value>();
    //         var.Add("x", new IntegerValue("10"));
    //         
    //         Calculator calculator = new Calculator(lex.Tokens, ref var);
    //         Node result = calculator.ParseTree();
    //         string e = result.Evaluate();
    //         Console.Out.WriteLine(e);
    //     }
    // }
}