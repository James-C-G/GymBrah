using System;
using System.Collections.Generic;

namespace Calculator
{
    class Calculator
    {
        private int _counter;
        private TokenType _currentToken;
        public List<Token> Tokens;
        
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
                case TokenType.Integer:
                {
                    return new Integer(ScanToken(Tokens).IntegerContent);
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

                    return null;
                }
                default:
                {
                    return null;
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
            Lexer lex = new Lexer("3 * 5 * 3?");

            Calculator calculator = new Calculator(lex.Tokens);
            Node result = calculator.Expression();
            int e = result.Evaluate();
            Console.Out.WriteLine(e);
        }
    }
}
