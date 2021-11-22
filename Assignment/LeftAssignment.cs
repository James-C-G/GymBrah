using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace Assignment
{
    public class LeftAssignment : Parse<string>
    {
        public string _curVar;
        public LeftAssignment(List<Token> tokens, ref Dictionary<String, Value> variableTable,
            ref string currentVariable)
            : base(tokens, ref variableTable)
        {
            _curVar = currentVariable;
        }

        private AssignmentNode _parseType()
        {
            Token node = ScanToken();
            switch (CurrentToken.Type)
            {
                // if next token is an in or id, no bench in between 
                case TokenType.Integer:
                case TokenType.Id:
                {
                    return new TerminalNode(node);
                }
                case TokenType.Bench:
                {
                    return new AssignmentVariableNode(node, new TerminalNode(CurrentToken));
                }
                default:
                {
                    throw new Exception("Error");
                }
            }
        }
        
        public override AssignmentNode ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    Token next = PeekToken();

                    if (next == null) // int x; is not allowed
                    {
                        throw new Exception("error");
                    }
                    
                    // Store variable being assigned
                    _curVar = CurrentToken.StringContent;
                    VariableTable.TryAdd(_curVar, null);

                    // TODO Check ID exists
                    
                    return _parseType();
                }
                default:
                {
                    throw new Exception("Incorrect assignment.");
                }
            }
        }
    }
}