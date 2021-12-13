/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Node.cs, Parse.cs, Value.cs
 * Last Modified :  10/12/21
 * Version :        1.4
 * Description :    File of inherited node classes from the base abstract node class. Each of these nodes are used to
 *                  build the various tree's during the parsing process. General nodes have been created to cover
 *                  multiple uses, as well as unique nodes specific for their individual parse tree.
 */

// TODO Left, Right == null for terminal, content made generic

using Tokenizer;

namespace Compiler
{
    // -------------------------------- Abstract Nodes
    
    /// <summary>
    /// Abstract node class to allow each node to be evaluated using the base method.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Abstract evaluate method to be called when parsing the given tree.
        /// </summary>
        /// <returns>Parsed string of the tree</returns>
        public abstract string Evaluate();
    }
    
    /// <summary>
    /// Abstract node that inherits from the base node, and has links to nodes on it's left and right.
    /// </summary>
    public abstract class LeftRightNode : Node
    {
        private protected readonly Node _left, _right;

        /// <summary>
        /// Constructor class.
        /// </summary>
        /// <param name="left"> Left Node </param>
        /// <param name="right"> Right node </param>
        protected LeftRightNode(Node left, Node right)
        {
            _left = left; 
            _right = right;
        }
    }
    
    /// <summary>
    /// Abstract node class that inherits from the base node, and is a terminal node storing the node token.
    /// </summary>
    public abstract class TokenNode : Node
    {
        private protected readonly Token _nodeToken;

        /// <summary>
        /// Constructor class.
        /// </summary>
        /// <param name="nodeToken"> Token for terminal node </param>
        protected TokenNode(Token nodeToken)
        {
            _nodeToken = nodeToken;
        }
    }
    
    /// <summary>
    /// Abstract node class that inherits from the base node, and stores a link to a single node.
    /// </summary>
    public abstract class NodeNode : Node
    {
        private protected readonly Node _node;

        /// <summary>
        /// Constructor class.
        /// </summary>
        /// <param name="node"> Node object </param>
        protected NodeNode(Node node)
        {
            _node = node;
        }
    }

    // -------------------------------- General Nodes
    
    /// <summary>
    /// Terminal node that stores a token.
    /// </summary>
    public class TerminalNode : TokenNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="nodeToken"></param>
        public TerminalNode(Token nodeToken) : base(nodeToken)
        {}
        
        /// <summary>
        /// Evaluation method that returns the node's token content.
        /// </summary>
        /// <returns> String of token content </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content;
        }
    }
    
    // -------------------------------- Calculator Nodes
    
    /// <summary>
    /// Calculator node to add two nodes together.
    /// </summary>
    public class AddNode : LeftRightNode
    {

        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public AddNode(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method that returns the addition of the integer content of the left and right nodes.
        /// </summary>
        /// <returns> String of the integer addition. </returns>
        public override string Evaluate()
        {
            return (double.Parse(_left.Evaluate()) + double.Parse(_right.Evaluate())).ToString();
        }

    }
    
    /// <summary>
    /// Calculator node for the string of the addition of two nodes.
    /// </summary>
    public class AddNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public AddNodeString(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the addition of two nodes.
        /// </summary>
        /// <returns> String of addition </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + "+" + _right.Evaluate();
        }

    }
    
    /// <summary>
    /// Calculator node that subtracts two nodes.
    /// </summary>
    public class SubtractNode : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public SubtractNode(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method that return the integer subtraction of the two nodes.
        /// </summary>
        /// <returns> String of the integer subtraction. </returns>
        public override string Evaluate()
        {
            return (double.Parse(_left.Evaluate()) - double.Parse(_right.Evaluate())).ToString();
        }
    }
    
    /// <summary>
    /// Calculator node for the string of the subtraction of two nodes.
    /// </summary>
    public class SubtractNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public SubtractNodeString(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the subtraction of the two nodes.
        /// </summary>
        /// <returns> String of subtraction </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + "-" + _right.Evaluate();
        }
    }
    
    /// <summary>
    /// Calculator node for the multiplication of two nodes.
    /// </summary>
    public class MultiplyNode : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public MultiplyNode(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method for the integer multiplication of two nodes.
        /// </summary>
        /// <returns> String of the integer multiplication. </returns>
        public override string Evaluate()
        {
            return (double.Parse(_left.Evaluate()) * double.Parse(_right.Evaluate())).ToString();
        }
    }
    
    /// <summary>
    /// Calculator node for the string of the multiplication of two nodes.
    /// </summary>
    public class MultiplyNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public MultiplyNodeString(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the multiplication of two nodes.
        /// </summary>
        /// <returns> String of multiplication </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + "*" + _right.Evaluate();
        }
    }
    
    /// <summary>
    /// Calculator node for the division of two nodes.
    /// </summary>
    public class DivideNode : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public DivideNode(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method for the integer division of two nodes.
        /// </summary>
        /// <returns> String of the integer division. </returns>
        public override string Evaluate()
        {
            return (double.Parse(_left.Evaluate()) / double.Parse(_right.Evaluate())).ToString();
        }
    }
    
    /// <summary>
    /// Calculator node for the string of the division of two nodes.
    /// </summary>
    public class DivideNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public DivideNodeString(Node left, Node right) : base(left, right)
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the division of two nodes.
        /// </summary>
        /// <returns> String of division. </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + "/" + _right.Evaluate();
        }
    }
    
    /// <summary>
    /// Calculator node for the negation of a node.
    /// </summary>
    public class NegateNode : NodeNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="node"></param>
        public NegateNode(Node node) : base(node)
        {}
        
        /// <summary>
        /// Evaluate method that returns the integer negation of a node.
        /// </summary>
        /// <returns> Negated node. </returns>
        public override string Evaluate()
        {
            return (- double.Parse(_node.Evaluate())).ToString();
        }
    }

    /// <summary>
    /// Calculator node for the string negation of a node.
    /// </summary>
    public class NegateNodeString : NodeNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="node"></param>
        public NegateNodeString(Node node) : base(node)
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the negation of the node.
        /// </summary>
        /// <returns> String of negation. </returns>
        public override string Evaluate()
        {
            return "-" + _node.Evaluate();
        }
    }
    
    /// <summary>
    /// Calculator node for the string of a bracketed node.
    /// </summary>
    public class BracketNodeString : NodeNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="node"></param>
        public BracketNodeString(Node node) : base(node)
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the bracketed node.
        /// </summary>
        /// <returns> Bracketed expression. </returns>
        public override string Evaluate()
        {
            return "(" + _node.Evaluate() + ")";
        }
    }
    
    // -------------------------------- Assignment Nodes
    
    /// <summary>
    /// Assignment node for the equals part of the expression.
    /// </summary>
    public class EqualNode : LeftRightNode
    {
        private readonly Token _nodeToken;

        /// <summary>
        /// Inherited constructor, with addition of token for the node.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="token"> Token of equals node. </param>
        public EqualNode(Node left, Node right, Token token) : base(left, right)
        {
            _nodeToken = token;
        }

        /// <summary>
        /// Evaluate method that returns the left and right hand side of an expression with the equals sign in the
        /// middle.
        /// </summary>
        /// <returns> Assignment expression. </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content + _right.Evaluate();
        }
    }

    /// <summary>
    /// Assignment node for the left hand side of the expression, concerning variable assignment.
    /// </summary>
    public class AssignmentVariableNode : TokenNode
    {
        private readonly Node _left;

        /// <summary>
        /// Inherited constructor, and a link to the left node.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="left"> Left node. </param>
        public AssignmentVariableNode(Token token, Node left) : base(token)
        {
            _left = left;
        }
        
        /// <summary>
        /// Evaluate method that returns the left hand side of the assignment expression.
        /// </summary>
        /// <returns> Variable being assigned. </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content;
        }
    }
    
    /// <summary>
    /// Assignment node for the right hand side of the expression, concerning the content of the assignment.
    /// </summary>
    public class VariableNode : TokenNode
    {
        private readonly Node _right;

        /// <summary>
        /// Inherited constructor, and a link to the right node.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="right"> Right node. </param>
        public VariableNode(Token token, Node right) : base(token)
        {
            _right = right;
        }
        
        /// <summary>
        /// Evaluate method that returns the right hand side of the assignment expression.
        /// </summary>
        /// <returns> Content of assignment. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }
    
    // -------------------------------- Statement Nodes
    
    /// <summary>
    /// Statement node for the content of the "scream" key word.
    /// </summary>
    public class ScreamContentNode : LeftRightNode
    {
        private readonly Token _nodeToken;

        /// <summary>
        /// Inherited constructor, and the token for the current node.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="nodeToken"> Current node token. </param>
        public ScreamContentNode(Node left, Node right, Token nodeToken) : base(left, right)
        {
            _nodeToken = nodeToken;
        }
        
        /// <summary>
        /// Evaluate method that returns the statement, it's content, and the EoL.
        /// </summary>
        /// <returns> Scream statement. </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content + _right.Evaluate();
        }
    }
    
    // -------------------------------- Boolean Nodes
    
    /// <summary>
    /// Boolean node for the boolean expressions that contain multiple characters e.g. "==".
    /// </summary>
    public class BoolStart : TokenNode
    {
        private readonly Node _right;

        /// <summary>
        /// Inherited constructor, plus link to right node.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="right"> Right node. </param>
        public BoolStart(Token token, Node right) : base(token)
        {
            _right = right;
        }
        
        /// <summary>
        /// Evaluate method that returns the multi-character boolean comparison.
        /// </summary>
        /// <returns> Boolean comparison. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }

    /// <summary>
    /// Boolean node for the entire boolean expression.
    /// </summary>
    public class BoolComparisonNode : LeftRightNode
    {
        private readonly Node _comparison;

        /// <summary>
        /// Inherited constructor, and a link to the comparison node.
        /// </summary>
        /// <param name="left"> Left hand side of expression. </param>
        /// <param name="right"> Right hand side of expression. </param>
        /// <param name="comparison"> Boolean comparison. </param>
        public BoolComparisonNode(Node left, Node right, Node comparison) : base(left, right)
        {
            _comparison = comparison;
        }
        
        /// <summary>
        /// Evaluate method that returns the boolean expression in brackets.
        /// </summary>
        /// <returns> Boolean comparison expression. </returns>
        public override string Evaluate()
        {
            return "(" + _left.Evaluate() + _comparison.Evaluate() + _right.Evaluate() + ") {";
        }
    }
    
    // -------------------------------- Selection & Repetition Nodes
    
    /// <summary>
    /// Selection node for the selection and repetition parse trees.
    /// </summary>
    public class SelectionNode : TokenNode
    {
        private readonly Node _right;

        /// <summary>
        /// Inherited constructor plus a node link to the boolean node (right).
        /// </summary>
        /// <param name="token"></param>
        /// <param name="right"> Boolean expression node. </param>
        public SelectionNode(Token token, Node right) : base(token)
        {
            _right = right;
        }
        
        /// <summary>
        /// Evaluate method to return the parsed expression for selection and repetition.
        /// </summary>
        /// <returns> Parsed expression. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }
    
    // -------------------------------- Function Nodes
    
    /// <summary>
    /// Function node for the root of the parse tree.
    /// </summary>
    public class FunctionNode : LeftRightNode
    {
        private readonly Token _functionName;

        /// <summary>
        /// Inherited constructor and a token for the root node.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="functionName"> Name of function. </param>
        public FunctionNode(Node left, Node right, Token functionName) : base(left, right)
        {
            _functionName = functionName;
        }

        /// <summary>
        /// Evaluate method that returns the completed function definition.
        /// </summary>
        /// <returns> Function definition. </returns>
        public override string Evaluate()
        {
            return _left.Evaluate() + _functionName.Content + "(" + _right.Evaluate() + "){";
        }
    }

    /// <summary>
    /// Parameter node that links a parameter in the function definition to the next.
    /// </summary>
    public class ParameterNode : NodeNode
    {
        private readonly Token _paramType;
        private readonly Token _paramId;
        
        /// <summary>
        /// Inherited constructor with additional tokens for the parameters type and identifier.
        /// </summary>
        /// <param name="paramType"> Parameter data type. </param>
        /// <param name="paramId"> Parameter variable name. </param>
        /// <param name="node"></param>
        public ParameterNode(Token paramType, Token paramId, Node node) : base(node)
        {
            _paramType = paramType;
            _paramId = paramId;
        }
        
        /// <summary>
        /// Evaluate method to return the list of parameters in the function definition.
        /// </summary>
        /// <returns> Listed parameters. </returns>
        public override string Evaluate()
        {
            return _paramType.Content + _paramId.Content + ", " + _node.Evaluate();
        }
    }

    /// <summary>
    /// Parameter node for the final (or only) parameter in a function definition.
    /// </summary>
    public class FinalParameterNode : TokenNode
    {
        private readonly Token _paramId;
        
        /// <summary>
        /// Inherited constructor with additional token for parameter identifier.
        /// </summary>
        /// <param name="paramType"> Parameter data type.</param>
        /// <param name="paramId"> Parameter identifier. </param>
        public FinalParameterNode(Token paramType, Token paramId) : base(paramType)
        {
            _paramId = paramId;
        }

        /// <summary>
        /// Evaluate method that returns a single parameter definition.
        /// </summary>
        /// <returns> Parameter definition. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + _paramId.Content;
        }
    }

    /// <summary>
    /// Function method root node for when a function is being called.
    /// </summary>
    public class FunctionCallNode : TokenNode
    {
        private readonly Node _params;
        
        /// <summary>
        /// Inherited constructor with link to the parameters in function call.
        /// </summary>
        /// <param name="nodeToken"></param>
        /// <param name="param"> Parameters. </param>
        public FunctionCallNode(Token nodeToken, Node param) : base(nodeToken)
        {
            _params = param;
        }
        
        /// <summary>
        /// Evaluate method to return the function call parse tree.
        /// </summary>
        /// <returns> Function call. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + "(" + _params.Evaluate() + ");";
        }
    }
    
    /// <summary>
    /// Function method for the parameters being passed in the function call.
    /// </summary>
    public class FunctionCallParameterNode : TokenNode
    {
        private readonly Node _params;
        
        /// <summary>
        /// Inherited constructor with additional node to parameters in function call.
        /// </summary>
        /// <param name="nodeToken"></param>
        /// <param name="param"> Parameters. </param>
        public FunctionCallParameterNode(Token nodeToken, Node param) : base(nodeToken)
        {
            _params = param;
        }
        
        /// <summary>
        /// Evaluate method for the parameters in function call.
        /// </summary>
        /// <returns> Parameters. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + "," + _params.Evaluate();
        }
    }
    
    // -------------------------------- Return Nodes

    /// <summary>
    /// Return node for the return keyword and the value being returned.
    /// </summary>
    public class ReturnNode : TokenNode
    {
        private readonly Node _returnVal;
        
        /// <summary>
        /// Inherited constructor with link to return value node.
        /// </summary>
        /// <param name="nodeToken"></param>
        /// <param name="returnVal"> Return value node. </param>
        public ReturnNode(Token nodeToken, Node returnVal) : base(nodeToken)
        {
            _returnVal = returnVal;
        }
        
        /// <summary>
        /// Evaluate method to generate the return expression.
        /// </summary>
        /// <returns> Return expression. </returns>
        public override string Evaluate()
        {
            return _nodeToken.Content + _returnVal.Evaluate() + ";";
        }
    }
}