/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tokenizer
{
    public class Lexer
    {
        public List<Token> Tokens { get; }

        private readonly Dictionary<String, TokenType> _dict = new KeyWords().Dict;
        
        private readonly StreamReader _inStream;
        private int _currentChar;

        public Lexer(string inStream)
        {
            Tokens = new List<Token>();
            _inStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inStream)));
            _currentChar = _inStream.Read();
            _lex();
        }
        
        public Lexer(StreamReader inStream)
        {
            Tokens = new List<Token>();
            _inStream = inStream;
            _currentChar = _inStream.Read();
            _lex();
        }

        private void _lex()
        {
           while (_currentChar != -1)
           {
               switch ((char)_currentChar)
                {
                    case '\n':
                    case '\r':
                    case ' ':
                    {
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '?':
                    //case '!':
                    {
                        Tokens.Add(new Token(TokenType.EoL, ";"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '\'':
                    case '\"':
                    {
                        _getString();
                        break;
                    }
                    case '!':
                    {
                        Tokens.Add(new Token(TokenType.Not, "!"));
                        _currentChar = _inStream.Read();
                        break; 
                    }
                    case '=':
                    {
                        Tokens.Add(new Token(TokenType.Equals, "="));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '<':
                    {
                        Tokens.Add(new Token(TokenType.LessThan, "<"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '>':
                    {
                        Tokens.Add(new Token(TokenType.GreaterThan, ">"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '*':
                    {
                        Tokens.Add(new Token(TokenType.Multiply, "*"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '/':
                    {
                        Tokens.Add(new Token(TokenType.Divide, "/"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '+':
                    {
                        Tokens.Add(new Token(TokenType.Addition, "+"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '-':
                    {
                        Token lastToken = Tokens.LastOrDefault();
                        if (lastToken != null &&
                            (lastToken.Type == TokenType.Integer || lastToken.Type == TokenType.Id))
                        {
                            Tokens.Add(new Token(TokenType.Subtraction, "-"));
                        }
                        else
                        {
                            Tokens.Add(new Token(TokenType.Negate, "-"));
                        }
                        
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '(':
                    {
                        Tokens.Add(new Token(TokenType.OpenBracket, "("));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case ')':
                    {
                        Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    default:
                    {
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
                            Tokens.Add(new Token(TokenType.Illegal, "" + (char)_currentChar));
                            throw new Exception("Illegal token found \"" + (char)_currentChar + "\"");
                        }

                        break;
                    }
                }
           } 
        }

        private void _getDigits()
        {
            String outString = "";
            
            while (_isDigit())
            {
                outString += (char)_currentChar;
                _currentChar = _inStream.Read();
            }

            Tokens.Add(new Token(TokenType.Integer, int.Parse(outString)));
        }

        /**
         * Specific for keywords/variables
         */
        private void _getLetters()
        {
            String outString = "";

            while (_isLetter())
            {
                outString += (char)_currentChar;
                
                if (_dict.TryGetValue(outString, out TokenType result))
                {
                    switch (result)
                    {
                        case TokenType.Plates: // TODO Add singular plate?
                        {
                            Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                            Tokens.Add(new Token(TokenType.Multiply, "*"));
                            Tokens.Add(new Token(TokenType.Integer, 40));
                            // Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                            Tokens.Add(new Token(TokenType.Addition, "+"));
                            Tokens.Add(new Token(TokenType.Integer, 20));
                            break;
                        }
                        case TokenType.Bench:
                        {
                            Tokens.Add(new Token(TokenType.Bench, "int "));
                            break;
                        }
                        case TokenType.Squat:
                        {
                            Tokens.Add(new Token(TokenType.Squat, "char* "));
                            break;
                        }
                        case TokenType.Can:
                        {
                            Tokens.Add(new Token(TokenType.Can, " = "));
                            break;
                        }
                        case TokenType.Scream:
                        {
                            Tokens.Add(new Token(TokenType.Scream, "printf("));
                            break;
                        }
                        default:
                        {
                            Tokens.Add(new Token(result, outString));
                            break;
                        }
                    }
                    outString = "";
                }
                _currentChar = _inStream.Read();
            }
            if (outString != "") 
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

            // Get \" or \'
            outString += (char)_currentChar;
            char start = (char)_currentChar;
            
            _currentChar = _inStream.Read();

            while (_isLetter() || _isDigit())
            {
                outString += (char)_currentChar;
                _currentChar = _inStream.Read();

                switch ((char) _currentChar)
                {
                    case '\\':
                    {
                        outString += (char) _currentChar;
                        _currentChar = _inStream.Read();
                        
                        switch ((char) _currentChar)
                        {
                            case '\'':
                            case '\"':
                            case '\\':
                            {
                                outString += (char) _currentChar;
                                _currentChar = _inStream.Read();
                                break;
                            }
                        }
                       
                        break;
                    }
                    case ' ':
                    {
                        outString += (char) _currentChar;
                        _currentChar = _inStream.Read();
                        break;
                    }
                }
            }

            // Ensure the correct string quote is used
            if ((char)_currentChar == start)
            {
                Tokens.Add(new Token(TokenType.String, outString + (char)_currentChar));
                _currentChar = _inStream.Read();
            }
            else
            {
                throw new Exception("String not closed properly.");
            }
        }
        
        private bool _isDigit()
        {
            return '0' <= (char)_currentChar && (char)_currentChar <= '9';
        }

        private bool _isLetter()
        {
            return 'a' <= (char)_currentChar && (char)_currentChar <= 'z' || 
                   'A' <= (char)_currentChar && (char)_currentChar <= 'Z' || 
                   (char)_currentChar == '_';
        }
        
        
    }
}