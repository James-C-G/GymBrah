using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace Assignment
{
    public class RightAssignment : Parse
    {
        public RightAssignment(List<Token> tokens) : base(tokens){}

        public AssignmentNode ParseRight()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Integer:
                {
                    List<Token> calcTokens = GetRemainingTokens();
                    string calculation = new Calculator.Calculator(calcTokens).Expression().Evaluate().ToString();
                    
                    return new TerminalNode(new Token(TokenType.Integer, calculation));
                }
                case TokenType.Id:
                {
                    Token next = PeekToken();
                    if (next.Type == TokenType.EoL) 
                    {
                        return new TerminalNode(CurrentToken);
                    }
                    else
                    {
                        List<Token> calcTokens = GetRemainingTokens();
                        string calculation = new Calculator.Calculator(calcTokens).Expression().Evaluate().ToString();
                    
                        return new TerminalNode(new Token(TokenType.Integer, calculation));
                    }
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}