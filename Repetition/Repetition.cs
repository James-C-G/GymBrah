using System;
using System.Collections.Generic;
using Boolean;
using Compiler;
using Tokenizer;

namespace Repetition
{
    public class Repetition : Parse
    {
        public Repetition(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.DropSet:
                {
                    Token cur = ScanToken();

                    return new SelectionNode(cur, new Boolean.Boolean(GetRemainingTokens(), ref VariableTable).ParseTree());
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
            
            Lexer lexer = new Lexer("dropset x == 5 lightweight");
            Repetition x = new Repetition(lexer.Tokens, ref var);
            
            Console.Out.WriteLine(x.ParseTree().Evaluate());
        }
    }
}