/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Selection.cs, Statement.cs  
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Selection parse tree to parse basic selection using boolean expressions.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class Selection : Parse
    {
        public Selection(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable) : 
            base(tokens, ref variableTable, ref functionTable)
        {}

        private List<Token> _parseB()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                if (i.Type == TokenType.LightWeight)
                {
                    exprTokens.Add(i);
                    return exprTokens;
                }
                ScanToken();
                exprTokens.Add(i);
            }

            return null; // TODO Throw errors
        }
        
        private List<Token> _parseA()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                if (i.Type == TokenType.Is)
                {
                    return exprTokens;
                }
                ScanToken();
                exprTokens.Add(i);
            }

            return null;  //TODO Throw errors
        }

        public override Node ParseTree()
        {
            List<Token> nodeOne = _parseA();
            
            switch (CurrentToken.Type)
            {
                case TokenType.Is:
                {
                    Token cur = ScanToken();
                    List<Token> nodeTwo = _parseB();

                    nodeOne.AddRange(nodeTwo);
                    
                    return new SelectionNode(cur, new Boolean(nodeOne, ref VariableTable, ref FunctionTable).ParseTree());
                }
                default:
                {
                    throw new Exception("Unrecognised key word.");
                }
            }
        }
    }
}