using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class Return : Parse
    {
        private readonly bool _evaluate; // Bool to describe whether identifiers should be evaluated or not
        private readonly TokenType _returnType;
        
        public Return(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable, TokenType returnType, bool evaluate = true) : 
            base(tokens, ref variableTable, ref functionTable)
        {
            _evaluate = evaluate;

            switch (returnType)
            {
                case TokenType.Bench:
                {
                    _returnType = TokenType.Integer;
                    break;
                }
                case TokenType.Squat:
                {
                    _returnType = TokenType.String;
                    break;
                }
                case TokenType.DeadLift:
                {
                    _returnType = TokenType.Double;
                    break;
                }
                default:
                {
                    throw new Exception("Unrecognised type.");
                }
            }
        }

        private Node _parseA()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    if (VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                    {
                        if (!_evaluate)
                        {
                            if (result.Type == _returnType)
                            {
                                Token returnVal = CurrentToken;
                                ScanToken();
                                if (CurrentToken.Type != TokenType.EoL)
                                {
                                    throw new Exception("Return statement missing EoL.");
                                }

                                return new TerminalNode(returnVal);
                            }

                            throw new Exception("Variable is not of correct return type.");
                        }
                            
                        if (result.Type == TokenType.Integer)
                        {
                            goto case TokenType.Integer;
                        }
                        if (result.Type == TokenType.String)
                        {
                            goto case TokenType.String;
                        }
                        if (result.Type == TokenType.Double)
                        {
                            goto case TokenType.Double;
                        }

                        throw new Exception("Unrecognised type.");
                    }

                    throw new Exception("Undefined variable cannot be returned.");
                }
                case TokenType.String:
                case TokenType.Integer:
                case TokenType.Double:
                {
                    Token returnVal = CurrentToken;
                    
                    if (CurrentToken.Type == _returnType)
                    {
                        ScanToken();
                        if (CurrentToken.Type != TokenType.EoL)
                        {
                            throw new Exception("Return statement missing EoL.");
                        }

                        return new TerminalNode(returnVal);
                    }

                    throw new Exception(returnVal.Type + " is not the correct return type.");
                }
                default:
                {
                    throw new Exception("Unrecognised return type.");
                }
            }
        }
        
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Yeet:
                {
                    return new ReturnNode(ScanToken(), _parseA());
                }
                default:
                {
                    throw new Exception("Unrecognised return statement.");
                }
            }
        }
    }
}