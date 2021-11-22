using Compiler;
using Tokenizer;

namespace Statement
{
    public abstract class StatementNode : Node<string>
    {
        public abstract override string Evaluate();
    }

    public class ScreamContentNode : StatementNode
    {
        private readonly StatementNode _left;
        private readonly StatementNode _right;
        private readonly Token _nodeToken;

        public ScreamContentNode(StatementNode left, StatementNode right, Token nodeToken)
        {
            _left = left;
            _right = right;
            _nodeToken = nodeToken;
        }
        
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.StringContent + _right.Evaluate();
        }
    }

    public class TerminalNode : StatementNode
    {
        private readonly Token _nodeToken;

        public TerminalNode(Token nodeToken)
        {
            _nodeToken = nodeToken;
        }
        
        public override string Evaluate()
        {
            return _nodeToken.StringContent;
        }
    }
}