/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs, Parser.cs, Syntax Tree.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */

 
using System;
using System.Collections.Generic;

namespace Calculator
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

        Question,   // "?" == ";"
        Can,        // Equals 
        You,        // Equals
        Plates,     // Integer
        
        Root,
        Term,
        Expression,
        Factor,
        Integer,
        Addition,
        EoL,
        Subtraction,
        OpenBrace,
        CloseBrace,
        Multiply,
        Divide,
        Negate
    }

    public class KeyWords
    {
        public Dictionary<String, TokenType> Dict = new Dictionary<String, TokenType>()
        {    
            {"can", TokenType.Can},
            {"you", TokenType.You},
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
    }
}