using System;
using System.Collections.Generic;

namespace Tokenizer
{
    public class Lexer
    {
        public List<Token> Tokens;

        private Dictionary<String, TokenType> _dict = new KeyWords().Dict;

        public Lexer(String inStream) // Pass input stream/file path
        {
            /*
             Go char by char, order of play:
             skip white spaces other than tabs, 
             single character tokens,
             strings (double/single quote),
             everything else  
             
             Whenever hit identifier check if its a key word           
             */
            Tokens = new List<Token>();

            String temp = "";
            TokenType result;

            foreach (char c in inStream)
            {
                if (c == '?' || c == '!')
                {
                    Tokens.Add(new Token(TokenType.Question, temp));
                    temp = "";
                }
                else if (Char.IsWhiteSpace(c))
                {
                    //TODO Put in optimised order i.e. size order
                    switch (temp)
                    {
                        case "EOF":
                            Tokens.Add(new Token(TokenType.EoF, ""));
                            break;
                        case "can":
                            Tokens.Add(new Token(TokenType.Can, temp));
                            break;
                        case "you":
                            Tokens.Add(new Token(TokenType.You, temp));
                            break;
                        case "bench":
                            Tokens.Add(new Token(TokenType.Bench, temp));
                            break;
                        case "plates":
                            Tokens.Add(new Token(TokenType.Plates, temp));
                            break;
                        default:
                            if (IsDigit(temp))
                            {
                                Tokens.Add(new Token(TokenType.Int, temp));
                            }
                            else if (IsString(temp))
                            {
                                Tokens.Add(new Token(TokenType.Str, temp));
                            }
                            else if (_dict.TryGetValue(temp, out result))
                            {
                                Tokens.Add(new Token(result, temp));
                            }
                            else if (IsVar(temp))
                            {
                                Tokens.Add(new Token(TokenType.Var, temp));
                            }
                            else
                            {
                                Tokens.Add(new Token(TokenType.Illegal, temp));
                            }
                            break;
                    }
                    temp = "";
                }
                else
                {
                    temp += c;
                }
            }
        }

        private static bool IsDigit(String input)
        {
            foreach (char c in input)
            {
                if ('0' <= c && c <= '9')
                {
                    return true;
                }
            }
            return false;
        }
        
        private static bool IsString(String input)
        {
            if (input[0] == '\"' && input[input.Length] == '\"')
            {
                return true;
            }
            return false;
        }
        
        private static bool IsVar(String input)
        {
            foreach (char c in input)
            {
                if ('a' <= c && c <= 'z' || 'A' <= c && c <= 'Z' || c == '_')
                {
                    return true;
                }
            }
            return false;
        }
    }
}