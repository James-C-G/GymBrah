/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs, Parser.cs, Syntax Tree.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */


using System;
using System.Collections.Generic;

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
                    case '!':   // Fall-through
                        Tokens.Add(new Token(TokenType.Question, "?"));
                        _endLine = true;
                        break;
                    case '\"':
                        //TODO Read string
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
                        }
                        break;
                }
                _curChar = _inStream[_curPos];
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

            Tokens.Add(new Token(TokenType.Int, outString));
        }

        private void _getLetters()
        {
            String outString = "";
            TokenType result;
            bool isKeyword = false;
            
            outString += _curChar.ToString();
            _curChar = _inStream[++_curPos];

            while (_isLetter())
            {
                outString += _curChar.ToString();

                if (_dict.TryGetValue(outString, out result))
                {
                    Tokens.Add(new Token(result, outString));
                    outString = "";
                    isKeyword = true;
                    _curChar = _inStream[++_curPos];
                    break;
                }
                                
                _curChar = _inStream[++_curPos];
            }
            if (!isKeyword)
            {
                Tokens.Add(new Token(TokenType.Var, outString));
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