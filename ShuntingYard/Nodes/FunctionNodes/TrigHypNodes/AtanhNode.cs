namespace ShuntingYardLibrary.Nodes;
internal class AtanhNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Atanh((double)this.XNode.Evaluate());
    }
}
