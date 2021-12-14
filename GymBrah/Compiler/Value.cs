/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Node.cs, Parse.cs, Value.cs
 * Last Modified :  06/12/21
 * Version :        1.4
 * Description :    Class for the storing of a value in the variable table, using this wrapper class, during the process
 *                  of parsing.
 */

using System;
using System.Collections.Generic;
using Tokenizer;

namespace Compiler
{
    /// <summary>
    /// Class for the construction of the variable table.
    /// </summary>
    public class Table
    {
        public Dictionary<String, Value> VariableTable = new Dictionary<String, Value>();
        public Dictionary<String, Function> FunctionTable = new Dictionary<String, Function>();
    }

    /// <summary>
    /// Abstract value class that stores the type of the value.
    /// </summary>
    public abstract class Value
    {
        public readonly TokenType Type;
        public readonly string Content;

        protected Value(TokenType type)
        {
            Type = type;
        }
        
        protected Value(TokenType type, string content)
        {
            Type = type;
            Content = content;
        }
    }

    /// <summary>
    /// Value class for the storage of integers.
    /// </summary>
    public class IntegerValue : Value
    {
        /// <summary>
        /// Inherited constructor, and the content of the value.
        /// </summary>
        /// <param name="content"> Value content. </param>
        public IntegerValue(string content) : base(TokenType.Integer, content)
        {
        }
    }
    
    /// <summary>
    /// Value class for the storage of strings.
    /// </summary>
    public class StringValue : Value
    {
        /// <summary>
        /// Inherited constructor, and the content of the value.
        /// </summary>
        /// <param name="content"> Value content. </param>
        public StringValue(string content) : base(TokenType.String, content)
        {
        }
        
        static void Main()
        {
            Console.Out.WriteLine("");
        }
    }
    
    /// <summary>
    /// Value class for the storage of floats.
    /// </summary>
    public class DoubleValue : Value
    {
        /// <summary>
        /// Inherited constructor, and the content of the value.
        /// </summary>
        /// <param name="content"> Value content. </param>
        public DoubleValue(string content) : base(TokenType.Double, content)
        {
        }
    }

    public class Parameter
    {
        public readonly Token VariableType;
        public readonly string VariableName;
        
        public Parameter(Token variableType, string variableName)
        {
            VariableName = variableName;
            VariableType = variableType;
        }
        
        public override string ToString()
        {
            return VariableType.Content + VariableName;
        }
    }
    
    public class Function : Value
    {
        public readonly List<Parameter> Parameters;
        public Dictionary<String, Value> VariableTable = new Dictionary<string, Value>();

        public Function(TokenType returnValue, List<Parameter> parameters) : base(returnValue)
        {
            Parameters = parameters;

            foreach (var i in Parameters)
            {
                Value parameterValue;
                switch (i.VariableType.Type)
                {
                    case TokenType.Bench:
                    {
                        parameterValue = new IntegerValue("");
                        break;
                    }
                    case TokenType.Squat:
                    {
                        parameterValue = new StringValue("");
                        break;
                    }
                    case TokenType.DeadLift:
                    {
                        parameterValue = new DoubleValue("");
                        break;
                    }
                    default:
                    {
                        throw new Exception("Unrecognised variable type.");
                    }
                }
                
                VariableTable.Add(i.VariableName, parameterValue);
            }
        }

        public override string ToString()
        {
            string output = "";
            
            foreach (var i in Parameters)
            {
                output += i + ", ";
            }

            output = output.Remove(output.Length - 2);
            output += ")";
            
            return output;
        }

    }
}