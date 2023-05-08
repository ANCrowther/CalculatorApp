namespace ShuntingYardLibrary.Nodes;
internal class TanhNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Tanh((double)this.XNode.Evaluate());
    }
}
