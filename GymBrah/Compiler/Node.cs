<<<<<<< Updated upstream
﻿/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Node.cs, Parse.cs, Value.cs
 * Last Modified :  06/12/21
 * Version :        1.4
 * Description :    File of inherited node classes from the base abstract node class. Each of these nodes are used to
 *                  build the various tree's during the parsing process. General nodes have been created to cover
 *                  multiple uses, as well as unique nodes specific for their individual parse tree.
 */

//TODO Left, Right == null for terminal, content made generic

=======
﻿
using System;
using System.Collections.Generic;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    
    /// <summary>
    /// Abstract node that inherits from the base node, and has links to nodes on it's left and right.
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    
    /// <summary>
    /// Abstract node class that inherits from the base node, and is a terminal node storing the node token.
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    
    /// <summary>
    /// Abstract node class that inherits from the base node, and stores a link to a single node.
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    
    /// <summary>
    /// Terminal node that stores a token.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class TerminalNode : TokenNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="nodeToken"></param>
        public TerminalNode(Token nodeToken) : base(nodeToken)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluation method that returns the node's token content.
        /// </summary>
        /// <returns> String of token content </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _nodeToken.Content;
        }
    }

    // -------------------------------- Calculator Nodes
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node to add two nodes together.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class AddNode : LeftRightNode
    {

        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public AddNode(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the addition of the integer content of the left and right nodes.
        /// </summary>
        /// <returns> String of the integer addition. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) + int.Parse(_right.Evaluate())).ToString();
        }

    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the string of the addition of two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class AddNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public AddNodeString(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the addition of two nodes.
        /// </summary>
        /// <returns> String of addition </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _left.Evaluate() + "+" + _right.Evaluate();
        }

    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node that subtracts two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class SubtractNode : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public SubtractNode(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that return the integer subtraction of the two nodes.
        /// </summary>
        /// <returns> String of the integer subtraction. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) - int.Parse(_right.Evaluate())).ToString();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the string of the subtraction of two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class SubtractNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public SubtractNodeString(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the subtraction of the two nodes.
        /// </summary>
        /// <returns> String of subtraction </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _left.Evaluate() + "-" + _right.Evaluate();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the multiplication of two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class MultiplyNode : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public MultiplyNode(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method for the integer multiplication of two nodes.
        /// </summary>
        /// <returns> String of the integer multiplication. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) * int.Parse(_right.Evaluate())).ToString();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the string of the multiplication of two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class MultiplyNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public MultiplyNodeString(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the multiplication of two nodes.
        /// </summary>
        /// <returns> String of multiplication </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _left.Evaluate() + "*" + _right.Evaluate();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the division of two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class DivideNode : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public DivideNode(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method for the integer division of two nodes.
        /// </summary>
        /// <returns> String of the integer division. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return (int.Parse(_left.Evaluate()) / int.Parse(_right.Evaluate())).ToString();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the string of the division of two nodes.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class DivideNodeString : LeftRightNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public DivideNodeString(Node left, Node right) : base(left, right)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the division of two nodes.
        /// </summary>
        /// <returns> String of division. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _left.Evaluate() + "/" + _right.Evaluate();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the negation of a node.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class NegateNode : NodeNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="node"></param>
        public NegateNode(Node node) : base(node)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the integer negation of a node.
        /// </summary>
        /// <returns> Negated node. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return (-int.Parse(_node.Evaluate())).ToString();
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
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the negation of the node.
        /// </summary>
        /// <returns> String of negation. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return "-" + _node.Evaluate();
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Calculator node for the string of a bracketed node.
    /// </summary>
=======

>>>>>>> Stashed changes
    public class BracketNodeString : NodeNode
    {
        /// <summary>
        /// Inherited constructor.
        /// </summary>
        /// <param name="node"></param>
        public BracketNodeString(Node node) : base(node)
<<<<<<< Updated upstream
        {}
        
        /// <summary>
        /// Evaluate method that returns the string of the bracketed node.
        /// </summary>
        /// <returns> Bracketed expression. </returns>
=======
        { }

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return "(" + _node.Evaluate() + ")";
        }
    }

    // -------------------------------- Assignment Nodes
<<<<<<< Updated upstream
    
    /// <summary>
    /// Assignment node for the equals part of the expression.
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
        /// <summary>
        /// Evaluate method that returns the left hand side of the assignment expression.
        /// </summary>
        /// <returns> Variable being assigned. </returns>
=======

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content;
        }
    }
<<<<<<< Updated upstream
    
    /// <summary>
    /// Assignment node for the right hand side of the expression, concerning the content of the assignment.
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
        /// <summary>
        /// Evaluate method that returns the right hand side of the assignment expression.
        /// </summary>
        /// <returns> Content of assignment. </returns>
=======

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _nodeToken.Content + _right.Evaluate();
        }
    }

    // -------------------------------- Statement Nodes
<<<<<<< Updated upstream
    
    /// <summary>
    /// Statement node for the content of the "scream" key word.
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
        /// <summary>
        /// Evaluate method that returns the statement, it's content, and the EoL.
        /// </summary>
        /// <returns> Scream statement. </returns>
=======

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return _left.Evaluate() + _nodeToken.Content + _right.Evaluate();
        }
    }

    // -------------------------------- Boolean Nodes
<<<<<<< Updated upstream
    
    /// <summary>
    /// Boolean node for the boolean expressions that contain multiple characters e.g. "==".
    /// </summary>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
        /// <summary>
        /// Evaluate method that returns the multi-character boolean comparison.
        /// </summary>
        /// <returns> Boolean comparison. </returns>
=======

>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        
        /// <summary>
        /// Evaluate method that returns the boolean expression in brackets.
        /// </summary>
        /// <returns> Boolean comparison expression. </returns>
=======

>>>>>>> Stashed changes
        public override string Evaluate()
        {
            return "(" + _left.Evaluate() + _comparison.Evaluate() + _right.Evaluate() + ") {";
        }
    }

    // -------------------------------- Selection Nodes
<<<<<<< Updated upstream
    
    /// <summary>
    /// Selection node for the selection...
    /// </summary>
=======

>>>>>>> Stashed changes
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

    public class FunctionNode : Node
    {
        private readonly Token _functionName;
        private readonly Function _table;

        public FunctionNode(Token functionName, Function table)
        {
            _functionName = functionName;

            _table = table;
        }

        public override string Evaluate()
        {
            
            return _table.Type.Content + _functionName.Content + "(" + _table.ToString();
        }
    }

}