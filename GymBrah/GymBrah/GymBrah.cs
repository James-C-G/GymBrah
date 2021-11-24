using System;
using System.Collections.Generic;
using System.IO;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class GymBrah
    {
        private readonly string _path;
        private Dictionary<String, Value> _variableTable;
        private Lexer _lexer;
        private List<List<Token>> _lines;
        
        public GymBrah(string path)
        {
            _path = path;
        }
        
        private void _getLines()
        {
            List<Token> temp = new List<Token>();

            foreach (var i in _lexer.Tokens)
            {
                switch (i.Type)
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

        public void Parse()
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
                Console.WriteLine("Syntax error: " + e.Message);
                return;
            }
            
            int counter = 0;
            Parse tree;
            
            Console.Out.WriteLine("#include <stdio.h>\nint main()\n{");
            
            try
            {
                foreach (var i in _lines)
                {
                    counter++;
                    switch (i[0].Type)
                    {
                        case TokenType.Can:
                        {
                            tree = new Assignment(i, ref _variableTable);
                            break;
                        }
                        case TokenType.Scream:
                        {
                            tree = new Statement(i, ref _variableTable);
                            break;
                        }
                        case TokenType.Baby:
                        {
                            Console.Out.WriteLine("}");
                            continue;
                        }
                        case TokenType.DropSet:
                        {
                            tree = new Repetition(i, ref _variableTable);
                            counter++;
                            break;
                        }
                        case TokenType.Id:
                        case TokenType.Integer:
                        {
                            tree = new Selection(i, ref _variableTable);
                            counter++;
                            break;
                        }
                        default:
                        {
                            throw new Exception("Invalid statement.");
                        }
                    }

                    Console.Out.WriteLine(tree.ParseTree().Evaluate());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error on line " + counter + ": " + e.Message);
                return;
            }
            
            
            //TODO Case "3 plates 3" - doesnt do anything
            //TODO Force plates to be last thing, if used
            
            
            Console.Out.WriteLine("}");
        }
        
    }
}