using System;
using System.Collections.Generic;
using Tokenizer;

namespace Compiler
{
    public class Parse
    {
        private int _counter;
        protected Token CurrentToken;
        private readonly List<Token> _tokens;
        
        protected Parse(List<Token> tokens)
        {
            _tokens = tokens;
            CurrentToken = tokens[_counter];
        }
        
        protected List<Token> GetRemainingTokens()
        {
            List<Token> outPut = _tokens.GetRange(_counter, _tokens.Count - _counter);
            return outPut;
        }

        protected Token PeekToken()
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