using System;
using System.Collections.Generic;
using Boolean;
using Compiler;
using Tokenizer;

namespace Selection
{
    public class Selection : Parse
    {
        public Selection(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        private List<Token> _parseB()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                if (i.Type == TokenType.LightWeight)
                {
                    exprTokens.Add(i);
                    return exprTokens;
                }
                ScanToken();
                exprTokens.Add(i);
            }

            return null;
        }
        
        private List<Token> _parseA()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                if (i.Type == TokenType.Is)
                {
                    return exprTokens;
                }
                ScanToken();
                exprTokens.Add(i);
            }

            return null;
        }

        public override Node ParseTree()
        {
            List<Token> nodeOne = _parseA();
            
            switch (CurrentToken.Type)
            {
                case TokenType.Is:
                {
                    Token cur = ScanToken();
                    List<Token> nodeTwo = _parseB();

                    nodeOne.AddRange(nodeTwo);
                    
                    return new SelectionNode(cur, new Boolean.Boolean(nodeOne, ref VariableTable).ParseTree());
                }
                default:
                {
                    return null;
                }
            }
        }

        public static void Main()
        {
            Dictionary<String, Value> var = new Dictionary<String, Value>();
            var.Add("x", new IntegerValue("2"));
            
            Lexer lexer = new Lexer("x+3 is == 5-x lightweight");
            Selection x = new Selection(lexer.Tokens, ref var);
            Console.Out.WriteLine(x.ParseTree().Evaluate());
        }
    }
}