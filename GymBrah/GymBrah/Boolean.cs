/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Selection.cs, Statement.cs  
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Boolean parse tree to parse boolean expressions, handling both integer and string comparisons.   
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Boolean parse tree that develops a relational expression, ensuring equal types.
    /// </summary>
    public class Boolean : Parse
    {
        public Boolean(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable) : 
            base(tokens, ref variableTable, ref functionTable)
        {}
        
        /// <summary>
        /// Parse method to get second part of a boolean expression (e.g. ==, !=) - if there is one - otherwise return
        /// null if there isn't.
        /// </summary>
        /// <param name="start"> First boolean comparison token. </param>
        /// <returns> Second boolean token/null. </returns>
        /// <exception cref="Exception"> Invalid boolean comparison error. </exception>
        private Node _parseC(Token start)
        {
            switch (CurrentToken.Type)
            {
                case TokenType.LessThan:
                case TokenType.GreaterThan:
                {
                    // Ensure the comparison "=>" cannot happen
                    if (start.Type == TokenType.Equals) throw new Exception("Invalid boolean comparison.");
                    goto case TokenType.Equals;
                }
                case TokenType.Equals:
                {
                    return new TerminalNode(ScanToken());
                }
                default:
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Parse method to evaluate first boolean token and get - if there is one - the second boolean token and link
        /// them together in the tree.
        /// </summary>
        /// <returns> Boolean comparison node. </returns>
        /// <exception cref="Exception"> Invalid boolean comparison. </exception>
        private Node _parseB()
        {
            Token start = ScanToken();
            Node nodeOne = _parseC(start);
            
            switch (start.Type)
            {
                case TokenType.Not:     // !
                case TokenType.Equals:  // =
                {
                    if (nodeOne == null) throw new Exception("Invalid boolean comparison.");
                    return new BoolStart(start, nodeOne);
                }
                case TokenType.LessThan:    // <
                case TokenType.GreaterThan: // >
                {
                    if (nodeOne == null) return new TerminalNode(start);
                    return new BoolStart(start, nodeOne);
                }
                default:
                {
                    throw new Exception("Unrecognised boolean token " + CurrentToken.Type); 
                }
            }
        }
        
        /// <summary>
        /// Parse method to separate the two expressions either side of the boolean comparison, by reading the boolean
        /// tokens and then returning the rest of the tokens.
        /// </summary>
        /// <returns> Boolean expression tokens. </returns>
        private List<Token> _parseA()
        {
            List<Token> boolTokens = GetRemainingTokens();
            List<Token> exprTokens = new List<Token>();
            
            foreach (var i in boolTokens)
            {
                // Read up to boolean comparison tokens
                switch (i.Type)
                {
                    case TokenType.Equals:
                    case TokenType.LessThan:
                    case TokenType.Not:
                    case TokenType.GreaterThan:
                    {
                        return exprTokens;
                    }
                    default:
                    {
                        ScanToken();
                        exprTokens.Add(i);
                        break;
                    }
                }   
            }
            return null;
        }
        
        /// <summary>
        /// Parse method to parse a complete boolean expression tree, returning a tree of the entire expression.
        /// </summary>
        /// <returns> Boolean parse tree node. </returns>
        /// <exception cref="Exception"> Invalid expression errors. </exception>
        public override Node ParseTree()
        {
            List<Token> exprOne = _parseA(); // Get first expression
            Node nodeOne = _parseB(); // Get boolean comparison
            
            List<Token> boolTokens = GetRemainingTokens(); // Tokens remaining after boolean comparison tokens
            List<Token> exprTwo = new List<Token>(); // Get second expression

            TokenType exprOneType;
            TokenType exprTwoType;
            
            foreach (var i in boolTokens)
            {
                if (i.Type == TokenType.LightWeight) // Reached end of boolean expression
                {
                    if (exprOne.Count == 1 && exprTwo.Count == 1) // Can only be id/string comparison
                    {
                        // Get expression one data type
                        if (exprOne[0].Type == TokenType.Id) 
                        {
                            // If identifier ensure it exists
                            if (VariableTable.TryGetValue(exprOne[0].Content, out Value result))
                            {
                                exprOneType = (TokenType)result.Type;
                            }
                            else
                            {
                                throw new Exception("Identifier not defined.");
                            }
                        }
                        else
                        {
                            exprOneType = exprOne[0].Type;
                        }
                        
                        // Get expression two data type
                        if (exprTwo[0].Type == TokenType.Id)
                        {
                            // If identifier ensure it exists
                            if (VariableTable.TryGetValue(exprTwo[0].Content, out Value result))
                            {
                                exprTwoType = (TokenType)result.Type;
                            }
                            else
                            {
                                throw new Exception("Identifier not defined.");
                            }
                        }
                        else
                        {
                            exprTwoType = exprTwo[0].Type;
                        }

                        if (exprOneType == exprTwoType) // Check types are equal
                        {
                            if (exprOneType == TokenType.String) // String comparison
                            {
                                return new BoolComparisonNode(new TerminalNode(exprOne[0]),
                                    new TerminalNode(exprTwo[0]), nodeOne);
                            }
                        }
                        else
                        {
                            throw new Exception("Comparison are not of same type.");
                        }
                    }
                    
                    // Integer/expression comparison
                    exprOne.Add(new Token(TokenType.EoL, ";"));
                    exprTwo.Add(new Token(TokenType.EoL, ";"));
                    
                    return new BoolComparisonNode(
                        new Calculator(exprOne, ref VariableTable, ref FunctionTable, false).ParseTree(), 
                        new Calculator(exprTwo, ref VariableTable, ref FunctionTable, false).ParseTree(), 
                        nodeOne);
                }
                ScanToken();
                exprTwo.Add(i);
            }
            
            throw new Exception("Incorrect boolean expression.");
        }
    }
}