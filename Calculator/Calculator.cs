using System;
using System.Collections.Generic;
using Tokenizer;

namespace Calculator
{
    class Calculator
    {
        private int _counter;
        private TokenType _currentToken;
        public List<Token> Tokens;
        
        // Temporary Variable Table
        public Dictionary<String, String> VarTable = new Dictionary<String, String>()
        {    
            {"x", "1"}
        };
        
        public Calculator(List<Token> tokens)
        {
            Tokens = tokens;
            _currentToken = tokens[_counter].Type;
        }
        
        private Token ScanToken(List<Token> tokens)
        {
            Token token = tokens[_counter];  
            _counter++; 
            _currentToken = tokens[_counter].Type; 
            return token;
        }
        
        private Node Factor()
        {
            switch (_currentToken)
            {
                case TokenType.Negate:
                {
                    ScanToken(Tokens);
                    return new Negate(Factor());
                }
                case TokenType.Integer:
                {
                    return new Integer(ScanToken(Tokens).IntegerContent);
                }
                case TokenType.Id:
                {
                    string varName = ScanToken(Tokens).StringContent;
                    if (VarTable.TryGetValue(varName, out string result))
                    {
                        return new Id(int.Parse(result));
                    }

                    throw new Exception("ID Error");
                }
                case TokenType.OpenBrace:
                {
                    ScanToken(Tokens);
                    Node node = Expression();
                    
                    if(_currentToken == TokenType.CloseBrace)
                    {
                        ScanToken(Tokens);
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
        
        private Node Term()
        {
            Node nodeOne = Factor();

            while (true)
            {
                switch (_currentToken)
                {
                    case TokenType.Multiply:
                    {
                        ScanToken(Tokens);
                        Node nodeTwo = Factor();
                        
                        nodeOne = new Multiply(nodeOne, nodeTwo);
                        break;
                    }
                    case TokenType.Divide:
                    {
                        ScanToken(Tokens);
                        Node nodeTwo = Factor();
                        
                        nodeOne = new Divide(nodeOne, nodeTwo);

                        break;
                    }
                    default:
                    {
                        return nodeOne;
                    }
                }
            }
        }
        
        public Node Expression()
        {
            Node nodeOne = Term();
            
            while (true)
            {
                switch (_currentToken)
                {
                    case TokenType.Addition:
                    {
                        ScanToken(Tokens);
                        Node nodeTwo = Term();
                    
                        nodeOne = new Add(nodeOne, nodeTwo);

                        break;
                    }
                    case TokenType.Subtraction:
                    {
                        ScanToken(Tokens);
                        Node nodeTwo = Term();
                    
                        nodeOne = new Subtract(nodeOne, nodeTwo);

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
            Lexer lex = new Lexer("(x + 1) plates?");
            
            Calculator calculator = new Calculator(lex.Tokens);
            Node result = calculator.Expression();
            int e = result.Evaluate();
            Console.Out.WriteLine(e);
        }
    }
}
