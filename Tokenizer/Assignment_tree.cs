/*
 * Author :         Jamie Grant & Pawel Bielinski
 * Files :          Lexer.cs, Tokens.cs, Parser.cs, Syntax Tree.cs
 * Last Modified :  06/10/21
 * Version :        1.4
 * Description :    
 */

using System;
using System.Collections.Generic;

namespace Tokenizer
{
    public class TreeNode<T> 
    {

        public T Data { get; set; }
        public TreeNode<T> Parent { get; set; }
        public ICollection<TreeNode<T>> Children { get; set; }

        public TreeNode(T data)
        {
            this.Data = data;
            this.Children = new LinkedList<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T child)
        {
            TreeNode<T> childNode = new TreeNode<T>(child) { Parent = this };
            this.Children.Add(childNode);
            return childNode;
        }

}
    //BNF :
    //    <assignment> ::= <variable type> <variable name> <value> <language multiplier> <endofline>
	   // |<variable type> <variable name> <endofline>
	   // |<variable type> <variable name> <value> <endofline>
	   // |<variable name> <value> <endofline>
	   // |<variable name> <variable name> <endofline>
    class AssignmentTree
    {

        TokenType lookahead(List<TokenType> t, int i) { return t[i]; }
        public TreeNode<TokenType> read(List<TokenType> t)
        {
            TreeNode<TokenType> root = new TreeNode<TokenType>(TokenType.root);
            try
            {
                
                int i = 0;
                switch (t[0])
                {
                    // set parent node
                    case TokenType.assignmentexpression:
                        i++;
                        TreeNode<TokenType> assignmentnode = root.AddChild(TokenType.assignmentexpression);
                        TreeNode<TokenType> vartype = assignmentnode.AddChild(TokenType.variabletype);
                        TreeNode<TokenType> left = assignmentnode.AddChild(TokenType.variablename);
                        TreeNode<TokenType> right = assignmentnode.AddChild(TokenType.right);
                        TreeNode<TokenType> multiplier = assignmentnode.AddChild(TokenType.multiplier);
                        TreeNode<TokenType> end = assignmentnode.AddChild(TokenType.end);
                        
                        switch (lookahead(t, i))
                        {
                            case TokenType.variablename:
                                TreeNode<TokenType> varnamenode = left.AddChild(TokenType.variablename);
                                i++;
                                switch (lookahead(t, i))
                                {
                                    case TokenType.value:
                                        TreeNode<TokenType> valuenode = right.AddChild(TokenType.value);
                                        i++;
                                        switch (lookahead(t, i))
                                        {
                                            case TokenType.Question:
                                                TreeNode<TokenType> endofexpressionnode = end.AddChild(TokenType.Question);
                                                break;
                                        }
                                        break;
                                    case TokenType.variablename:
                                        TreeNode<TokenType> varvalueassign = left.AddChild(TokenType.value);
                                        i++;
                                        switch (lookahead(t, i))
                                        {
                                            case TokenType.Question:
                                                TreeNode<TokenType> endofexpressionnode = end.AddChild(TokenType.Question);
                                                break;
                                        }
                                        break;
                                }
                                break;
                            case TokenType.variabletype:
                                TreeNode<TokenType> variabletype = vartype.AddChild(TokenType.variabletype);
                                i++;
                                switch (lookahead(t, i))
                                {
                                    case TokenType.variablename:
                                        i++;
                                        TreeNode<TokenType> varname = left.AddChild(TokenType.variablename);
                                        switch (lookahead(t, i))
                                        {
                                            case TokenType.Question:
                                                TreeNode<TokenType> endofexpression = end.AddChild(TokenType.Question);
                                                break;
                                            case TokenType.value:
                                                TreeNode<TokenType> value = right.AddChild(TokenType.value);
                                                i++;
                                                switch (lookahead(t, i))
                                                {
                                                    case TokenType.Question:
                                                        endofexpression = end.AddChild(TokenType.Question);
                                                        break;
                                                    case TokenType.langmultiplier:
                                                        TreeNode<TokenType> langmultiplier = multiplier.AddChild(TokenType.langmultiplier);
                                                        i++;
                                                        switch (lookahead(t, i))
                                                        {
                                                            case TokenType.Question:
                                                                endofexpression = end.AddChild(TokenType.Question);
                                                                break;
                                                        }
                                                        break;
                                                }
                                                break;

                                        }
                                        break;
                                }
                                break;
                        }
                        return root;

                }
            }

            catch(Exception ex)
            {
                Console.WriteLine("Too little/many args");
            }
            return root;
        }
        void isEOF() { }


    }

    class Program
    {
        static void Main(string[] args)
        {
            List<TokenType> tokens = new List<TokenType>();
            tokens.Add(TokenType.assignmentexpression);
            tokens.Add(TokenType.variabletype);
            tokens.Add(TokenType.variablename);
            tokens.Add(TokenType.value);
            tokens.Add(TokenType.langmultiplier);
            tokens.Add(TokenType.Question);
            AssignmentTree p = new AssignmentTree();
            TreeNode<TokenType> root = new TreeNode<TokenType>(TokenType.root);
            root = p.read(tokens);
        }
    }
}
