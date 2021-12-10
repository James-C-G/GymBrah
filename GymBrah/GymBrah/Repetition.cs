/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, GymBrah.cs, Program.cs, Repetition.cs, Selection.cs,
 *                  Statement.cs 
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Repetition parse tree to parse the while loops using boolean expressions.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class Repetition : Parse
    {
        public Repetition(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.DropSet:
                {
                    Token cur = ScanToken();

                    return new SelectionNode(cur, new Boolean(GetRemainingTokens(), ref VariableTable).ParseTree());
                }
                default:
                {
                    return null;  //TODO Throw errors
                }
            }
        }
    }
}