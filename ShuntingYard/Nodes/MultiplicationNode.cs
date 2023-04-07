namespace ShuntingYardLibrary.Nodes;
public class MultiplicationNode : OperatorNode {
    public override decimal Evaluate() {
        return this.LeftNode.Evaluate() * this.RightNode.Evaluate();
    }
}
