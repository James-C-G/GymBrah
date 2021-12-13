/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Assignment.cs, Boolean.cs, Calculator.cs, Functions.cs GymBrah.cs, Program.cs, Repetition.cs,
 *                  Return.cs, Selection.cs, Statement.cs  
 * Last Modified :  13/12/21
 * Version :        1.4
 * Description :    Assignment parse tree that parses assignment expressions for string, integers, and doubles.
 */

using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    /// <summary>
    /// Assignment parse tree class to handle the assignment of arithmetic expressions, and strings to variable
    /// identifiers.
    /// </summary>
    public class Assignment : Parse
    {
        private TokenType _assignmentType;  // Type of assignment to ensure both sides of the assignment are the same
        private string _variableName;       // Name of variable being assigned
        private bool _evaluate;    // Boolean of whether to evaluate the mathematical expressions

        /// <summary>
        /// Inherited constructor and a boolean for either the string or integer evaluation.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="variableTable"></param>
        /// <param name="functionTable"></param>
        /// <param name="evaluate"> Boolean for type of evaluation. </param>
        public Assignment(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, Function> functionTable, bool evaluate = true) : 
            base(tokens, ref variableTable, ref functionTable)
        {
            _evaluate = evaluate;
        }

        /// <summary>
        /// Parse method for parsing the left hand side of an assignment. Dealing with the data type being assigned.
        /// </summary>
        /// <returns> Node of datatype being assigned. </returns>
        /// <exception cref="Exception"> Unrecognised assignment types. </exception>
        private Node _parseLeftB()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Bench: // Integer
                {
                    _assignmentType = TokenType.Integer;
                    return new TerminalNode(ScanToken());
                }
                case TokenType.DeadLift: // Double
                {
                    _assignmentType = TokenType.Double;
                    return new TerminalNode(ScanToken());
                }
                case TokenType.Squat: // String
                {
                    _assignmentType = TokenType.String;
                    return new TerminalNode(ScanToken());
                }
                case TokenType.Integer:
                case TokenType.Double:
                case TokenType.String:
                case TokenType.Id:  // Assignment without type e.g. x = 3;
                {
                    return null;
                }
                default:
                {
                    throw new Exception("Unrecognised assignment type " + CurrentToken.Content);
                }
            }
        }
        
        /// <summary>
        /// Parse method for left hand side of assignment, getting the variable being assigned and storing its type.
        /// </summary>
        /// <returns> Node for left side of assignment. </returns>
        /// <exception cref="Exception"> Assignment errors. </exception>
        private Node _parseLeft()
        {
            Token start =  ScanToken();
            Node nodeOne = _parseLeftB();
            
            switch (start.Type)
            {
                case TokenType.Id: // Get identifier
                {
                    _variableName = start.Content;

                    if (nodeOne == null) // Assignment without type e.g. x = 3;
                    {
                        // Check variable is defined
                        if (VariableTable.TryGetValue(_variableName, out Value result))
                        {
                            _assignmentType = result.Type; // Get datatype of variable
                        }
                        else
                        {
                            throw new Exception("Undefined variables cannot be assigned.");
                        }
                    }
                    else if (VariableTable.TryGetValue(_variableName, out Value result)) // If variable exists
                    {
                        // Assignment with datatype e.g. int x = 3;
                        if (result.Type.ToString() != _assignmentType.ToString()) // Ensure variable isn't already defined
                        {
                            throw new Exception("Variable being assigned is already defined and not of the correct type.");
                        }

                        throw new Exception("Variable cannot be redefined.");
                    }
                    else if (FunctionTable.TryGetValue(_variableName, out Function functionResult)) // If defined as function
                    {
                        throw new Exception("Variable cannot have the same name as a function.");
                    }
                    
                    if (nodeOne == null) return new TerminalNode(start);    // x = 3;
                    return new AssignmentVariableNode(start, nodeOne);      // int x = 3;
                }
                default:
                {
                    throw new Exception("Unrecognised token in assignment " + CurrentToken.Type);
                }
            }
        }

        /// <summary>
        /// Parse method to handle the right hand side of the assignment, handling what the variable is being assigned
        /// to store.
        /// </summary>
        /// <returns> Assignment node for right hand side. </returns>
        /// <exception cref="Exception"> Errors in assignment. </exception>
        private Node _parseRight()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id: // If being assigned to an identifier e.g. int x = y;
                {
                    if (VariableTable.TryGetValue(CurrentToken.Content, out Value result)) // Ensure variable exists
                    {
                        // Ensure variable is of the same type as the assignment type e.g. int x = "";
                        if (result.Type != _assignmentType) 
                        {
                            // Edge case for double assignment with integers e.g. double x = 1;
                            if (_assignmentType == TokenType.Double && result.Type == TokenType.Integer) goto case TokenType.Double;
                            
                            throw new Exception("Variables are not of the same type.");
                        }
                        if (result.Type == TokenType.Integer) // Handle integers
                        {
                            goto case TokenType.Integer;
                        }
                        if (result.Type == TokenType.Double) // Handle doubles
                        {
                            goto case TokenType.Double;
                        }
                        if (result.Type == TokenType.String) // Handle strings
                        {
                            goto case TokenType.String;
                        }

                        throw new Exception("Variable being assigned is not a valid type.");
                    }
                    if (FunctionTable.TryGetValue(CurrentToken.Content, out Function function)) // If function assignment
                    {
                        TokenType returnVal;
                        
                        // Check function type is being maintained in assignment
                        switch (_assignmentType)
                        {
                            case TokenType.Integer:
                            {
                                if (function.Type == TokenType.Bench)
                                {
                                    returnVal = TokenType.Integer;
                                    
                                    // Store variable
                                    if (!VariableTable.TryAdd(_variableName, new IntegerValue("function")))
                                    {
                                        VariableTable[_variableName] = new IntegerValue("function");
                                    }
                                    
                                    break;
                                }
                                goto default;
                            }
                            case TokenType.String:
                            {
                                if (function.Type == TokenType.Squat)
                                {
                                    returnVal = TokenType.String;
                                    
                                    // Store variable
                                    if (!VariableTable.TryAdd(_variableName, new StringValue("function")))
                                    {
                                        VariableTable[_variableName] = new StringValue("function");
                                    }
                                    
                                    break;
                                }
                                goto default;
                            }
                            case TokenType.Double:
                            {
                                if (function.Type == TokenType.DeadLift)
                                {
                                    returnVal = TokenType.Double;
                                    
                                    // Store variable
                                    if (!VariableTable.TryAdd(_variableName, new DoubleValue("function")))
                                    {
                                        VariableTable[_variableName] = new DoubleValue("function");
                                    }
                                    
                                    break;
                                }
                                goto default;
                            }
                            default:
                            {
                                throw new Exception("Function return type does not match variable type.");  
                            }
                        }

                        // Variable assignment 
                        return new TerminalNode(new Token(returnVal, new Functions(GetRemainingTokens(), ref VariableTable, ref FunctionTable).ParseTree().Evaluate()));
                    }
                    
                    throw new Exception("Variable is being assigned to something undefined.");
                }
                case TokenType.Double: // Calculate expression
                {
                    List<Token> calcTokens = GetRemainingTokens(); // Get tokens for calculation
                    
                    // Put brackets around entire expression
                    calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    calcTokens.Insert(calcTokens.Count - 1, new Token(TokenType.CloseBracket, ")"));

                    string calculation = new Calculator(calcTokens, ref VariableTable, ref FunctionTable, _evaluate).ParseTree().Evaluate();

                    if (!VariableTable.TryAdd(_variableName, new DoubleValue(calculation))) 
                        VariableTable[_variableName] = new DoubleValue(calculation);

                    // If expression is being string evaluated
                    if (!_evaluate) calculation = new Calculator(calcTokens, ref VariableTable, ref FunctionTable, false).ParseTree().Evaluate();
                    
                    return new VariableNode(new Token(CurrentToken.Type, calculation), 
                        new TerminalNode(new Token(TokenType.EoL, ";")));
                }
                case TokenType.Integer:
                {
                    List<Token> calcTokens = GetRemainingTokens(); // Get tokens for calculation
                    
                    // Put brackets around entire expression
                    calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    calcTokens.Insert(calcTokens.Count - 1, new Token(TokenType.CloseBracket, ")"));

                    string calculation = new Calculator(calcTokens, ref VariableTable, ref FunctionTable, _evaluate).ParseTree().Evaluate();

                    if (!VariableTable.TryAdd(_variableName, new IntegerValue(calculation))) 
                        VariableTable[_variableName] = new IntegerValue(calculation);

                    // If expression is being string evaluated
                    if (!_evaluate) calculation = new Calculator(calcTokens, ref VariableTable, ref FunctionTable, false).ParseTree().Evaluate();
                    
                    return new VariableNode(new Token(CurrentToken.Type, calculation), 
                        new TerminalNode(new Token(TokenType.EoL, ";")));
                }
                case TokenType.String:
                {
                    // Ensure assignment type 
                    if (CurrentToken.Type != _assignmentType && CurrentToken.Type != TokenType.Id)
                    {
                        throw new Exception("Incorrect string assignment type.");
                    }
                    
                    Token returnToken = CurrentToken;

                    if (CurrentToken.Type == TokenType.Id) // If identifier, get content
                    {
                        if (!VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                        {
                            throw new Exception("String identifier not defined.");
                        }

                        if (result.Content == "function") _evaluate = false;
                        
                        // Return string evaluation or variable content
                        if (!_evaluate) returnToken = CurrentToken;
                        else returnToken = new Token(TokenType.String, ((StringValue) result).Content);
                    }
                    
                    // Store variable content
                    if (!VariableTable.TryAdd(_variableName, new StringValue(returnToken.Content)))
                        VariableTable[_variableName] = new StringValue(returnToken.Content);

                    return new VariableNode(returnToken, new TerminalNode(new Token(TokenType.EoL, ";")));
                }
                default:
                {
                    throw new Exception("Unrecognised token in assignment " + CurrentToken.Type);
                }
            }
            
        }
        
        /// <summary>
        /// Parse tree method that returns an assignment tree for evaluation.
        /// </summary>
        /// <returns> Assignment parse tree node. </returns>
        /// <exception cref="Exception"> Assignment errors. </exception>
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Can: // All assignments must start with "Can"
                {
                    Token start = ScanToken();
                    return new EqualNode(_parseLeft(), _parseRight(), start);
                }
                default:
                {
                    throw new Exception("Unrecognised token in assignment " + CurrentToken.Type);
                }
            }
        }
    }
}