namespace ShuntingYardLibrary.Nodes;
internal class TangentNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Tan((double)this.XNode.Evaluate());
    }
}
