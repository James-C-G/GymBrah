/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Selection.cs, Statement.cs  
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    Class to wrap around the whole parser and create the individual trees for parsing of the program and
 *                  then evaluate them.  
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private Dictionary<String, Value> _variableTable;
        private Dictionary<String, Function> _functionTable;
        private Lexer _lexer;
        private List<List<Token>> _lines;
        private readonly StringBuilder _outString = new StringBuilder();
        
        /// <summary>
        /// Constructor to initialise the path to the text file.
        /// </summary>
        /// <param name="path"> Text file to parse. </param>
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
        
        /// <summary>
        /// Split the list of tokens into individual lines.
        /// </summary>
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
                _lexer = new Lexer(new StreamReader(_path));
                _lines = new List<List<Token>>();
                _getLines();
            }
            catch (Exception e)
            {
                throw new Exception("Error: Syntax error " + e.Message);
            }
        }
        
        //TODO When parsing a function, create new gymbrah object with remaining tokens up til "}"
        //TODO When in a selection/repetition don't evaluate identifiers e.g. x = x + 1;

        /// <summary>
        /// Method to parse the entire inputted text file.
        /// </summary>
        /// <returns> String of parsed text. </returns>
        /// <exception cref="Exception"> Errors in parsing and lexing. </exception>
        public string Parse(bool parse = true)
        {
            // Initialise tables
            _variableTable = new Table().VariableTable;
            _functionTable = new Table().FunctionTable;
            
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

            //TODO Catch errors with hacks e.g. int x = 3? 3, int main(int x){, int y){
            
            // Catch any parse errors.
            try
            {
                for (int i = 0; i < _lines.Count; i++)
                {
                    List<Token> currentLine = _lines[i];
                    counter++; //TODO Maybe not use
                    
                    // Different type of line start
                    switch (currentLine[0].Type) 
                    {
                        case TokenType.Can: // Assignment
                        {
                            tree = new Assignment(currentLine, ref _variableTable, ref _functionTable, parse);
                            break;
                        }
                        case TokenType.Scream: // Statement
                        {
                            tree = new Statement(currentLine, ref _variableTable, ref _functionTable, parse);
                            break;
                        }
                        case TokenType.Baby: // End of brackets
                        {
                            _outString.AppendLine("}\n");
                            continue;
                        }
                        case TokenType.BroSplit: // Function definition
                        {
                            // TODO Catch return type
                            tree = new Functions(currentLine, ref _variableTable, ref _functionTable, parse);

                            int bracket = 0;
                            int j = i;
                                
                            for (; j < _lines.Count; j++)
                            {
                                if (_lines[j].Last().Type == TokenType.LightWeight)
                                {
                                    bracket++;
                                }
                                if (_lines[j][0].Type == TokenType.Baby)
                                {
                                    bracket--;

                                    if (bracket < 0)
                                    {
                                        throw new Exception("Braces not closed properly.");
                                    }
                                    if (bracket == 0)
                                    {
                                        _outString.AppendLine(tree.ParseTree().Evaluate());
                                        string scope = new GymBrah(_lines.GetRange(i + 1, j - 1)).Parse();
                                        
                                        if (scope.Split('\n').Last().Contains("Error"))
                                            throw new Exception(scope.Split('\n').Last().Split(':')[1]);
                                        
                                        _outString.Append(scope);
                                        
                                        i += j - 1;
                                        break;
                                    }
                                }
                            }

                            if (bracket != 0) throw new Exception("Braces not closed properly.");
                            
                            continue;
                        }
                        case TokenType.DropSet: // Repetition
                        {
                            tree = new Repetition(currentLine, ref _variableTable, ref _functionTable);
                            break;
                        }
                        case TokenType.Id: // Selection
                        {
                            // If in function table, function call, else selection
                            if (_functionTable.TryGetValue(currentLine[0].Content, out Function result))
                            {
                                tree = new Functions(currentLine, ref _variableTable, ref _functionTable, parse);
                                break;
                            }
                            
                            goto case TokenType.String;
                        }
                        case TokenType.Integer:
                        case TokenType.Double:
                        case TokenType.String:
                        {
                            tree = new Selection(currentLine, ref _variableTable, ref _functionTable);
                            counter += 2;
                            break;
                        }
                        default:
                        {
                            throw new Exception("Error: Invalid statement.");
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
            
            return _outString.ToString();
        }
    }
}