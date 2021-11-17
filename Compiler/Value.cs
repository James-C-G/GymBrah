using System;
using System.Collections.Generic;

namespace Compiler
{
    public static class Table
    {
        public static Dictionary<String, Value> VariableTable = new Dictionary<String, Value>();
    }
    
    public enum ValueType
    {
        Integer,
        String,
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
    
        }
    }
}