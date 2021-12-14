/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Node.cs, Parse.cs, Value.cs
 * Last Modified :  06/12/21
 * Version :        1.4
 * Description :    Class for the storing of a value in the variable table and a function in the function table, using
 *                  this wrapper class, during the process of parsing.
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
        public readonly Dictionary<String, Value> VariableTable = new Dictionary<String, Value>();
        public readonly Dictionary<String, Function> FunctionTable = new Dictionary<String, Function>();
    }

    /// <summary>
    /// Abstract value class that stores the type of the value.
    /// </summary>
    public abstract class Value
    {
        public readonly TokenType Type;
        public readonly string Content;

        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="type"> Value type. </param>
        protected Value(TokenType type)
        {
            Type = type;
        }
        
        /// <summary>
        /// Protected constructor.
        /// </summary>
        /// <param name="type"> Value type. </param>
        /// <param name="content"> Value content. </param>
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

    /// <summary>
    /// Parameter class for storing a parameter of a function.
    /// </summary>
    public class Parameter
    {
        public readonly Token VariableType;     // Data type
        public readonly string VariableName;    // Variable identifier
        
        /// <summary>
        /// Constructor for a parameter.
        /// </summary>
        /// <param name="variableType"> Data type. </param>
        /// <param name="variableName"> Variable identifier. </param>
        public Parameter(Token variableType, string variableName)
        {
            VariableName = variableName;
            VariableType = variableType;
        }
        
        /// <summary>
        /// Parameter to string method printing out the parameter and its type.
        /// </summary>
        /// <returns> Function parameter. </returns>
        public override string ToString()
        {
            return VariableType.Content + VariableName;
        }
    }
    
    /// <summary>
    /// Function class inheriting from value.
    /// </summary>
    public class Function : Value
    {
        public readonly List<Parameter> Parameters; // List of parameters in function
        
        /// <summary>
        /// Inherited constructor that stores the function return type and the list of parameters in the function.
        /// </summary>
        /// <param name="returnValue"> Return type. </param>
        /// <param name="parameters"> Function parameters. </param>
        public Function(TokenType returnValue, List<Parameter> parameters) : base(returnValue)
        {
            Parameters = parameters;
        }
    }
}