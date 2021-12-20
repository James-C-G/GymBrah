/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs 
 * Last Modified :  19/12/21
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
    /// <summary>
    /// Function parse tree class for the parsing of function definitions and calls, ensure the correct types are
    /// maintained in the function parameters.
    /// </summary>
    public class Functions : Parse
    {
        private readonly List<Parameter> _parameters; // List of function parameters
        private readonly bool _evaluate;    // Boolean of whether to evaluate the mathematical expressions

        /// <summary>
        /// Inherited constructor with with evaluate boolean for whether or not to compile variables, and the functions
        /// list of parameters.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        /// <param name="evaluate"> Evaluate boolean. </param>
        public Functions(List<Token> tokens, ref Dictionary<String, Value> variableTable,
            ref Dictionary<String, Function> functionTable, bool evaluate = true) :
            base(tokens, ref variableTable, ref functionTable)
        {
            _parameters = new List<Parameter>();
            _evaluate = evaluate;
        }

        /// <summary>
        /// Recursive parse method for getting the parameters passed in at function call, ensuring they're the right
        /// type matching the function definition.
        /// </summary>
        /// <param name="parameters"> List of function parameters. </param>
        /// <param name="currentParam"> Current parameter being checked. </param>
        /// <returns> Node for list of parameters in function call. </returns>
        /// <exception cref="Exception"> Parse errors. </exception>
        private Node _parseFunctionCallParameters(List<Parameter> parameters, int currentParam)
        {
            // Too many parameters passed
            if (currentParam >= parameters.Count)
            {
                throw new Exception("Too many parameters passed in function call.");
            }
            
            Parameter currentParameter = parameters[currentParam]; // Get current parameter to check
            
            Token paramToken = null;
                
            // Get data type
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    // Check identifier exists
                    if (VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                    {
                        // Store identifier value, and evaluate if necessary.
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
                    // Check if identifier being used
                    if (paramToken == null) paramToken = CurrentToken;
                    
                    // Ensure correct type
                    if (currentParameter.VariableType.Type == TokenType.Bench)
                    {
                        ScanToken();
                        if (CurrentToken.Type == TokenType.Comma) // Get comma separation
                        {
                            ScanToken();
                            
                            // Get remaining parameters
                            return new FunctionCallParameterNode(paramToken, _parseFunctionCallParameters(parameters, ++currentParam));
                        }
                        if (CurrentToken.Type == TokenType.CloseBracket) // End of list of parameters
                        {
                            ScanToken();
                            
                            // Ensure EoL at end
                            if (CurrentToken.Type == TokenType.EoL)
                            {
                                return new TerminalNode(paramToken); // Final parameter
                            }
                        }
                        
                        throw new Exception("Parameter call not ended correctly.");
                    }
                    
                    throw new Exception("Incorrect parameter value passed - integer gotten, expected: " + currentParameter.VariableType.Type);
                }
                case TokenType.Double:
                {
                    // Check if identifier being used
                    if (paramToken == null) paramToken = CurrentToken;
                    
                    // Ensure correct type
                    if (currentParameter.VariableType.Type == TokenType.DeadLift)
                    {
                        ScanToken();
                        if (CurrentToken.Type == TokenType.Comma) // Get comma separation
                        {
                            ScanToken();
                            
                            // Get remaining parameters
                            return new FunctionCallParameterNode(paramToken, _parseFunctionCallParameters(parameters, ++currentParam));
                        }
                        if (CurrentToken.Type == TokenType.CloseBracket)
                        {
                            ScanToken();
                            
                            // Ensure EoL at end
                            if (CurrentToken.Type == TokenType.EoL)
                            {
                                return new TerminalNode(paramToken); // Final parameter
                            }
                        }
                        
                        throw new Exception("Parameter call not ended correctly.");
                    }

                    throw new Exception("Incorrect parameter value passed - double gotten, expected: " + currentParameter.VariableType.Type);
                }
                case TokenType.String:
                {
                    // Check if identifier being used
                    if (paramToken == null) paramToken = CurrentToken;
                    
                    // Ensure the correct type
                    if (currentParameter.VariableType.Type == TokenType.Squat)
                    {
                        ScanToken();
                        if (CurrentToken.Type == TokenType.Comma) // Get comma separation
                        {
                            ScanToken();
                            
                            // Get remaining parameters
                            return new FunctionCallParameterNode(paramToken, _parseFunctionCallParameters(parameters, ++currentParam));
                        }
                        if (CurrentToken.Type == TokenType.CloseBracket)
                        {
                            ScanToken();
                            
                            // Ensure EoL at end
                            if (CurrentToken.Type == TokenType.EoL)
                            {
                                return new TerminalNode(paramToken); // Final parameter
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
        
        /// <summary>
        /// Parse method for parsing a function being called, getting the parameters if there are any.
        /// </summary>
        /// <param name="function"> Function being called. </param>
        /// <returns> Function call parameter node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private Node _parseExistingFunction(Function function)
        {
            // Function has parameters
            if (function.Parameters.Count != 0)
            {
                // Get parameters
               return _parseFunctionCallParameters(function.Parameters, 0);
            }

            // Function has no parameters
            Token current = ScanToken();
            if (current == null) throw new Exception("Parameters not called correctly.");
            
            if (current.Type == TokenType.CloseBracket)
            {
                if (CurrentToken.Type == TokenType.EoL) // Ensure EoL
                {
                    // Empty node for function call without parameters
                    return new TerminalNode(new Token(TokenType.Illegal, "")); 
                }

                throw new Exception("Function call not closed properly.");
            }

            throw new Exception("Incorrect parameters passed to function call.");
        }

        /// <summary>
        /// Recursive parse method for getting the parameters in a function definition.
        /// </summary>
        /// <returns> Function definition parameter node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private Node _parseParameters()
        {
            Token paramType = _parseFunctionType(); // Get parameter type
            if (paramType == null) throw new Exception("Unrecognised datatype in parameters.");
            
            ScanToken();
            
            // Get parameter identifier
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    // Ensure parameter identifier isn't already being used
                    foreach (var i in _parameters)
                    {
                        if (i.VariableName == CurrentToken.Content)
                        {
                            throw new Exception("Parameters cannot have the same name.");
                        }
                    }

                    // Add new parameter to parameter list
                    _parameters.Add(new Parameter(paramType, CurrentToken.Content));

                    Token current = ScanToken();

                    if (CurrentToken.Type == TokenType.CloseBracket) // End of definition
                    {
                        ScanToken();
                        
                        // Ensure open "{" character for function definition
                        if (CurrentToken.Type == TokenType.LightWeight && ScanToken() == null)
                        {
                            return new FinalParameterNode(paramType, current); // End of parameters
                        }

                        throw new Exception("Function definition braces not used properly.");
                    }

                    Token next = ScanToken();
                    if (next == null) goto default;
                    
                    // Ensure parameters are comma seperated
                    if (next.Type != TokenType.Comma)
                    {
                        throw new Exception("Invalid comma separation in parameters.");
                    }
                    
                    // Get next parameter, linking to current parameter node
                    return new ParameterNode(paramType, current, _parseParameters());
                }
                default:
                {
                    throw new Exception("Error in function parameter definition.");
                }
            }
        }
        
        /// <summary>
        /// Parse method for function definitions getting the parameters.
        /// </summary>
        /// <returns> Function parameters. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        private Node _parseFunctionParameters()
        {
            Token paramType = _parseFunctionType(); // Get first parameter type
            
            if (paramType == null) // No parameters
            {
                Token current = ScanToken();
                if (current == null) throw new Exception("Parameters not defined correctly.");
                
                // Look for end of params and start of braces
                if (current.Type == TokenType.CloseBracket)
                {
                    // Ensure braces are last token in definition
                    if (CurrentToken.Type == TokenType.LightWeight && ScanToken() == null)
                    {
                        return new TerminalNode(new Token(TokenType.String, "")); // No parameters 
                    }

                    throw new Exception("Function definition braces not used properly.");
                }

                throw new Exception("Function parameters not defined correctly.");
            }
            
            // Get parameters
            return _parseParameters();
        }
        
        /// <summary>
        /// Parse method for getting the current token's type, specifically for the function's return type.
        /// </summary>
        /// <returns> Data type token. </returns>
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
        
        /// <summary>
        /// Parse method for parsing function call and definitions, getting the list of parameters and ensuring their
        /// types.
        /// </summary>
        /// <returns> Function parse tree node. </returns>
        /// <exception cref="Exception"> Parse error. </exception>
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id: // Function call
                {
                    Token functionName = CurrentToken; // Get function name

                    // Cannot call main
                    if (functionName.Content == "main") throw new Exception("Main cannot be called.");
                    
                    if (FunctionTable.TryGetValue(ScanToken().Content, out Function result)) // Make sure function exists
                    {
                        Token current = ScanToken();
                        if (current == null) throw new Exception("Parameters not called correctly.");
                        
                        if (current.Type == TokenType.OpenBracket) // Ensure it is called properly
                        {
                            // Get function call
                            return new FunctionCallNode(functionName, _parseExistingFunction(result)); 
                        }

                        throw new Exception("Function called incorrectly.");
                    }

                    throw new Exception("Function is not defined.");
                }
                case TokenType.Workout: // Function definition
                {
                    ScanToken(); // Ignore keyword
                    
                    Token functionType = _parseFunctionType(); // Get function return type
                    
                    // Invalid function return type
                    if (functionType == null) throw new Exception("Invalid return type.");
                    
                    Node left = new TerminalNode(functionType); // Get function return type node
                    
                    ScanToken();
                    
                    if (CurrentToken.Type == TokenType.Id) // Function name
                    {
                        // Cannot redefine main method
                        if (CurrentToken.Content == "main") throw new Exception("Main cannot be redefined.");
                        
                        // Check not already defined as variable.
                        if (!VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                        {
                            Token name = ScanToken(); // Function name
                            
                            Token current = ScanToken();
                            if (current == null) throw new Exception("Parameters not defined correctly.");
                            
                            if (current.Type == TokenType.OpenBracket) // Start of function definition
                            {
                                // Get parameters
                                Node returnNode = new FunctionNode(left, _parseFunctionParameters(), name);

                                // Add function to function table
                                if (FunctionTable.TryAdd(name.Content, new Function(functionType.Type, _parameters)))
                                {
                                    return returnNode; // Return function parse tree
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
