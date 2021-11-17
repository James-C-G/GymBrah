using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace Assignment
{
    public class LeftAssignment : Parse
    {
        public LeftAssignment(List<Token> tokens) : base(tokens){}

        private AssignmentNode _parseType(Token previous)
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Integer:
                case TokenType.Id:
                {
                    return new TerminalNode(previous);
                }
                case TokenType.Bench:
                {
                    return new AssignmentVariableNode(previous, new TerminalNode(CurrentToken));
                }
                default:
                {
                    throw new Exception("Error");
                }
            }
        }
        
        public AssignmentNode ParseLeft()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    Token next = PeekToken();

                    if (next == null)
                    {
                        throw new Exception("error");
                    }

                    Token previous = ScanToken();
                    return _parseType(previous);
                }
                default:
                {
                    throw new Exception("Incorrect assignment.");
                }
            }
        }
    }
}