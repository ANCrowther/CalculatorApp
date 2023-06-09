﻿namespace ShuntingYardLibrary.Nodes;
public abstract class OperatorNode : INode {
    public char Precedence { get; set; }
    public INode LeftNode { get; set; } = null;
    public INode RightNode { get; set; } = null;

    public abstract decimal Evaluate();
}
