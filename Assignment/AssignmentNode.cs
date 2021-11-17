using Tokenizer;

namespace Assignment
{
    public abstract class AssignmentNode
    {
        public abstract string Evaluate();
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
    
    public class TypeIntegerNode : AssignmentNode
    {
        private readonly Token _nodeToken;
            
        public TypeIntegerNode(Token token)
        {
            _nodeToken = token;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.StringContent;
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
    
    public class EVarNode : AssignmentNode
    {
        private Token NodeToken;
        private AssignmentNode Right;

        public EVarNode(Token token, AssignmentNode right)
        {
            NodeToken = token;
            Right = right;
        }
        
        public override string Evaluate()
        {
            return NodeToken.StringContent + Right.Evaluate();
        }
    }
    
    public class AVarNode : AssignmentNode
    {
        private Token NodeToken;
        private AssignmentNode Left;

        public AVarNode(Token token, AssignmentNode left)
        {
            NodeToken = token;
            Left = left;
        }
        
        public override string Evaluate()
        {
            return Left.Evaluate() + NodeToken.StringContent;
        }
    }
    
    public class NVarNode : AssignmentNode
    {
        private Token NodeToken;

        public NVarNode(Token token)
        {
            NodeToken = token;
        }
        
        public override string Evaluate()
        {
            return NodeToken.StringContent;
        }
    }
}