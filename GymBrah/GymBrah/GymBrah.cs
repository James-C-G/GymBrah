/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, GymBrah.cs, Program.cs, Repetition.cs, Selection.cs,
 *                  Statement.cs 
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Class to wrap around the whole parser and create the individual trees for parsing of the program and
 *                  then evaluate them.  
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Class for the parsing of the language with a text file path as the input.
    /// </summary>
    public class GymBrah
    {
        private readonly string _path;
        private Dictionary<String, FunctionTable> _functionTable;
        private Dictionary<String, Value> _variableTable;
        private Lexer _lexer;
        private List<List<Token>> _lines;
<<<<<<< Updated upstream
        private StringBuilder _outString = new StringBuilder();
        
        /// <summary>
        /// Constructor to initialise the path to the text file.
        /// </summary>
        /// <param name="path"> Text file to parse. </param>
=======

>>>>>>> Stashed changes
        public GymBrah(string path)
        {
            _path = path;
            _outString.AppendLine("#include <stdio.h>\n");
        }
        
        /// <summary>
        /// Constructor to initialise parser with a list of tokens split into lines.
        /// </summary>
        /// <param name="lines"> List of tokens split into lines. </param>
        public GymBrah(List<List<Token>> lines)
        {
            _lines = lines;
        }
<<<<<<< Updated upstream
        
        /// <summary>
        /// Split the list of tokens into individual lines.
        /// </summary>
=======

>>>>>>> Stashed changes
        private void _getLines()
        {
            List<Token> temp = new List<Token>();

            foreach (var i in _lexer.Tokens)
            {
                switch (i.Type) // End line tokens
                {
                    case TokenType.LightWeight:
                    case TokenType.Baby:
                    case TokenType.EoL:
                        {
                            temp.Add(i);
                            _lines.Add(temp);
                            temp = new List<Token>();
                            break;
                        }
                    default:
                        {
                            temp.Add(i);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Lex method to pass the given file to the lexer for analysis.
        /// </summary>
        /// <exception cref="Exception"> Lexical analysis errors. </exception>
        private void _lex()
        {
            try
            {
                _variableTable = new Table().VariableTable;
                _lexer = new Lexer(new StreamReader(_path));
                _lines = new List<List<Token>>();
                _getLines();
            }
            catch (Exception e)
            {
                throw new Exception("Syntax error: " + e.Message);
            }
<<<<<<< Updated upstream
        }
        
        //TODO When parsing a function, create new gymbrah object with remaining tokens up til "}"
        //TODO When in a selection/repetition don't evaluate identifiers e.g. x = x + 1;

        /// <summary>
        /// Method to parse the entire inputted text file.
        /// </summary>
        /// <returns> String of parsed text. </returns>
        /// <exception cref="Exception"> Errors in parsing and lexing. </exception>
        public string Parse()
        {
            // Perform lexing, if necessary, and catch any errors
            if (_lines == null)
            {
                try
                {
                    _lex();

                    if (_lines.Count == 0) throw new Exception("Error, unrecognised input code.");
                }
                catch (Exception e)
                {
                    _outString.AppendLine(e.Message);
                    return _outString.ToString();
                }
            }

            int counter = 0;
            Parse tree;
            
            _outString.AppendLine("int main()\n{");
            
            // Catch any parse errors.
=======

            int counter = 0;
            Parse tree;

            Console.Out.WriteLine("#include <stdio.h>\nint main()\n{");

>>>>>>> Stashed changes
            try
            {
                foreach (var i in _lines)
                {
                    counter++; //TODO Maybe not use
                    
                    // Different type of line start
                    switch (i[0].Type) 
                    {
<<<<<<< Updated upstream
                        case TokenType.Can: // Assignment
                        {
                            tree = new Assignment(i, ref _variableTable);
                            break;
                        }
                        case TokenType.Scream: // Statement
                        {
                            tree = new Statement(i, ref _variableTable);
                            break;
                        }
                        case TokenType.Baby: // End of brackets
                        {
                            _outString.AppendLine("}\n");
                            continue;
                        }
                        case TokenType.DropSet: // Repetition
                        {
                            tree = new Repetition(i, ref _variableTable); //TODO False parseMaths
                            // Gymbrah...
                            counter += 2;
                            break;
                        }
                        case TokenType.Id: // Selection
                        case TokenType.Integer:
                        case TokenType.String:
                        {
                            tree = new Selection(i, ref _variableTable);
                            counter += 2;
                            break;
                        }
=======
                        case TokenType.Can:
                            {
                                tree = new Assignment(i, ref _variableTable,ref _functionTable);
                                break;
                            }
                        case TokenType.Brosplit:
                            {
                                tree = new Functions(i, ref  _variableTable, ref _functionTable);
                                break;
                            }
                        case TokenType.Scream:
                            {
                                tree = new Statement(i, ref _variableTable, ref _functionTable);
                                break;
                            }
                        case TokenType.Baby:
                            {
                                Console.Out.WriteLine("}");
                                continue;
                            }
                        case TokenType.DropSet:
                            {
                                tree = new Repetition(i, ref _variableTable, ref _functionTable);
                                counter++;
                                break;
                            }
                        case TokenType.Id:
                        case TokenType.Integer:
                            {
                                tree = new Selection(i, ref _variableTable, ref _functionTable);
                                counter++;
                                break;
                            }
>>>>>>> Stashed changes
                        default:
                            {
                                throw new Exception("Invalid statement.");
                            }
                    }
                    
                    _outString.AppendLine(tree.ParseTree().Evaluate());
                }
            }
            catch (Exception e)
            {
                _outString.Append("Error on line " + counter + ": " + e.Message);
                return _outString.ToString();
            }

<<<<<<< Updated upstream
            _outString.AppendLine("}");
            return _outString.ToString();
        }
=======

            //TODO Case "3 plates 3" - doesnt do anything
            //TODO Force plates to be last thing, if used


            Console.Out.WriteLine("}");
        }

>>>>>>> Stashed changes
    }
}