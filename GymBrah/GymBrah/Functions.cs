using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler;
using Tokenizer;

namespace GymBrah
{
    public class Functions : Parse

    {
        public Functions(List<Token> tokens, ref Dictionary<String, Value> variableTable, ref Dictionary<String, FunctionTable> functions) : base(tokens, ref variableTable, ref functions) { }

        public FunctionParameters CreateNewParameters(List<FunctionParameters> parameters)
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Bench:
                case TokenType.Squat:
                    {
                        Token vartype = ScanToken();
                        Token parameter = ScanToken();
                        if (parameter.Type != TokenType.Id)
                        {
                            throw new Exception("Expected parameter, got" + " " + parameter.Type);
                        }
                        else
                        {
                            if (parameters.Count() > 0)
                            {
                                for (int i = 0; i < parameters.Count; i++)
                                {
                                    // checking for parameters with the same name, as this is not allowed
                                    if (parameter.Content == parameters[i]._variablename.Content)
                                    {
                                        throw new Exception("Cannot have parameters with the same name");
                                    }
                                    else
                                    {
                                        return new FunctionParameters(vartype, parameter);
                                    }
                                }
                            }
                            else
                            {
                                return new FunctionParameters(vartype, parameter);
                            }
                            // do error here
                            return null;
                        }

                    }
                // if comma, scan past
                case TokenType.Comma:
                    {
                        ScanToken();
                        break;
                    }
            }
            return null;
        }
        public Node ParseExistingFunction()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                    {
                        Token functionname = ScanToken();
                        // if function exists, check type correctness of parameters
                        if (Functions.TryGetValue(functionname.Content, out FunctionTable result))
                        {

                            Function currentfunctionparameters = (Function)result;

                            if (CurrentToken.Type == TokenType.OpenBracket)
                            {
                                int i = 0;
                                ScanToken();
                                List<string> variables = new List<string>();
                                while (CurrentToken.Type != TokenType.CloseBracket)
                                {
                                    if (CurrentToken.Type == TokenType.Comma)
                                    {
                                        ScanToken();
                                    }
                                    Token var = ScanToken();
                                    VariableTable.TryGetValue(var.Content, out Value vart);

                                    if (vart.Type == Compiler.ValueType.String && currentfunctionparameters.Parameters[i]._variabletype.Type == TokenType.Squat)
                                    {
                                        variables.Add(var.Content);
                                        i++;
                                        continue;
                                    }
                                    else if (vart.Type == Compiler.ValueType.Integer && currentfunctionparameters.Parameters[i]._variabletype.Type == TokenType.Bench)
                                    {
                                        variables.Add(var.Content);
                                        i++;
                                        continue;
                                    }
                                    else
                                    {
                                        throw new Exception("Mismatched parameter inputs");
                                    }
                                    ScanToken();
                                }
                               
                                string outputstring = functionname.Content + "(";
                                for(int c = 0;c< variables.Count; c++)
                                {
                                    outputstring += variables[c] + ",";
                                }
                                outputstring = outputstring.Remove(outputstring.Length - 1);
                                outputstring += ")";
                                Token output = new Token(TokenType.Brosplit, outputstring);
                                TerminalNode t = new TerminalNode(output);
                                return t;
                            }

                        }
                        break;
                    }
            }
            return null;
        }
        public override Node ParseTree()
        {
            switch (CurrentToken.Type)
            {
                case TokenType.Id:
                    {
                        return ParseExistingFunction();
                    }
                case TokenType.Brosplit:
                    {
                        ScanToken();
                        Token functionreturntype = ScanToken();
                        switch (functionreturntype.Type)
                        {
                            case TokenType.Bench:
                            case TokenType.Squat:
                                {
                                    Token functionname = ScanToken();
                                    // if function exists, check type correctness of parameters
                                    if (CurrentToken.Type == TokenType.OpenBracket)
                                    {
                                        List<FunctionParameters> parameters = new List<FunctionParameters>();
                                        while (ScanToken().Type != TokenType.CloseBracket)
                                        {
                                            parameters.Add(CreateNewParameters(parameters));
                                        }

                                        if (!Functions.TryAdd(functionname.Content, new Function(functionreturntype, parameters)))
                                        {
                                            throw new Exception("Function already exists");
                                        };

                                        Functions.TryGetValue(functionname.Content, out FunctionTable v);
                                        Function currentfunction = (Function)v;
                                        FunctionNode newfunction = new FunctionNode(functionname, currentfunction);
                                        return newfunction;
                                    }
                                    ScanToken();

                                    break;
                                }
                        }

                        return null;
                    }
                default:
                    {
                        throw new Exception("Illegal token found");
                    }
            }
        }
        public static void Main()
        {
            Lexer lexer = new Lexer("brosplit squat add(bench x,squat y,bench z)lightweight baby");
            Dictionary<String, Value> var = new Dictionary<String, Value>();
            Dictionary<String, FunctionTable> func = new Dictionary<String, FunctionTable>();
            Functions x = new Functions(lexer.Tokens, ref var, ref func);
            Console.Out.WriteLine(x.ParseTree().Evaluate());
            Lexer lexer2 = new Lexer("add(x,y,z)lightweight baby");
            var.Add("x", new IntegerValue("3"));
            var.Add("y", new StringValue("eee"));
            var.Add("z", new IntegerValue("4"));
            Functions x2 = new Functions(lexer2.Tokens, ref var, ref func);
            Console.Out.WriteLine(x2.ParseTree().Evaluate());

        }
    }

}
