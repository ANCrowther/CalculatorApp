namespace ShuntingYardLibrary.Nodes;
public class SinNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Sin((double)this.XNode.Evaluate());
    }
}
