using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace Assignment
{
    public class RightAssignment : Parse<string>
    {
        private readonly string _currentVariable;
        public RightAssignment(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref string currentVariable) 
            : base(tokens, ref variableTable)
        {
            _currentVariable = currentVariable;
        }

        public override AssignmentNode ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Integer: // int x = 3;
                {
                    List<Token> calcTokens = GetRemainingTokens();
                    calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    int calculation = new Calculator.Calculator(calcTokens, ref VariableTable).ParseTree().Evaluate();

                    if (VariableTable.TryGetValue(_currentVariable, out Value result))
                    {
                        VariableTable[_currentVariable] = new IntegerValue(calculation);
                    }
                    else
                    {
                        VariableTable.Add(_currentVariable, new IntegerValue(calculation));
                    }
                    
                    return new TerminalNode(new Token(TokenType.Integer, calculation.ToString()));
                }
                case TokenType.Id: // int x = y;
                {
                    Token test = PeekToken();
                    
                    if (VariableTable.TryGetValue(CurrentToken.StringContent, out Value result))
                    {
                        // Check variable exists and is of the right type
                        if (result.Type == ValueType.Integer)
                        {
                            VariableTable[_currentVariable] =  new IntegerValue(((IntegerValue)result).Content);
                            // return new TerminalNode(new Token(TokenType.Integer, ((IntegerValue)result).Content));
                        }
                        else
                        {
                            // TODO Lookahead for when its a string
                            throw new Exception("Variable not an integer.");
                        }
                    }
                    else
                    {
                        throw new Exception("Variable not defined.");
                    }
                    
                    List<Token> calcTokens = GetRemainingTokens();
                    
                    // TODO If just int x = y; brackets not needed
                    if (test.Type != TokenType.EoL)
                    {
                        calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                        calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    }
                    
                    string calculation = new Calculator.Calculator(calcTokens, ref VariableTable).ParseTree().Evaluate().ToString();
                
                    return new TerminalNode(new Token(TokenType.Integer, calculation));
                    
                }
                default:
                {
                    return null;
                }
            }
        }
    }
}