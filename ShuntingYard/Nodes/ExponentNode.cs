namespace ShuntingYardLibrary.Nodes;
public class ExponentNode : OperatorNode {
    public override decimal Evaluate() {
        return (decimal)Math.Pow((double)this.LeftNode.Evaluate(), (double)this.RightNode.Evaluate());
    }
}
