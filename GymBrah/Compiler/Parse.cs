/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Node.cs, Parse.cs, Value.cs
 * Last Modified :  12/12/21
 * Version :        1.4
 * Description :    Abstract recursive descent parse tree class that can be inherited to create a basic parse tree using
 *                  the general structure provided, relying on the list of tokens from the lexer and a dictionary for
 *                  the variable table.
 */

using System;
using System.Collections.Generic;
using Tokenizer;

namespace Compiler
{
    /// <summary>
    /// Abstract parse tree class.
    /// </summary>
    public abstract class Parse
    {
        private int _counter;
        protected Token CurrentToken;
        private readonly List<Token> _tokens;
        protected Dictionary<String, Value> VariableTable;
        protected Dictionary<String, Function> FunctionTable;
        
        /// <summary>
        /// Constructor to initialise variables, and set up current token..
        /// </summary>
        /// <param name="tokens"> List of tokens to parse. </param>
        /// <param name="variableTable"> Variable table. </param>
        /// /// <param name="functionTable"> Function table. </param>
        protected Parse(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable)
        {
            _tokens = tokens;
            CurrentToken = tokens[_counter];
            VariableTable = variableTable;
            FunctionTable = functionTable;
        }

        /// <summary>
        /// General parse tree method.
        /// </summary>
        /// <returns> An abstract node object. </returns>
        public abstract Node ParseTree();

        /// <summary>
        /// Helper method used to get the remaining tokens, starting from the current token.
        /// </summary>
        /// <returns> List of remaining tokens. </returns>
        protected List<Token> GetRemainingTokens()
        {
            try
            {
                List<Token> outPut = _tokens.GetRange(_counter, _tokens.Count - _counter);
                return outPut;
            }
            catch (Exception e)
            {
                throw new Exception("Token error.");
            }
        }

        /// <summary>
        /// Move to the next token while returning the current token.
        /// </summary>
        /// <returns> Current token. </returns>
        protected Token ScanToken()
        {
            try
            {
                Token token = _tokens[_counter];  
                _counter++;
                
                CurrentToken = _tokens[_counter]; 
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        static void Main()
        {
            Console.Out.WriteLine("");
        }
    }
}