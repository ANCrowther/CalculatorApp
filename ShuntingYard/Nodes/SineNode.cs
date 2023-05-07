namespace ShuntingYardLibrary.Nodes;
public class SineNode : FunctionNode {
    public override decimal Evaluate() {
        return (decimal)Math.Sin((double)this.XNode.Evaluate());
    }
}
