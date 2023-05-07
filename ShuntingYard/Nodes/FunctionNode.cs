namespace ShuntingYardLibrary.Nodes;
public abstract class FunctionNode : INode {
    public INode XNode { get; set; } = null;

    public abstract decimal Evaluate();
}
