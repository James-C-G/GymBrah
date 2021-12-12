/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Selection.cs, Statement.cs 
 * Last Modified :  12/12/21
 * Version :        1.4
 * Description :    Function class that builds parse trees for both function definitions and function calls. Parameters
 *                  are recursively parsed and types are maintained.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class Functions : Parse
    {
        private readonly List<Parameter> _parameters;
        private readonly bool _evaluate;    // Boolean of whether to evaluate the mathematical expressions

        public Functions(List<Token> tokens, ref Dictionary<String, Value> variableTable,
            ref Dictionary<String, Function> functionTable, bool evaluate = true) :
            base(tokens, ref variableTable, ref functionTable)
        {
            _parameters = new List<Parameter>();
            _evaluate = evaluate;
        }

        private Node _parseFunctionCallParameters(List<Parameter> parameters, int currentParam)
        {
            if (currentParam >= parameters.Count)
            {
                throw new Exception("Too many parameters passed in function call.");
            }
            
            Parameter currentParameter = parameters[currentParam];
            
            Token paramToken = null;
                
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    if (VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                    {
                        if (result.Type == TokenType.Integer)
                        {
                            if (_evaluate) paramToken = new Token(TokenType.Integer, result.Content);
                            else paramToken = CurrentToken;
                            
                            goto case TokenType.Integer;
                        }
                        if (result.Type == TokenType.String)
                        {
                            if (_evaluate) paramToken = new Token(TokenType.String, result.Content);
                            else paramToken = CurrentToken;
                            
                            goto case TokenType.String;
                        }
                        if (result.Type == TokenType.Double)
                        {
                            if (_evaluate) paramToken = new Token(TokenType.Double, result.Content);
                            else paramToken = CurrentToken;
                            
                            goto case TokenType.Double;
                        }

                        throw new Exception("Variable with unknown type.");
                    }

                    throw new Exception("Undefined variable in function call.");
                }
                case TokenType.Integer:
                {
                    if (paramToken == null) paramToken = CurrentToken;
                    
                    if (currentParameter.VariableType.Type == TokenType.Bench)
                    {
                        ScanToken();
                        if (CurrentToken.Type == TokenType.Comma)
                        {
                            ScanToken();
                            return new FunctionCallParameterNode(paramToken, _parseFunctionCallParameters(parameters, ++currentParam));
                        }
                        if (CurrentToken.Type == TokenType.CloseBracket)
                        {
                            ScanToken();
                            if (CurrentToken.Type == TokenType.EoL)
                            {
                                return new TerminalNode(paramToken);
                            }
                        }
                        
                        throw new Exception("Parameter call not ended correctly.");
                    }
                    
                    throw new Exception("Incorrect parameter value passed - integer gotten, expected: " + currentParameter.VariableType.Type);
                }
                case TokenType.Double:
                {
                    if (paramToken == null) paramToken = CurrentToken;
                    
                    if (currentParameter.VariableType.Type == TokenType.DeadLift)
                    {
                        ScanToken();
                        if (CurrentToken.Type == TokenType.Comma)
                        {
                            ScanToken();
                            return new FunctionCallParameterNode(paramToken, _parseFunctionCallParameters(parameters, ++currentParam));
                        }
                        if (CurrentToken.Type == TokenType.CloseBracket)
                        {
                            ScanToken();
                            if (CurrentToken.Type == TokenType.EoL)
                            {
                                return new TerminalNode(paramToken);
                            }
                        }
                        
                        throw new Exception("Parameter call not ended correctly.");
                    }

                    throw new Exception("Incorrect parameter value passed - double gotten, expected: " + currentParameter.VariableType.Type);
                }
                case TokenType.String:
                {
                    if (paramToken == null) paramToken = CurrentToken;
                    
                    // Ensure the correct type
                    if (currentParameter.VariableType.Type == TokenType.Squat)
                    {
                        ScanToken();
                        if (CurrentToken.Type == TokenType.Comma)
                        {
                            ScanToken();
                            return new FunctionCallParameterNode(paramToken, _parseFunctionCallParameters(parameters, ++currentParam));
                        }
                        if (CurrentToken.Type == TokenType.CloseBracket)
                        {
                            ScanToken();
                            if (CurrentToken.Type == TokenType.EoL)
                            {
                                return new TerminalNode(paramToken);
                            }
                        }
                        
                        throw new Exception("Parameter call not ended correctly.");
                    }
                    
                    throw new Exception("Incorrect parameter value passed - string gotten, expected: " + currentParameter.VariableType.Type);
                }
                default:
                {
                    throw new Exception("Error, unrecognised type in function call.");
                }
            }
        }
        
        private Node _parseExistingFunction(Function function)
        {
            // Function has parameters
            if (function.Parameters.Count != 0)
            {
               return _parseFunctionCallParameters(function.Parameters, 0);
            }

            // Function has no parameters
            if (ScanToken().Type == TokenType.CloseBracket)
            {
                if (CurrentToken.Type == TokenType.EoL)
                {
                    return new TerminalNode(new Token(TokenType.Illegal, ""));
                }

                throw new Exception("Function call not closed properly.");
            }

            throw new Exception("Incorrect parameters passed to function call.");
        }

        private Node _parseParameters()
        {
            Token paramType = _parseFunctionType();
            if (paramType == null) throw new Exception("Unrecognised datatype in parameters.");
            
            ScanToken();
            
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    foreach (var i in _parameters)
                    {
                        if (i.VariableName == CurrentToken.Content)
                        {
                            throw new Exception("Parameters cannot have the same name.");
                        }
                    }

                    _parameters.Add(new Parameter(paramType, CurrentToken.Content));

                    Token current = ScanToken();

                    if (CurrentToken.Type == TokenType.CloseBracket)
                    {
                        ScanToken();
                        
                        if (CurrentToken.Type == TokenType.LightWeight && ScanToken() == null)
                        {
                            return new FinalParameterNode(paramType, current); // End 
                        }

                        throw new Exception("Function definition braces not used properly.");
                    }

                    if (ScanToken().Type != TokenType.Comma)
                    {
                        throw new Exception("Invalid comma separation in parameters.");
                    }
                    
                    return new ParameterNode(paramType, current, _parseParameters());
                }
                default:
                {
                    throw new Exception("Error in function parameter definition.");
                }
            }
        }
        
        private Node _parseFunctionParameters()
        {
            Token paramType = _parseFunctionType();
            
            if (paramType == null)
            {
                // Look for end of params and {
                if (ScanToken().Type == TokenType.CloseBracket)
                {
                    if (CurrentToken.Type == TokenType.LightWeight && ScanToken() == null)
                    {
                        return new TerminalNode(new Token(TokenType.String, "")); // No parameters 
                    }

                    throw new Exception("Function definition braces not used properly.");
                }

                throw new Exception("Function parameters not defined correctly.");
            }
            
            return _parseParameters();
        }
        
        private Token _parseFunctionType()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Squat:
                case TokenType.DeadLift:
                case TokenType.Bench:
                {
                    return CurrentToken;
                }
                default:
                {
                    return null;
                }
            }
        }
        
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id: // Function call
                {
                    Token functionName = CurrentToken;
                    
                    if (FunctionTable.TryGetValue(ScanToken().Content, out Function result)) // Make sure function exists
                    {
                        if (ScanToken().Type == TokenType.OpenBracket) // Ensure it is called properly
                        {
                            return new FunctionCallNode(functionName, _parseExistingFunction(result)); 
                        }

                        throw new Exception("Function called incorrectly.");
                    }

                    throw new Exception("Function is not defined.");
                }
                case TokenType.BroSplit: // Function definition
                {
                    ScanToken(); // Ignore keyword
                    Token functionType = _parseFunctionType();
                    if (functionType == null) throw new Exception("Invalid return type.");
                    
                    Node left = new TerminalNode(functionType); // Get function return type
                    
                    ScanToken();
                    
                    if (CurrentToken.Type == TokenType.Id)
                    {
                        // Check not already defined as variable.
                        if (!VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                        {
                            Token name = ScanToken();
                            
                            if (ScanToken().Type == TokenType.OpenBracket) // Start of function definition
                            {
                                Node returnNode = new FunctionNode(left,_parseFunctionParameters(), name);

                                if (FunctionTable.TryAdd(name.Content, new Function(functionType.Type, _parameters)))
                                {
                                    return returnNode;
                                }

                                throw new Exception("Function of that name already exists.");
                            }

                            throw new Exception("Error in function definition.");
                        }

                        throw new Exception("Cannot name a function after a variable.");
                    }
                    
                    throw new Exception("Function name declaration error.");
                }
                default:
                {
                    throw new Exception("Unrecognised token.");
                }
            }
        }
    }

}
