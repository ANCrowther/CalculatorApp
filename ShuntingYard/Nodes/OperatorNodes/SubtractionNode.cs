namespace ShuntingYardLibrary.Nodes;
public class SubtractionNode : OperatorNode {
    public override decimal Evaluate() {
        return this.LeftNode.Evaluate() - this.RightNode.Evaluate();
    }
}
