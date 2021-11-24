using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class Boolean : Parse
    {
        private TokenType _comparisonType;
        public Boolean(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        private Node _parseC()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.LessThan:
                case TokenType.GreaterThan:
                case TokenType.Equals:
                {
                    return new TerminalNode(ScanToken());
                }
                default:
                {
                    return null;
                }
            }
        }

        private Node _parseB()
        {
            Token start = ScanToken();
            Node nodeOne = _parseC();
            
            switch (start.Type)
            {
                case TokenType.Not:
                case TokenType.Equals:
                {
                    if (nodeOne == null) throw new Exception("Not a boolean comparison.");
                    return new BoolStart(start, nodeOne);
                }
                case TokenType.LessThan:
                case TokenType.GreaterThan:
                {
                    if (nodeOne == null) return new TerminalNode(start);
                    return new BoolStart(start, nodeOne);
                }
                default:
                {
                    throw new Exception("Unrecognised boolean token " + CurrentToken.Type); 
                }
            }
        }
        
        private List<Token> _parseA()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                switch (i.Type)
                {
                    case TokenType.Equals:
                    case TokenType.LessThan:
                    case TokenType.Not:
                    case TokenType.GreaterThan:
                    {
                        return exprTokens;
                    }
                    default:
                    {
                        ScanToken();
                        exprTokens.Add(i);
                        break;
                    }
                }   
            }
            return null;
        }
        
        public override Node ParseTree()
        {
            List<Token> exprOne = _parseA();
            Node nodeOne = _parseB();
            
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTwo = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                if (i.Type == TokenType.LightWeight)
                {
                    exprOne.Add(new Token(TokenType.EoL, ";"));
                    exprTwo.Add(new Token(TokenType.EoL, ";"));
                    
                    return new BoolComparisonNode(
                        new Calculator(exprOne, ref VariableTable, false).ParseTree(), 
                        new Calculator(exprTwo, ref VariableTable, false).ParseTree(), 
                        nodeOne);
                }
                ScanToken();
                exprTwo.Add(i);
            }
            
            return null;
        }
        
        // public static void Main()
        // {
        //     Dictionary<String, Value> var = new Dictionary<String, Value>();
        //     Lexer lexer = new Lexer("!= 8");
        //     Boolean x = new Boolean(lexer.Tokens, ref var);
        //     Console.Out.WriteLine(x.ParseTree().Evaluate());
        // }
    }
}