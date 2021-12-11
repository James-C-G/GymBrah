/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, GymBrah.cs, Program.cs, Repetition.cs, Selection.cs,
 *                  Statement.cs 
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Statement parse tree to parse basic statements.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace GymBrah
{
    /// <summary>
    /// Statement parse tree class.
    /// </summary>
    public class Statement : Parse
    {
<<<<<<< Updated upstream
        private readonly bool _evaluate; // Bool to describe whether identifiers should be evaluated or not

        public Statement(List<Token> tokens, ref Dictionary<String, Value> variableTable, bool evaluate = true) : 
            base(tokens, ref variableTable)
        {
            _evaluate = evaluate;
        }
=======
        public Statement(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String,FunctionTable> functions) : base(tokens, ref variableTable, ref functions)
        {}
>>>>>>> Stashed changes

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
                case TokenType.Integer: // Print out a literal integer
                {
                    curToken = new Token(TokenType.String, "\"" + curToken.Content + "\"");
                    return new ScreamContentNode(nodeOne, _parseB(), curToken);
                }
                case TokenType.Id: // Print out a variable
                {
                    if (VariableTable.TryGetValue(curToken.Content, out Value result))
                    {
                        switch(result.Type)
                        {
                            case ValueType.String: // If string use %s
                            {
                                if (!_evaluate) curToken.Content = "\"%s\"," + curToken.Content + ")";
                                else curToken = new Token(TokenType.String, ((StringValue) result).Content + ")");
                                break;
                            }
                            case ValueType.Integer: // If integer use %d
                            {
                                if (!_evaluate) curToken.Content = "\"%d\"," + curToken.Content + ")";
                                else curToken = new Token(TokenType.String, "\"" + ((IntegerValue) result).Content + "\")");
                                break;
                            }
                            default:
                            {
                                throw new Exception("Can't print value of type " + CurrentToken.Type);
                            }
                        }
                        
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
<<<<<<< Updated upstream
=======

/*        public static void Main()
        {
            Lexer lexer = new Lexer("scream y?");
            Dictionary<String, Value> var = new Dictionary<String, Value>();
            var.Add("x", new StringValue("\"output\""));
            var.Add("y", new IntegerValue("10"));

            Statement x = new Statement(lexer.Tokens, ref var);
            Console.Out.WriteLine(x.ParseTree().Evaluate());
        }*/
>>>>>>> Stashed changes
    }
}