/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Node.cs, Parse.cs, Value.cs
 * Last Modified :  06/12/21
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
        protected Dictionary<String, FunctionTable> Functions;
        
<<<<<<< Updated upstream
        /// <summary>
        /// Constructor to initialise variables, and set up current token..
        /// </summary>
        /// <param name="tokens"> List of tokens to parse. </param>
        /// <param name="variableTable"> Variable table. </param>
        protected Parse(List<Token> tokens, ref Dictionary<String, Value> variableTable)
=======
        protected Parse(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, FunctionTable> functions)
>>>>>>> Stashed changes
        {
            _tokens = tokens;
            CurrentToken = tokens[_counter];
            VariableTable = variableTable;
            Functions = functions;
        }

        /// <summary>
        /// General parse tree method.
        /// </summary>
        /// <returns> An abstract node object. </returns>
        public abstract Node ParseTree();

        protected List<Token> GetTokens() { return _tokens; } //TODO Maybe remove?
        
        /// <summary>
        /// Helper method used to get the remaining tokens, starting from the current token.
        /// </summary>
        /// <returns> List of remaining tokens. </returns>
        protected List<Token> GetRemainingTokens()
        {
            List<Token> outPut = _tokens.GetRange(_counter, _tokens.Count - _counter);
            return outPut;
        }

        protected Token PeekToken() //TODO Maybe remove?
        {
            try
            {
                return _tokens[_counter + 1];
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Move to the next token while returning the current token.
        /// </summary>
        /// <returns> Current token. </returns>
        protected Token ScanToken()
        {
            Token token = _tokens[_counter];  
            _counter++;
            
            try
            {
                CurrentToken = _tokens[_counter]; 
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}