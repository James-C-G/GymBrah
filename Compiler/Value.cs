using System;
using System.Collections.Generic;
using Tokenizer;

namespace Compiler
{
    public class Table
    {
        public Dictionary<String, Value> VariableTable = new Dictionary<String, Value>();
    }
    
    public enum ValueType
    {
        Integer = TokenType.Integer,
        String = TokenType.String,
        Bool
    }
    public abstract class Value
    {
        public ValueType Type;

        protected Value(ValueType type)
        {
            Type = type;
        }
    }

    public class IntegerValue : Value
    {
        public int Content;

        public IntegerValue(int content) : base(ValueType.Integer)
        {
            Content = content;
        }
    }
    
    public class StringValue : Value
    {
        public string Content;

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