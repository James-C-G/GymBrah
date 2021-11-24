using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace GymBrah
{
    public class Assignment : Parse
    {
        private TokenType _assignmentType;
        private string _variableName;
        public Assignment(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        private Node _parseLeftB()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Bench:
                {
                    _assignmentType = TokenType.Integer;
                    return new TerminalNode(ScanToken());
                }
                case TokenType.Squat:
                {
                    _assignmentType = TokenType.String;
                    return new TerminalNode(ScanToken());
                }
                case TokenType.Integer:
                case TokenType.String:
                case TokenType.Id:  // can x y?
                {
                    return null;
                }
                default:
                {
                    throw new Exception("Unrecognised assignment type " + CurrentToken.Content);
                }
            }
        }
        
        private Node _parseLeft()
        {
            Token start =  ScanToken();
            Node nodeOne = _parseLeftB();
            
            switch (start.Type)
            {
                case TokenType.Id:
                {
                    _variableName = start.Content;

                    if (nodeOne == null) // can x y?
                    {
                        if (VariableTable.TryGetValue(_variableName, out Value result))
                        {
                            _assignmentType = (TokenType) result.Type;
                        }
                        else
                        {
                            throw new Exception("Undefined variables cannot be assigned.");
                        }
                    }
                    else if (VariableTable.TryGetValue(_variableName, out Value result)) // Can x bench y?
                    {
                        if (result.Type.ToString() != _assignmentType.ToString())
                        {
                            throw new Exception("Variable being assigned is already defined and not of the correct type.");
                        }
                    }
                    
                    if (nodeOne == null) return new TerminalNode(start);
                    return new AssignmentVariableNode(start, nodeOne);
                }
                default:
                {
                    throw new Exception("Unrecognised token in assignment " + CurrentToken.Type);
                }
            }
        }

        private Node _parseRight()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    if (VariableTable.TryGetValue(CurrentToken.Content, out Value result))
                    {
                        if ((TokenType) result.Type != _assignmentType)
                        {
                            throw new Exception("Variables are not of the same type.");
                        }
                        if (result.Type == ValueType.Integer)
                        {
                            goto case TokenType.Integer;
                        }
                        if (result.Type == ValueType.String)
                        {
                            goto case TokenType.String;
                        }

                        throw new Exception("Variable being assigned is not a valid type.");
                    }
                    
                    throw new Exception("Variable being assigned is not defined.");
                }
                case TokenType.Integer:
                {
                    List<Token> calcTokens = GetRemainingTokens();
                    
                    // TODO Maybe expand plates, here not in lexer
                    // Exploits the fact that lone close brackets don't break anything
                    calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    calcTokens.Insert(calcTokens.Count - 1, new Token(TokenType.CloseBracket, ")"));

                    string calculation = new Calculator(calcTokens, ref VariableTable).ParseTree().Evaluate();
                    
                    if (!VariableTable.TryAdd(_variableName, new IntegerValue(calculation)))
                        VariableTable[_variableName] = new IntegerValue(calculation);
                        
                    return new VariableNode(new Token(TokenType.Integer, calculation), 
                        new TerminalNode(new Token(TokenType.EoL, ";")));
                }
                case TokenType.String:
                {
                    Token returnToken = CurrentToken;

                    if (CurrentToken.Type == TokenType.Id)
                    {
                        VariableTable.TryGetValue(CurrentToken.Content, out Value result);
                        returnToken = new Token(TokenType.String, ((StringValue) result).Content);
                    }
                    
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
        
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Can:
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


        // public static void Main()
        // {
        //     Dictionary<String, Value> var = new Dictionary<String, Value>();
        //     var.Add("x", new StringValue("this"));
        //     var.Add("y", new StringValue("\"this\""));
        //     
        //     Lexer lexer = new Lexer("can x \"4\"?");
        //     Assignment x = new Assignment(lexer.Tokens, ref var);
        //     Console.Out.WriteLine(x.ParseTree().Evaluate());
        //     
        //     // lexer = new Lexer("can x bench x + 5 * 2 / 1 plates?");
        //     // x = new Assignment(lexer.Tokens, ref var);
        //     // Console.Out.WriteLine(x.ParseTree().Evaluate());
        // }
    }
}