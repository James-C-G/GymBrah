/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs  
 * Last Modified :  13/12/21
 * Version :        1.4
 * Description :    Selection parse tree to parse basic selection using boolean expressions.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Parse tree class for selection statements using boolean expression trees.
    /// </summary>
    public class Selection : Parse
    {
        private readonly bool _evaluate;    // Boolean of whether to evaluate

        /// <summary>
        /// Inherited constructor with boolean for the evaluation of expressions.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        /// <param name="evaluate"> Evaluate boolean. </param>
        public Selection(List<Token> tokens, ref Dictionary<String, Value> variableTable,
            ref Dictionary<String, Function> functionTable, bool evaluate = true) :
            base(tokens, ref variableTable, ref functionTable)
        {
            _evaluate = evaluate;
        }

        /// <summary>
        /// Parse method for getting remaining tokens (after "is" token) for boolean expression.
        /// </summary>
        /// <returns> List of tokens. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private List<Token> _parseB()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                switch (i.Type)
                {
                    case TokenType.LightWeight: // End of statement
                    {
                        exprTokens.Add(i);
                        return exprTokens;
                    }
                    case TokenType.Id:
                    case TokenType.Double:
                    case TokenType.Integer:
                    case TokenType.String:
                    case TokenType.Equals:
                    case TokenType.Not:
                    case TokenType.LessThan:
                    case TokenType.GreaterThan: // Add tokens to list
                    {
                        ScanToken();
                        exprTokens.Add(i);
                        break;
                    }
                    default:
                    {
                        throw new Exception("Unrecognised data type in selection statement.");
                    }
                }
            }

            throw new Exception("Selection statement not closed properly.");
        }
        
        /// <summary>
        /// Parse method for getting a list of tokens before the "is" token, for the boolean expression.
        /// </summary>
        /// <returns> List of tokens. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private List<Token> _parseA()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                switch (i.Type)
                {
                    case TokenType.Is: // End of first set of tokens
                    {
                        return exprTokens;
                    }
                    case TokenType.Id:
                    case TokenType.Double:
                    case TokenType.Integer:
                    case TokenType.String: // Only valid tokens before "is"
                    {
                        ScanToken();
                        exprTokens.Add(i);
                        break;
                    }
                    default:
                    {
                        throw new Exception("Unrecognised data type in selection statement.");
                    }
                }
            }

            throw new Exception("Unrecognised selection statement.");
        }

        /// <summary>
        /// Parse tree method for getting the parse tree node of a selection statement.
        /// </summary>
        /// <returns> Selection parse tree node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        public override Node ParseTree()
        {
            List<Token> nodeOne = _parseA(); // Get first list of tokens
            
            switch (CurrentToken.Type)
            {
                case TokenType.Is:
                {
                    Token cur = ScanToken();
                    List<Token> nodeTwo = _parseB(); // Get second list of tokens

                    nodeOne.AddRange(nodeTwo); // Merge lists
                    
                    // Build tree
                    return new SelectionNode(cur, new Boolean(nodeOne, ref VariableTable, ref FunctionTable, _evaluate).ParseTree());
                }
                default:
                {
                    throw new Exception("Unrecognised key word.");
                }
            }
        }
    }
}