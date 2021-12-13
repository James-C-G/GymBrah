/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs  
 * Last Modified :  13/12/21
 * Version :        1.4
 * Description :    Statement parse tree to parse basic statements.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Statement parse tree class.
    /// </summary>
    public class Statement : Parse
    {
        private bool _evaluate; // Bool to describe whether identifiers should be evaluated or not

        /// <summary>
        /// Inherited constructor with evaluate boolean for whether or not to compile variables.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        /// <param name="evaluate"> Evaluate boolean. </param>
        public Statement(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable, bool evaluate = true) : 
            base(tokens, ref variableTable, ref functionTable)
        {
            _evaluate = evaluate;
        }

        /// <summary>
        /// Parse method to handle key words of statements and EoL.
        /// </summary>
        /// <returns> Statement keyword node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private Node _parseB()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Scream:
                {
                    return new TerminalNode(ScanToken());
                }
                case TokenType.EoL:
                {
                    return new TerminalNode(CurrentToken);
                }
                default:
                {
                    throw new Exception("Unrecognised statement token " + CurrentToken.Type);
                }
            }
        }

        /// <summary>
        /// Parse method to return the entire parsed statement. The print statement is parsed such that is can take
        /// either an identifier or string argument.
        /// </summary>
        /// <returns> Statement parse tree node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        public override Node ParseTree()
        {
            Node nodeOne = _parseB();
            Token curToken = CurrentToken;
            ScanToken();
            
            switch (curToken.Type)
            {
                case TokenType.String: // Print out a literal string
                {
                    curToken.Content += ")";
                    return new ScreamContentNode(nodeOne, _parseB(), curToken);
                }
                case TokenType.Double:  // Print out a literal double
                case TokenType.Integer: // Print out a literal integer
                {
                    curToken = new Token(TokenType.String, "\"" + curToken.Content + "\")");
                    return new ScreamContentNode(nodeOne, _parseB(), curToken);
                }
                case TokenType.Id: // Print out a variable
                {
                    if (VariableTable.TryGetValue(curToken.Content, out Value result))
                    {
                        if (result.Content == "function") _evaluate = false;

                        switch(result.Type)
                        {
                            case TokenType.String: // If string use %s
                            {
                                if (!_evaluate) curToken.Content = "\"%s\"," + curToken.Content + ")";
                                else curToken = new Token(TokenType.String, "\"%s\"," + ((StringValue) result).Content + ")");
                                break;
                            }
                            case TokenType.Integer: // If integer use %d
                            {
                                if (!_evaluate) curToken.Content = "\"%d\"," + curToken.Content + ")";
                                else curToken = new Token(TokenType.String, "\"%d\"," + ((IntegerValue) result).Content + ")");
                                break;
                            }
                            case TokenType.Double: // If double use %f
                            {
                                if (!_evaluate) curToken.Content = "\"%f\"," + curToken.Content + ")";
                                else curToken = new Token(TokenType.String, "\"%f\"," + ((DoubleValue) result).Content + ")");
                                break;
                            }
                            default:
                            {
                                throw new Exception("Can't print value of type " + CurrentToken.Type);
                            }
                        }
                        
                        // Statement parse tree
                        return new ScreamContentNode(nodeOne, _parseB(), curToken);
                    }

                    throw new Exception("Variable is not defined.");
                }
                default:
                {
                    throw new Exception("Unrecognised variable token in statement " + CurrentToken.Type);;
                }
            }
        }
    }
}