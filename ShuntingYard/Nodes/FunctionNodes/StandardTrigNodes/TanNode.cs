namespace ShuntingYardLibrary.Nodes;
internal class TanNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Tan((double)this.XNode.Evaluate());
    }
}
