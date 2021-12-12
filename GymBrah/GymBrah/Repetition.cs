/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Selection.cs, Statement.cs 
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
        public Repetition(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable) : 
            base(tokens, ref variableTable, ref functionTable)
        {}

        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.DropSet:
                {
                    Token cur = ScanToken();

                    return new SelectionNode(cur, new Boolean(GetRemainingTokens(), ref VariableTable, ref FunctionTable).ParseTree());
                }
                default:
                {
                    return null;  //TODO Throw errors
                }
            }
        }
    }
}