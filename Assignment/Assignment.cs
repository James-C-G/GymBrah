using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace Assignment
{
    public class Assignment : Parse<string>
    {
        private TokenType _assignmentType = TokenType.Illegal;
        private string _variableName;
        public Assignment(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        private AssignmentNode _parseLeftB()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Bench:
                {
                    _assignmentType = TokenType.Integer;
                    return new TerminalNode(ScanToken());
                }
                default:
                {
                    return null;
                }
            }
        }
        
        private AssignmentNode _parseLeft()
        {
            Token start =  ScanToken();
            AssignmentNode nodeOne = _parseLeftB();
            
            switch (start.Type)
            {
                case TokenType.Id:
                {
                    _variableName = start.StringContent;

                    if (_assignmentType == TokenType.Illegal) // can x y?
                    {
                        if (VariableTable.TryGetValue(_variableName, out Value result))
                        {
                            _assignmentType = (TokenType) result.Type;
                        }
                        else
                        {
                            throw new Exception("Variable not assigned.");
                        }
                    }
                    else if (VariableTable.TryGetValue(_variableName, out Value result)) // Can x bench y?
                    {
                        if (result.Type.ToString() != _assignmentType.ToString())
                        {
                            throw new Exception("Not the same type.");
                        }
                    }
                   
                    
                    if (nodeOne == null) return new TerminalNode(start);
                    return new AssignmentVariableNode(start, nodeOne);
                }
                default:
                {
                    return null;
                }
            }
        }

        private AssignmentNode _parseRight()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                {
                    if (VariableTable.TryGetValue(CurrentToken.StringContent, out Value result))
                    {
                        if ((TokenType) result.Type != _assignmentType)
                        {
                            throw new Exception("Not of same type.");
                        }
                        if (result.Type == ValueType.Integer)
                        {
                            goto case TokenType.Integer;
                        }
                        if (result.Type == ValueType.String)
                        {
                            goto case TokenType.String;
                        }

                        throw new Exception("Not valid type.");
                    }
                    
                    throw new Exception("Variable not defined.");
                }
                case TokenType.Integer:
                {
                    List<Token> calcTokens = GetRemainingTokens();
                    
                    // TODO Maybe expand plates, here not in lexer
                    // Exploits the fact that lone close brackets don't break anything
                    calcTokens.Insert(0, new Token(TokenType.OpenBracket, "("));
                    calcTokens.Insert(calcTokens.Count - 1, new Token(TokenType.CloseBracket, ")"));

                    string calculation = new Calculator.Calculator(calcTokens, ref VariableTable).ParseTree().Evaluate().ToString();
                    if (!VariableTable.TryAdd(_variableName, new IntegerValue(Int32.Parse(calculation))))
                        VariableTable[_variableName] = new IntegerValue(Int32.Parse(calculation));
                        
                    return new VariableNode(new Token(TokenType.Integer, calculation), 
                        new TerminalNode(new Token(TokenType.EoL, ";")));
                }
                case TokenType.String:
                default:
                {
                    return null;
                }
            }
            
        }
        
        public override Node<string> ParseTree()
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
                    return null;
                }
            }
        }


        public static void Main()
        {
            Dictionary<String, Value> var = new Dictionary<String, Value>();
            var.Add("y", new IntegerValue(2));
            Lexer lexer = new Lexer("can x bench 2 3?");
            Assignment x = new Assignment(lexer.Tokens, ref var);
            Console.Out.WriteLine(x.ParseTree().Evaluate());
            
            // lexer = new Lexer("can x bench x + 5 * 2 / 1 plates?");
            // x = new Assignment(lexer.Tokens, ref var);
            // Console.Out.WriteLine(x.ParseTree().Evaluate());
        }
    }
}