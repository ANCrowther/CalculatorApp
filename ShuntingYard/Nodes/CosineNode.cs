namespace ShuntingYardLibrary.Nodes;
public class CosineNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Cos((double)this.XNode.Evaluate());
    }
}
