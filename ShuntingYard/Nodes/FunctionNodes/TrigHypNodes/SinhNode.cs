namespace ShuntingYardLibrary.Nodes;
internal class SinhNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Sinh((double)this.XNode.Evaluate());
    }
}
