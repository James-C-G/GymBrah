/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */


using System;
using System.Collections.Generic;
using System.Linq;

namespace Tokenizer
{
    public class Lexer
    {
        public List<Token> Tokens { get; }

        private Dictionary<String, TokenType> _dict = new KeyWords().Dict;

        private readonly String _inStream;
        private char _curChar;
        private int _curPos;
        private bool _endLine;

        public Lexer(String inStream) // Pass input stream/file path
        {
            Tokens = new List<Token>();
            _inStream = inStream;
            _curChar = _inStream[_curPos];
            
            _curPos = 0;
            _endLine = false;
            
            _lex();
        }

        private void _lex()
        {
            while (!_endLine)
            {
                switch (_curChar)
                {
                    case ' ':
                        _curPos++;
                        break;
                    case '?':
                    case '!':
                        Tokens.Add(new Token(TokenType.EoL, ""));
                        _endLine = true;
                        break;
                    case '\'':
                    case '\"':
                        _getString();
                        break;
                    case '*':
                        Tokens.Add(new Token(TokenType.Multiply, "*"));
                        _curPos++;
                        break;
                    case '/':
                        Tokens.Add(new Token(TokenType.Divide, "/"));
                        _curPos++;
                        break;
                    case '+':
                        Tokens.Add(new Token(TokenType.Addition, "+"));
                        _curPos++;
                        break;
                    case '-':
                        Token lastToken = Tokens.LastOrDefault();
                        if (lastToken != null && (lastToken.Type == TokenType.Integer || lastToken.Type == TokenType.Id))
                        {
                            Tokens.Add(new Token(TokenType.Subtraction, "-"));
                        }
                        else
                        {
                            Tokens.Add(new Token(TokenType.Negate, "-"));
                        }
                        _curPos++;
                        break;
                    case '(':
                        Tokens.Add(new Token(TokenType.OpenBracket, "("));
                        _curPos++;
                        break;
                    case ')':
                        Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                        _curPos++;
                        break;
                    // TODO EOF case which also ends line
                    default:
                        if (_isDigit())
                        {
                            _getDigits();
                        }
                        else if (_isLetter())
                        {
                            _getLetters();
                        }
                        else
                        {
                            Tokens.Add(new Token(TokenType.Illegal, ""));
                            throw new Exception("Illegal token found.");
                        }
                        break;
                }

                try
                {
                    _curChar = _inStream[_curPos];
                }
                catch (Exception)
                {
                    _endLine = true;
                }
                
            }
        }

        private void _getDigits()
        {
            String outString = "";
            
            outString += _curChar.ToString();
            _curChar = _inStream[++_curPos];

            while (_isDigit())
            {
                outString += _curChar.ToString();
                _curChar = _inStream[++_curPos];
            }

            Tokens.Add(new Token(TokenType.Integer, int.Parse(outString)));
        }

        /**
         * Specific for keywords/variables
         */
        private void _getLetters()
        {
            String outString = "";
            bool isKeyword = false;
            
            outString += _curChar.ToString();
            _curChar = _inStream[++_curPos];

            while (_isLetter())
            {
                outString += _curChar.ToString();
                
                if (_dict.TryGetValue(outString, out TokenType result))
                {
                    switch (result)
                    {
                        case TokenType.Plates:
                        {
                            //Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                            Tokens.Add(new Token(TokenType.Multiply, "*"));
                            Tokens.Add(new Token(TokenType.Integer, 40));
                            //Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                            Tokens.Add(new Token(TokenType.Addition, "+"));
                            Tokens.Add(new Token(TokenType.Integer, 20));
                            break;
                        }
                        case TokenType.Bench:
                        {
                            Tokens.Add(new Token(TokenType.Bench, "int "));
                            break;
                        }
                        case TokenType.Can:
                        {
                            Tokens.Add(new Token(TokenType.Can, " = "));
                            break;
                        }
                        default:
                        {
                            Tokens.Add(new Token(result, outString));
                            break;
                        }
                    }
                    outString = "";
                    isKeyword = true;
                    _curChar = _inStream[++_curPos];
                    break;
                }
                                
                _curChar = _inStream[++_curPos];
            }
            if (!isKeyword) 
            {
                /* Store identifier name */
                Tokens.Add(new Token(TokenType.Id, outString));
            }
        }

        /**
         * Specific for strings
         */
        private void _getString()
        {
            String outString = "";

            outString += _curChar.ToString();
            _curChar = _inStream[++_curPos];

            while (_isLetter())
            {
                outString += _curChar.ToString();
                _curChar = _inStream[++_curPos]; 
            }

            if (outString.Substring(0, 1).Equals(_curChar.ToString()))
            {
                Tokens.Add(new Token(TokenType.String, outString + _curChar));
                _curPos++;
            }
            else
            {
                throw new Exception("String not closed properly.");
            }
        }
        
        private bool _isDigit()
        {
            return '0' <= _curChar && _curChar <= '9';
        }

        private bool _isLetter()
        {
            return 'a' <= _curChar && _curChar <= 'z' || 'A' <= _curChar && _curChar <= 'Z' || _curChar == '_';
        }
        
        
    }
}