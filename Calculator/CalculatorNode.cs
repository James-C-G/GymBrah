using Tokenizer;

namespace Calculator
{
    public abstract class CalculatorNode
    {
        public abstract int Evaluate();
    }
    
    public class IntegerNode : CalculatorNode
    {
        private readonly Token _nodeToken;
        
        public IntegerNode(int value)
        {
            _nodeToken = new Token(TokenType.Integer, value);
        }
        public override int Evaluate()
        {
            return _nodeToken.IntegerContent;
        }
    }
    public class AddNode : CalculatorNode
    {
        private readonly CalculatorNode _left, _right;
        
        public AddNode(CalculatorNode left, CalculatorNode right)
        {
            _left = left;
            _right = right;
        }
        public override int Evaluate()
        {
            return _left.Evaluate() +  _right.Evaluate();
        }

    }
    public class SubtractNode : CalculatorNode
    {
        private readonly CalculatorNode _left, _right;
        
        public SubtractNode(CalculatorNode left, CalculatorNode right)
        {
            _left = left;
            _right = right;
        }
        public override int Evaluate()
        {
            return _left.Evaluate() - _right.Evaluate();
        }
    }
    public class MultiplyNode : CalculatorNode
    {
        private readonly CalculatorNode _left, _right;
        
        public MultiplyNode(CalculatorNode left, CalculatorNode right)
        {
            _left = left;
            _right = right;
        }
        public override int Evaluate()
        {
            return _left.Evaluate() * _right.Evaluate();
        }
    }
    public class DivideNode : CalculatorNode
    {
        private readonly CalculatorNode _left, _right;
        
        public DivideNode(CalculatorNode left, CalculatorNode right)
        {
            _left = left;
            _right = right;
        }
        public override int Evaluate()
        {
            return _left.Evaluate() / _right.Evaluate();
        }
    }
    
    public class NegateNode : CalculatorNode
    {
        private readonly CalculatorNode _node;
        
        public NegateNode(CalculatorNode node)
        {
            _node = node;
        }
        public override int Evaluate()
        {
            return - _node.Evaluate();
        }
    }
    
    public class IdNode : CalculatorNode
    {
        private readonly Token _nodeToken;
        
        public IdNode(int value)
        {
            _nodeToken = new Token(TokenType.Id, value);
        }
        public override int Evaluate()
        {
            return _nodeToken.IntegerContent;
        }
    }
}