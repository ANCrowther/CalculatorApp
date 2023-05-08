namespace ShuntingYardLibrary.Nodes;
internal class AtanNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Atan((double)this.XNode.Evaluate());
    }
}
