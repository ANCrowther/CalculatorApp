namespace ShuntingYardLibrary.Nodes;
public class CosNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Cos((double)this.XNode.Evaluate());
    }
}
