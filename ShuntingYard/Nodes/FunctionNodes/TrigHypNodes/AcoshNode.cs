namespace ShuntingYardLibrary.Nodes;
internal class AcoshNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Acosh((double)this.XNode.Evaluate());
    }
}
