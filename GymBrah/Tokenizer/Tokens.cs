/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs
 * Last Modified :  06/12/21
 * Version :        1.4
 * Description :    Class for the storage of tokens in the lexical analysis phase. Wrapper classes for the keywords and
 *                  tokens are defined here.
 */

 
using System;
using System.Collections.Generic;

namespace Tokenizer
{
    /// <summary>
    /// Enum of all token types for the language. 
    /// </summary>
    public enum TokenType
    {
        Illegal,    // Illegal character
        EoL,        // End of line

        Bench,      // int
        Squat,      // string
        DeadLift,   // Bool
        Can,        // =
        
        Plates,     // ( var * 40) + 20 
        
        Scream,     // Printf("")
        Is,         // If
        DropSet,    // While
        
        OpenBracket,    // (    
        CloseBracket,   // )
        LightWeight,    // {
        Baby,           // }
        Id,

        Integer,
        String,         
        Equals,         // =
        LessThan,       // <
        GreaterThan,    // >
        Not,            // !
        
        Addition,       // +
        Subtraction,    // -    
        Multiply,       // *
        Divide,         // /
        Negate          // -()
    }

    /// <summary>
    /// Keyword class to initialise the dictionary of keywords.
    /// </summary>
    public class KeyWords
    {
        public Dictionary<String, TokenType> Dict = new Dictionary<String, TokenType>()
        {    
            {"can", TokenType.Can},
            {"bench", TokenType.Bench},
            {"squat", TokenType.Squat},
            {"deadlift", TokenType.DeadLift},
            {"plates", TokenType.Plates},
            {"scream", TokenType.Scream},
            {"lightweight", TokenType.LightWeight},
            {"baby", TokenType.Baby},
            {"is", TokenType.Is},
            {"dropset", TokenType.DropSet}
        };
    }

    /// <summary>
    /// Token class to initialise a token with a type and content.
    /// </summary>
    public class Token
    {
        public TokenType Type;
        public string Content;

        /// <summary>
        /// Constructor to initialise variables.
        /// </summary>
        /// <param name="type"> Token type. </param>
        /// <param name="contents"> Token content. </param>
        public Token(TokenType type, string contents)
        {
            Type = type;
            Content = contents;
        }

        /// <summary>
        /// To string method.
        /// </summary>
        /// <returns> Token type. </returns>
        public override string ToString()
        {
            return Type.ToString();
        }

        static void Main(string[] args)
        {
        }
    }
}