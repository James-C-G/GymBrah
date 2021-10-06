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
            //{TokenType.If, "if"}
            //{TokenType.For, "for"},
            //{TokenType.While, "while"}
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
            return this.type.ToString();
        }
    }

    public class Tokenizer
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            Lexer x = new Lexer("can you bench 2 plates x?");
            x.Tokens.ForEach(Console.WriteLine);
        } 
    }
}