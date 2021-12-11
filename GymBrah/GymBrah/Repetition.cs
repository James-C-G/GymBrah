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
        public Repetition(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, FunctionTable> functions) : base(tokens, ref variableTable,ref functions)
        {}

        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Brosplit:
                {
                    Token cur = ScanToken();

                        return null;
                }
                default:
                {
                    return null;  //TODO Throw errors
                }
            }
        }
    }
}