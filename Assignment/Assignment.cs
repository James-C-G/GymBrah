using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;


namespace Assignment
{
    public class Assignment : Parse
    {
        public Assignment(List<Token> tokens) : base(tokens){}

        public AssignmentNode ParseAssignment()
        {
            if (CurrentToken.Type == TokenType.Can)
            {
                Token start = CurrentToken;
                
                ScanToken();

                AssignmentNode left = new LeftAssignment(GetRemainingTokens()).ParseLeft();
                
                ScanToken();
                
                if (CurrentToken.Type != TokenType.Id && CurrentToken.Type != TokenType.Integer)
                {
                    ScanToken();
                }
                
                AssignmentNode right = new RightAssignment(GetRemainingTokens()).ParseRight();
                
                return new EqualNode(left, right, start);
            }

            throw new Exception("Not assignment.");
        }
        
        static void Main()
        {
            Lexer x = new Lexer("can x bench 2?");
            Assignment y = new Assignment(x.Tokens);
            string z = y.ParseAssignment().Evaluate();
            Console.Out.WriteLine(z);
        }
    }
}