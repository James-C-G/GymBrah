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
        Plates     // Integer
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
        public TokenType type;
        public string contents; // int x = 10

        public Token(TokenType type, string contents)
        {
            this.type = type;
            this.contents = contents;
        }

        public override string ToString()
        {
            return type.ToString();
        }
    }

    public class Tokenizer
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");

            Lexer x = new Lexer("can you bench 2 plates x ?");
            x.Tokens.ForEach(Console.WriteLine);

            // Parser x = new Parser("canyou bench 2 plates x ?");
            // x._tokens.Add(new Token(TokenType.EoF, ""));
            // Console.Out.WriteLine(x.Parse());
        } 
    }
}