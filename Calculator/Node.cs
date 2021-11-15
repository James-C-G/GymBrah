namespace Calculator
{
    public abstract class Node
    {
        public Token NodeToken;
        public abstract int Evaluate();
    }
    
    public class Integer : Node
    {
        public new Token NodeToken;
        public Integer(int value)
        {
            NodeToken = new Token(TokenType.Integer, value);
        }
        public override int Evaluate()
        {
            return NodeToken.IntegerContent;
        }
    }
    public class Add : Node
    {
        public Node Left, Right;
        public Add(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
        public override int Evaluate()
        {
            return Left.Evaluate() + Right.Evaluate();
        }

    }
    public class Subtract : Node
    {
        public Node Left, Right;
        public Subtract(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
        public override int Evaluate()
        {
            return Left.Evaluate() + Right.Evaluate();
        }
    }
    public class Multiply : Node
    {
        Node Left, Right;
        public Multiply(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
        public override int Evaluate()
        {
            return Left.Evaluate() * Right.Evaluate();
        }
    }
    public class Divide : Node
    {
        public Node Left, Right;
        public Divide(Node left, Node right)
        {
            Left = left;
            Right = right;
        }
        public override int Evaluate()
        {
            return Left.Evaluate() / Right.Evaluate();
        }
    }
    
    public class Negate : Node
    {
        public Node Value;
        
        public Negate(Node value)
        {
            Value = value;
        }
        public override int Evaluate()
        {
            return -Value.Evaluate();
        }
    }
}