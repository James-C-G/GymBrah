using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;


namespace Assignment
{
    public class Assignment : Parse<string>
    {
        public string CurrentVariable; // TODO Make this work better

        public Assignment(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens,
            ref variableTable){}
        
        public override Node<string> ParseTree()
        {
            if (CurrentToken.Type == TokenType.Can)
            {
                Token start = CurrentToken;
                
                ScanToken();

                AssignmentNode left = new LeftAssignment(GetRemainingTokens(), ref VariableTable, ref CurrentVariable).ParseTree();
                
                Token x = ScanToken();

                CurrentVariable = x.StringContent; // TODO Fix this
                
                // TODO Make this if better
                if (CurrentToken.Type != TokenType.Id && CurrentToken.Type != TokenType.Integer)
                {
                    ScanToken();
                }

                AssignmentNode right = new RightAssignment(GetRemainingTokens(), ref VariableTable, ref CurrentVariable).ParseTree();
                
                return new EqualNode(left, right, start);
            }

            throw new Exception("Not assignment.");
        }
        
        static void Main()
        {
            Lexer x = new Lexer("can x bench 2?");
            // Assignment y = new Assignment(x.Tokens);
            // string z = y.ParseAssignment().Evaluate();
            // Console.Out.WriteLine(z);
            /*
             *  x = cur
             *  scan
             *  case id
             *  
             * 
             */
        }
    }
}