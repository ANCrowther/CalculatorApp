namespace ShuntingYardLibrary.Nodes;
internal class CoshNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Cosh((double)this.XNode.Evaluate());
    }
}
