/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs, Parser.cs, Syntax Tree.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */


using System;
using System.Collections.Generic;
using Tokenizer;

namespace Parser
{
    public class Parser
    {
        
        public List<Token> _tokens; // TODO Make private
        private Token _curToken;
        private Token _peekToken;
        private SyntaxTree ParseTree;

        public Parser(String inStream)
        {
            _tokens = new Lexer(inStream).Tokens;
        }

        public String Parse()
        {
            String output = "";
            
            if (_tokens.Count > 2)
            {
                _curToken = _tokens[0];
                _peekToken = _tokens[1];
            }
            else
            {
                // TODO not enough code
                return output;
            }

            while (_curToken.type != TokenType.EoF)
            {
                ParseTree = new SyntaxTree();

                try
                {
                    while (_curToken.type != TokenType.Question)
                    {
                        if (_curToken.type == TokenType.Can && _peekToken.type == TokenType.You)
                        {
                            ParseTree.Root = new Node("= ");
                            _nextToken();
                        }
                        else if (_curToken.type == TokenType.Bench)
                        {
                            ParseTree.Root.LeftChild = new Node("", new Node("int"), new Node());
                        }
                        else if (_curToken.type == TokenType.Int && _peekToken.type == TokenType.Plates)
                        {
                            ParseTree.Root.RightChild = new Node("", new Node(_curToken.contents), new Node("* 40 + 20"));
                            _nextToken();
                        }
                        else if (_curToken.type == TokenType.Var)
                        {
                            try
                            {
                                ParseTree.Root.LeftChild.RightChild = new Node(_curToken.contents);
                            }
                            catch (Exception e)
                            {
                                throw new Exception("Syntax error");
                            }
                        }
                        else
                        {
                            throw new Exception("Syntax error");
                        }
                
                        _nextToken();
                    }
                    _nextToken();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                output += ParseTree.ToString();
                output += ";\n";
            }

            return output;
        }

        private void _nextToken()
        {
            _curToken = _peekToken;
            _peekToken = _tokens[(_tokens.IndexOf(_peekToken) + 1) % _tokens.Count];
        }
    }
}