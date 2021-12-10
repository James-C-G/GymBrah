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
    }
    
    /// <summary>
    /// Enum for the possible variable types that can be stored.
    /// </summary>
    public enum ValueType
    {
        Integer = TokenType.Integer,
        String = TokenType.String,
    }
    
    /// <summary>
    /// Abstract value class that stores the type of the value.
    /// </summary>
    public abstract class Value
    {
        public ValueType Type;

        protected Value(ValueType type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// Value class for the storage of integers.
    /// </summary>
    public class IntegerValue : Value
    {
        public string Content;

        /// <summary>
        /// Inherited constructor, and the content of the value.
        /// </summary>
        /// <param name="content"> Value content. </param>
        public IntegerValue(string content) : base(ValueType.Integer)
        {
            Content = content;
        }
    }
    
    /// <summary>
    /// Value class for the storage of strings.
    /// </summary>
    public class StringValue : Value
    {
        public string Content;

        /// <summary>
        /// Inherited constructor, and the content of the value.
        /// </summary>
        /// <param name="content"> Value content. </param>
        public StringValue(string content) : base(ValueType.String)
        {
            Content = content;
        }
        
        static void Main()
        {
            ValueType x = ValueType.Integer;
            Console.Out.WriteLine((TokenType)x);
        }
    }
}