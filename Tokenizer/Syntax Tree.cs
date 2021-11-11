/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs, Parser.cs, Syntax Tree.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */

using System;

namespace Tokenizer
{
    public class Node
    {
        public String Value { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }

        public Node()
        {
            Value = "";
            LeftChild = null;
            RightChild = null;
        }
        
        public Node(String value)
        {
            Value = value;
            LeftChild = null;
            RightChild = null;
        }

        public Node(String value, Node leftChild, Node rightChild)
        {
            Value = value;
            LeftChild = leftChild;
            RightChild = rightChild;
        }
        
        public override string ToString()
        {
            if (LeftChild == null || RightChild == null)
            {
                return Value + " ";
            }
            
            return String.Format("{0}{1}{2}", LeftChild, Value, RightChild);
        }

    }

    public class SyntaxTree
    {
        public Node Root { get; set; }

        public SyntaxTree()
        {
            Root = null;
        }
        
        public SyntaxTree(Node root)
        {
            Root = root;
        }

        public override string ToString()
        {
            return Root.ToString();
        }
    }
}