using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokenizer;

namespace Compiler
{
    public class FunctionTable
    {
        public Dictionary<String, FunctionTable> Functions = new Dictionary<String, FunctionTable>();
    }

    public abstract class FunctionValueType : FunctionTable
    {
        public ValueType Type;

        protected FunctionValueType(ValueType type)
        {
            Type = type;
        }
    }
    public class Function : FunctionValueType
    {
        public List<FunctionParameters> Parameters;
        public Token Type;
        public Function(Token type, List<FunctionParameters> parameters) : base(ValueType.Function)
        {
            Parameters = parameters;
            Type = type;
        }
        public override string ToString()
        {
            string output = "";
            for(int i = 0; i < Parameters.Count; i++)
            {
                output += Parameters[i].ToString() + ",";
            }
            output = output.Remove(output.Length - 1);
            output += ")";
            return output;
        }

    }
    public class FunctionParameters : FunctionTable
    {
        public Token _variabletype, _variablename;
        public FunctionParameters(Token variabletype, Token variablename)
        {
            _variablename = variablename;
            _variabletype = variabletype;
        }
        public override string ToString()
        {
            return _variabletype.Content + " " + _variablename.Content;
        }
    }
}
