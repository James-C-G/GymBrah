
using Tokenizer;

namespace Compiler
{
    // -------------------------------- Abstract Nodes
    public abstract class Node
    {
        public abstract string Evaluate();
    }
    
    public abstract class LeftRightNode : Node
    {
        private protected readonly Node _left, _right;

        protected LeftRightNode(Node left, Node right)
        {
            _left = left; 
            _right = right;
        }
    }
    
    public abstract class TokenNode : Node
    {
        private protected readonly Token _nodeToken;

        protected TokenNode(Token nodeToken)
        {
            _nodeToken = nodeToken;
        }
    }
    
    public abstract class NodeNode : Node
    {
        private protected readonly Node _node;

        protected NodeNode(Node node)
        {
            _node = node;
        }
    }

    // -------------------------------- General Nodes
    
    public class TerminalNode : TokenNode
    {
        public TerminalNode(Token nodeToken) : base(nodeToken)
        {}
        
        public override string Evaluate()
        {
            return _nodeToken.Content;
        }
    }
    
    // -------------------------------- Calculator Nodes
    
    public class AddNode : LeftRightNode
    {

        public AddNode(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) + int.Parse(_right.Evaluate())).ToString();
        }

    }
    
    public class AddNodeString : LeftRightNode
    {
        public AddNodeString(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return _left.Evaluate() + "+" + _right.Evaluate();
        }

    }
    
    public class SubtractNode : LeftRightNode
    {
        public SubtractNode(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) - int.Parse(_right.Evaluate())).ToString();
        }
    }
    
    public class SubtractNodeString : LeftRightNode
    {
        public SubtractNodeString(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return _left.Evaluate() + "-" + _right.Evaluate();
        }
    }
    
    public class MultiplyNode : LeftRightNode
    {
        public MultiplyNode(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) * int.Parse(_right.Evaluate())).ToString();
        }
    }
    
    public class MultiplyNodeString : LeftRightNode
    {
        public MultiplyNodeString(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return _left.Evaluate() + "*" + _right.Evaluate();
        }
    }
    
    public class DivideNode : LeftRightNode
    {
        public DivideNode(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) / int.Parse(_right.Evaluate())).ToString();
        }
    }
    
    public class DivideNodeString : LeftRightNode
    {
        public DivideNodeString(Node left, Node right) : base(left, right)
        {}
        
        public override string Evaluate()
        {
            return _left.Evaluate() + "/" + _right.Evaluate();
        }
    }
    
    public class NegateNode : NodeNode
    {
        public NegateNode(Node node) : base(node)
        {}
        
        public override string Evaluate()
        {
            return (- int.Parse(_node.Evaluate())).ToString();
        }
    }

    public class NegateNodeString : NodeNode
    {
        public NegateNodeString(Node node) : base(node)
        {}
        
        public override string Evaluate()
        {
            return "-" + _node.Evaluate();
        }
    }
    
    public class BracketNodeString : NodeNode
    {
        public BracketNodeString(Node node) : base(node)
        {}
        
        public override string Evaluate()
        {
            return "(" + _node.Evaluate() + ")";
        }
    }
    
    // -------------------------------- Assignment Nodes
    
    public class EqualNode : LeftRightNode
    {
        private readonly Token _nodeToken;

        public EqualNode(Node left, Node right, Token token) : base(left, right)
        {
            _nodeToken = token;
        }

        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content + _right.Evaluate();
        }
    }

    public class AssignmentVariableNode : TokenNode
    {
        private readonly Node _left;

        public AssignmentVariableNode(Token token, Node left) : base(token)
        {
            _left = left;
        }
        
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content;
        }
    }
    
    public class VariableNode : TokenNode
    {
        private readonly Node _right;

        public VariableNode(Token token, Node right) : base(token)
        {
            _right = right;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }
    
    // -------------------------------- Statement Nodes
    
    public class ScreamContentNode : LeftRightNode
    {
        private readonly Token _nodeToken;

        public ScreamContentNode(Node left, Node right, Token nodeToken) : base(left, right)
        {
            _nodeToken = nodeToken;
        }
        
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content + _right.Evaluate();
        }
    }
    
    // -------------------------------- Boolean Nodes
    
    public class BoolStart : TokenNode
    {
        private readonly Node _right;

        public BoolStart(Token token, Node right) : base(token)
        {
            _right = right;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }

    public class BoolComparisonNode : LeftRightNode
    {
        private readonly Node _comparison;

        public BoolComparisonNode(Node left, Node right, Node comparison) : base(left, right)
        {
            _comparison = comparison;
        }
        
        public override string Evaluate()
        {
            return "(" + _left.Evaluate() + _comparison.Evaluate() + _right.Evaluate() + ") {";
        }
    }
    
    // -------------------------------- Selection Nodes
    
    public class SelectionNode : TokenNode
    {
        private readonly Node _right;

        public SelectionNode(Token token, Node right) : base(token)
        {
            _right = right;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }
}