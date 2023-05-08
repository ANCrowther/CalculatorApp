namespace ShuntingYardLibrary.Nodes;
internal class AsinNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Asin((double)this.XNode.Evaluate());
    }
}
