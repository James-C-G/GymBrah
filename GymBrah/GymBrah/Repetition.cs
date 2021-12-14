/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs 
 * Last Modified :  13/12/21
 * Version :        1.4
 * Description :    Repetition parse tree to parse the while loops using boolean expressions.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Parse tree class for repetition statements using boolean expression trees.
    /// </summary>
    public class Repetition : Parse
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        public Repetition(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable) : 
            base(tokens, ref variableTable, ref functionTable)
        {}

        /// <summary>
        /// Parse tree method for parsing a repetition tree.
        /// </summary>
        /// <returns> Repetition parse tree node. </returns>
        /// <exception cref="Exception"></exception>
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.DropSet:
                {
                    Token cur = ScanToken(); // Get "while" keyword token
                    List<Token> whileTokens = GetRemainingTokens(); // Get remaining tokens

                    if (whileTokens.Last().Type == TokenType.LightWeight) // Ensure EoL
                    {
                        // Build repetition tree
                        return new SelectionNode(cur, new Boolean(whileTokens, ref VariableTable, ref FunctionTable, false).ParseTree());
                    }

                    throw new Exception("Missing braces in repetition statement.");
                }
                default:
                {
                    throw new Exception("Unrecognised repetition statement.");
                }
            }
        }
    }
}