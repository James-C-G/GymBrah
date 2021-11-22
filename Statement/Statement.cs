using System;
using System.Collections.Generic;
using Compiler;
using Tokenizer;
using ValueType = Compiler.ValueType;

namespace Statement
{
    public class Statement : Parse<string>
    {
        public Statement(List<Token> tokens, ref Dictionary<String, Value> variableTable) : base(tokens, ref variableTable)
        {}

        private StatementNode _parseB()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Scream:
                {
                    return new TerminalNode(ScanToken());
                }
                case TokenType.EoL:
                {
                    // TODO Might be messy for when other statements are added
                    CurrentToken.StringContent = ")" + CurrentToken.StringContent;
                    return new TerminalNode(CurrentToken);
                }
                default:
                {
                    return null;
                }
            }
        }

        public override StatementNode ParseTree()
        {
            StatementNode nodeOne = _parseB();
            Token curToken = CurrentToken;
            ScanToken();
            
            switch (curToken.Type)
            {
                case TokenType.String:
                {
                    return new ScreamContentNode(nodeOne, _parseB(), curToken);
                }
                case TokenType.Integer:
                {
                    curToken = new Token(TokenType.String, "\"" + curToken.IntegerContent + "\"");
                    return new ScreamContentNode(nodeOne, _parseB(), curToken);
                }
                case TokenType.Id:
                {
                    if (VariableTable.TryGetValue(curToken.StringContent, out Value result))
                    {
                        switch(result.Type)
                        {
                            case ValueType.String:
                            {
                                curToken = new Token(TokenType.String, ((StringValue) result).Content);
                                break;
                            }
                            case ValueType.Integer:
                            {
                                curToken = new Token(TokenType.String, "\"" + ((IntegerValue) result).Content + "\"");
                                break;
                            }
                            case ValueType.Bool: // TODO Handle this
                            default:
                            {
                                throw new Exception("Can't print given value.");
                            }
                        }
                        
                        return new ScreamContentNode(nodeOne, _parseB(), curToken);
                    }

                    throw new Exception("ID not found");
                }
                default:
                {
                    return null;
                }
            }
        }
        
        public static void Main()
        {
            Lexer lexer = new Lexer("scream y!");
            Dictionary<String, Value> var = new Dictionary<String, Value>();
            var.Add("x", new StringValue("\"output\""));
            var.Add("y", new IntegerValue(10));
            
            Statement x = new Statement(lexer.Tokens, ref var);
            Console.Out.WriteLine(x.ParseTree().Evaluate());
        }
    }
}