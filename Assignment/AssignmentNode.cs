using Compiler;
using Tokenizer;

namespace Assignment
{
    public abstract class AssignmentNode : Node<string>
    {
        public abstract override string Evaluate();
    }
    
    public class EqualNode : AssignmentNode
    {
        private readonly AssignmentNode _left;
        private readonly AssignmentNode _right;
        private readonly Token _nodeToken;

        public EqualNode(AssignmentNode left, AssignmentNode right, Token token)
        {
            _left = left;
            _right = right;
            _nodeToken = token;
        }

        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.StringContent + _right.Evaluate();
        }
    }

    public class AssignmentVariableNode : AssignmentNode
    {
        private readonly Token _nodeToken;
        private readonly AssignmentNode _left;

        public AssignmentVariableNode(Token token, AssignmentNode left)
        {
            _nodeToken = token;
            _left = left;
        }
        
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.StringContent;
        }
    }
    
    public class VariableNode : AssignmentNode
    {
        private readonly Token _nodeToken;
        private readonly AssignmentNode _right;

        public VariableNode(Token token, AssignmentNode right)
        {
            _nodeToken = token;
            _right = right;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.StringContent + _right.Evaluate();
        }
    }
    
    public class TerminalNode : AssignmentNode
    {
        private readonly Token _nodeToken;

        public TerminalNode(Token token)
        {
            _nodeToken = token;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.StringContent;
        }
    }
}