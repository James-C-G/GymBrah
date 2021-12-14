/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Calculator parse tree that parses mathematical expressions. Returns a parse tree of of either
 *                  integer/double nodes that when evaluated return a single integer/double output of the evaluated
 *                  expression, or string nodes which when evaluated return the entire expression in string form.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Calculator parse tree class that builds a mathematical expression tree that can be evaluated into a single
    /// integer value, or an entire string for the expression.
    /// </summary>
    public class Calculator : Parse
    {
        // Boolean for either return of string of expression or compilation of expression
        private bool _parseMaths; 

        /// <summary>
        /// Inherited constructor and a boolean for either the string or integer evaluation.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        /// <param name="parseMaths"> Boolean for type of evaluation. </param>
        public Calculator(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable, bool parseMaths = true) : 
            base(tokens, ref variableTable, ref functionTable)
        {
            _parseMaths = parseMaths;
        }

        /// <summary>
        /// Method for parsing the individual factors of an expression.
        /// </summary>
        /// <returns> Node of a factor. </returns>
        /// <exception cref="Exception"> Errors in parsing. </exception>
        private Node _parseFactor()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Negate:
                {
                    ScanToken();
                    if (_parseMaths) return new NegateNode(_parseFactor());
                    return new NegateNodeString(_parseFactor());
                }
                case TokenType.Double:
                case TokenType.Integer:
                {
                    return new TerminalNode(ScanToken());
                }
                case TokenType.Id: // Identifier
                {
                    Token idToken = ScanToken();
                    
                    // Check identifier exists
                    if (VariableTable.TryGetValue(idToken.Content, out Value result))
                    {
                        if (result.Content == "function") _parseMaths = false;
                        
                        if (result.Type == TokenType.Integer)
                        {
                            // Check that variable is of the right type
                            if (_parseMaths) return new TerminalNode(new Token(TokenType.Id, ((IntegerValue)result).Content));
                            return new TerminalNode(idToken);
                        }
                        if (result.Type == TokenType.Double)
                        {
                            // Check that variable is of the right type
                            if (_parseMaths) return new TerminalNode(new Token(TokenType.Id, ((DoubleValue)result).Content));
                            return new TerminalNode(idToken);
                        }

                        throw new Exception("Variable is not an integer or double.");
                    }

                    throw new Exception("Variable is not defined.");
                }
                case TokenType.OpenBracket:
                {
                    ScanToken();
                    
                    // Parse contents of brackets
                    Node node = ParseTree();
                    
                    // Parse until a close bracket
                    if(CurrentToken.Type == TokenType.CloseBracket)
                    {
                        ScanToken();
                        return _parseMaths ? node : new BracketNodeString(node);
                    }

                    throw new Exception("Bracket not closed properly.");
                }
                default:
                {
                    throw new Exception("Invalid token in calculation " + CurrentToken.Content); 
                }
            }
        }
        
        /// <summary>
        /// Method for parsing terms of a mathematical expression.
        /// </summary>
        /// <returns> Node of a term. </returns>
        private Node _parseTerm()
        {
            Node nodeOne = _parseFactor();

            while (true)
            {
                // Parse factors for BIDMAS maintenance
                switch (CurrentToken.Type)
                {
                    case TokenType.Multiply: 
                    {
                        ScanToken();
                        Node nodeTwo = _parseFactor();
                        
                        nodeOne = _parseMaths ? new MultiplyNode(nodeOne, nodeTwo) : new MultiplyNodeString(nodeOne, nodeTwo);
                        break;
                    }
                    case TokenType.Divide:
                    {
                        ScanToken();
                        Node nodeTwo = _parseFactor();
                        
                        nodeOne = _parseMaths ? new DivideNode(nodeOne, nodeTwo) : new DivideNodeString(nodeOne, nodeTwo);
                        break;
                    }
                    default:
                    {
                        return nodeOne;
                    }
                }
            }
        }
        
        /// <summary>
        /// Parse method that parses the list of given tokens and returns a node which can be evaluated.
        /// </summary>
        /// <returns> Parse tree node. </returns>
        public override Node ParseTree()
        {
            Node nodeOne = _parseTerm();
            
            while (true)
            {
                // Parse for BIDMAS maintenance
                switch (CurrentToken.Type)
                {
                    case TokenType.Addition:
                    {
                        ScanToken();
                        Node nodeTwo = _parseTerm();
                    
                        nodeOne = _parseMaths ? new AddNode(nodeOne, nodeTwo) : new AddNodeString(nodeOne, nodeTwo);

                        break;
                    }
                    case TokenType.Subtraction:
                    {
                        ScanToken();
                        Node nodeTwo = _parseTerm();
                    
                        nodeOne = _parseMaths ? new SubtractNode(nodeOne, nodeTwo) : new SubtractNodeString(nodeOne, nodeTwo);

                        break;
                    }
                    default:
                    {
                        return nodeOne;
                    }
                }
            }
        }
    }
}