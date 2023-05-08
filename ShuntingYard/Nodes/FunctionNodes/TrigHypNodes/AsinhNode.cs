namespace ShuntingYardLibrary.Nodes;
internal class AsinhNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Asinh((double)this.XNode.Evaluate());
    }
}
