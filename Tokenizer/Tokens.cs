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
        EoL,

        Bench,      // int
        Squat,      // string
        Can,        // =
        Plates,     // ( var * 40) + 20 
        Scream,     // Printf("")
        Is,         // If
        DropSet,    // While
        
        OpenBracket,
        CloseBracket,
        LightWeight,    // {
        Baby,           // }
        Id,

        Integer,
        String,
        Equals,
        LessThan,       // <
        GreaterThan,    // >
        Not,            // !
        
        Addition,
        Subtraction,
        Multiply,
        Divide,
        Negate
    }

    public class KeyWords
    {
        public Dictionary<String, TokenType> Dict = new Dictionary<String, TokenType>()
        {    
            {"can", TokenType.Can},
            {"bench", TokenType.Bench},
            {"squat", TokenType.Squat},
            {"plates", TokenType.Plates},
            {"scream", TokenType.Scream},
            {"lightweight", TokenType.LightWeight},
            {"baby", TokenType.Baby},
            {"is", TokenType.Is},
            {"dropset", TokenType.DropSet}
        };
    }

    public class Token
    {
        public TokenType Type;
        public string Content;

        public Token(TokenType type, string contents)
        {
            Type = type;
            // StringContent = contents;
            Content = contents;
        }

        // public Token(TokenType type, int contents)
        // {
        //     Type = type;
        //     IntegerContent = contents;
        // }

        public override string ToString()
        {
            return Type.ToString();
        }

        static void Main(string[] args)
        {
        }
    }
}