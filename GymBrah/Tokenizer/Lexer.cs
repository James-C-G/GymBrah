/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs
 * Last Modified :  08/12/21
 * Version :        1.4
 * Description :    Class for the lexical analysis of a stream of characters, using the Token class as a wrapper class
 *                  to create a stream of output tokens for the parsing.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tokenizer
{
    /// <summary>
    /// Lexer class that reads through an input stream of characters and converts that into a stream of tokens.
    /// </summary>
    public class Lexer
    {
        public List<Token> Tokens { get; }

        private readonly Dictionary<String, TokenType> _dict = new KeyWords().Dict;
        
        private readonly StreamReader _inStream;
        private int _currentChar;

        /// <summary>
        /// Constructor for a single string input, which is then converted into a stream.
        /// </summary>
        /// <param name="inStream"> String of content. </param>
        public Lexer(string inStream)
        {
            Tokens = new List<Token>();
            _inStream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(inStream)));
            _currentChar = _inStream.Read();
            _lex();
        }
        
        /// <summary>
        /// Constructor for an input stream of characters.
        /// </summary>
        /// <param name="inStream"> Input stream. </param>
        public Lexer(StreamReader inStream)
        {
            Tokens = new List<Token>();
            _inStream = inStream;
            _currentChar = _inStream.Read();
            _lex();
        }

        /// <summary>
        /// Lexer method that converts the input stream into a list of tokens.
        /// </summary>
        /// <exception cref="Exception"> Errors in lexing. </exception>
        private void _lex()
        {
           while (_currentChar != -1) // While there is still content in the stream
           {
               switch ((char)_currentChar)
                {
                    case '\n': // Characters to ignore.
                    case '\r':
                    case ' ':
                    {
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '?': // EoL character.
                    {
                        Tokens.Add(new Token(TokenType.EoL, ";"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '\'': // Quote characters
                    case '\"':
                    {
                        _getString();
                        break;
                    }
                    case '!': // Boolean character
                    {
                        Tokens.Add(new Token(TokenType.Not, "!"));
                        _currentChar = _inStream.Read();
                        break; 
                    }
                    case '=': // Boolean character
                    {
                        Tokens.Add(new Token(TokenType.Equals, "="));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '<': // Boolean character
                    {
                        Tokens.Add(new Token(TokenType.LessThan, "<"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '>': // Boolean character
                    {
                        Tokens.Add(new Token(TokenType.GreaterThan, ">"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '*': // Boolean character
                    {
                        Tokens.Add(new Token(TokenType.Multiply, "*"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '/': // Maths character
                    {
                        Tokens.Add(new Token(TokenType.Divide, "/"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '+': // Maths character
                    {
                        Tokens.Add(new Token(TokenType.Addition, "+"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '-': // Maths character
                    {
                        Token lastToken = Tokens.LastOrDefault();
                        if (lastToken != null &&
                            (lastToken.Type == TokenType.Integer || lastToken.Type == TokenType.Id))
                        {
                            Tokens.Add(new Token(TokenType.Subtraction, "-")); // Subtraction
                        }
                        else
                        {
                            Tokens.Add(new Token(TokenType.Negate, "-")); // Negation
                        }
                        
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '(': // Maths character
                    {
                        Tokens.Add(new Token(TokenType.OpenBracket, "("));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case ')': // Maths character
                    {
                        Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    case '#': // Comment character
                    {
                        _getComment();
                        break;
                    }
                    case ',': // Comma
                    {
                        Tokens.Add(new Token(TokenType.Comma, ","));
                        _currentChar = _inStream.Read();
                        break;
                    }
                    default:
                    {
                        // Check for string of digits, or letters (variables)
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

        /// <summary>
        /// Method to ignore comments.
        /// </summary>
        private void _getComment()
        {
            while ((char) _currentChar != '\n')
            {
                _currentChar = _inStream.Read();
            }
            _currentChar = _inStream.Read();
        }
        
        /// <summary>
        /// Method to get a string of digits.
        /// </summary>
        private void _getDigits()
        {
            String outString = "";
            bool isDouble = false;
            
            while (_isDigit())
            {
                outString += (char)_currentChar;
                _currentChar = _inStream.Read();
                
                if ((char) _currentChar == '.') // Double handler
                {
                    if (isDouble) throw new Exception("Double cannot have more than one decimal point.");
                    
                    isDouble = true;
                    outString += (char)_currentChar;
                    _currentChar = _inStream.Read();
                }
            }
            
            if (isDouble) Tokens.Add(new Token(TokenType.Double, outString));
            else Tokens.Add(new Token(TokenType.Integer, outString));
        }

        /// <summary>
        /// Method to get a string of letters.
        /// </summary>
        private void _getLetters()
        {
            String outString = "";

            while (_isLetter())
            {
                outString += (char) _currentChar;

                // If string of letters is a keyword
                if (_dict.TryGetValue(outString, out TokenType result))
                {
                    switch (result) // Handle all the different keywords
                    {
                        case TokenType.Plates:
                        {
                            Tokens.Add(new Token(TokenType.CloseBracket, ")"));
                            Tokens.Add(new Token(TokenType.Multiply, "*"));
                            Tokens.Add(new Token(TokenType.Integer, "40"));
                            Tokens.Add(new Token(TokenType.Addition, "+"));
                            Tokens.Add(new Token(TokenType.Integer, "20"));
                            
                            // Has to be last thing in the expression
                            if ((char) _inStream.Peek() == '?') break;

                            while ((char) _currentChar != '?')
                            {
                                _currentChar = _inStream.Read();
                                
                                if ((char) _currentChar != ' ')
                                {
                                    throw new Exception("Plates key word must be the last part of an expression.");
                                }
                            }
                            
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
                        case TokenType.DeadLift:
                        {
                            Tokens.Add(new Token(TokenType.DeadLift, "double "));
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
                        case TokenType.Is:
                        {
                            Tokens.Add(new Token(TokenType.Is, "if "));
                            break;
                        }
                        case TokenType.DropSet:
                        {
                            Tokens.Add(new Token(TokenType.DropSet, "while "));
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

            // Not a key word, so variable identifier.
            if (outString != "") 
            {
                Tokens.Add(new Token(TokenType.Id, outString));
            }
        }

        /// <summary>
        /// Get string content.
        /// </summary>
        /// <exception cref="Exception"> Errors in getting string content. </exception>
        private void _getString()
        {
            String outString = "";
            
            outString += (char)_currentChar;
            char start = (char)_currentChar; // Get \" or \'
            
            _currentChar = _inStream.Read();

            while (_isLetter() || _isDigit() || (char)_currentChar == '\\') //TODO Make method for escape characters
            {
                outString += (char)_currentChar;
                _currentChar = _inStream.Read();
                
                switch ((char) _currentChar) // Handle escape characters
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

            // Ensure the correct string quote is used at the start and end
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
        
        /// <summary>
        /// Boolean for whether the current character is an integer
        /// </summary>
        /// <returns> True/False </returns>
        private bool _isDigit()
        {
            return '0' <= (char)_currentChar && (char)_currentChar <= '9';
        }

        /// <summary>
        /// Boolean for whether the current character is a alphabetic character.
        /// </summary>
        /// <returns> True/False </returns>
        private bool _isLetter()
        {
            return 'a' <= (char)_currentChar && (char)_currentChar <= 'z' || 
                   'A' <= (char)_currentChar && (char)_currentChar <= 'Z' || 
                   (char)_currentChar == '_';
        }
        
        
    }
}