/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */

 
using System;
using System.Collections.Generic;

namespace Tokenizer
{
    // Enum of all token types for the language
    public enum TokenType
    {
        Illegal,    // Illegal character
        EoF,        // End of file
        
        Int,        // Integer
        Str,        // String
        Var,        // Variable
        
        Bench,      // int

        Question,
        Can,
        Plates,     
        
        Root,
        Term,
        Expression,
        Factor,
        Integer,
        String,
        Addition,
        EoL,
        Subtraction,
        OpenBracket,
        CloseBracket,
        Multiply,
        Divide,
        Negate,
        Id
    }

    public class KeyWords
    {
        public Dictionary<String, TokenType> Dict = new Dictionary<String, TokenType>()
        {    
            {"can", TokenType.Can},
            {"bench", TokenType.Bench},
            {"plates", TokenType.Plates}
        };
    }

    public class Token
    {
        public TokenType Type;
        public String StringContent;
        public int IntegerContent;

        public Token(TokenType type, string contents)
        {
            Type = type;
            StringContent = contents;
        }

        public Token(TokenType type, int contents)
        {
            Type = type;
            IntegerContent = contents;
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        static void Main(string[] args)
        {
            
        }
    }
}