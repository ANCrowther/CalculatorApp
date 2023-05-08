namespace ShuntingYardLibrary.Nodes;
internal class AcosNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Acos((double)this.XNode.Evaluate());
    }
}
