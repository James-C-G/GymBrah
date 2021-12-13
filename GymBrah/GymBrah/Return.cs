/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs 
 * Last Modified :  13/12/21
 * Version :        1.4
 * Description :    Return class for the parsing of return statements and their return value.   
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Return statement parse tree for the parsing of return statements for functions.
    /// </summary>
    public class Return : Parse
    {
        private readonly bool _evaluate; // Bool to describe whether identifiers should be evaluated or not
        private readonly TokenType _returnType; // Function return type
        
        /// <summary>
        /// Inherited constructor with evaluate boolean for whether or not to compile variables.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        /// <param name="returnType"></param>
        /// <param name="evaluate"> Evaluate boolean. </param>
        /// <exception cref="Exception"> Datatype error. </exception>
        public Return(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable, TokenType returnType, bool evaluate = true) : 
            base(tokens, ref variableTable, ref functionTable)
        {
            _evaluate = evaluate;

            // Check function return value datatype is valid
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

        /// <summary>
        /// Parse method for the value being returned and ensuring it is of the correct type.
        /// </summary>
        /// <returns> Return value node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private Node _parseA()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    // Ensure identifier exists
                    if (VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                    {
                        if (!_evaluate) // Do not evaluate identifier
                        {
                            if (result.Type == _returnType) // Ensure return type is maintained
                            {
                                Token returnVal = CurrentToken;
                                ScanToken();
                                
                                if (CurrentToken.Type != TokenType.EoL) // Check for EoL
                                {
                                    throw new Exception("Return statement missing EoL.");
                                }

                                return new TerminalNode(returnVal); // Return value node
                            }

                            throw new Exception("Variable is not of correct return type.");
                        }
                            
                        // Get the type of the identifier
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

                        throw new Exception("Unrecognised return type.");
                    }

                    throw new Exception("Undefined variable cannot be returned.");
                }
                case TokenType.String:
                case TokenType.Integer:
                case TokenType.Double:
                {
                    Token returnVal = CurrentToken;
                    
                    if (CurrentToken.Type == _returnType) // Ensure matching type
                    {
                        ScanToken();
                        if (CurrentToken.Type != TokenType.EoL) // Ensure EoL
                        {
                            throw new Exception("Return statement missing EoL.");
                        }

                        return new TerminalNode(returnVal); // Return value node
                    }

                    throw new Exception(returnVal.Type + " is not the correct return type.");
                }
                default:
                {
                    throw new Exception("Unrecognised return type.");
                }
            }
        }
        
        /// <summary>
        /// Parse tree method to return the return parse tree node.
        /// </summary>
        /// <returns> Return parse tree. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        public override Node ParseTree()
        {
            // Ensure correct token
            switch (CurrentToken.Type)
            {
                case TokenType.Gain:
                {
                    // Get return value
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