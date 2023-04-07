namespace ShuntingYardLibrary.Nodes;
public class AdditionNode : OperatorNode {
    public override decimal Evaluate() {
        return this.LeftNode.Evaluate() + this.RightNode.Evaluate();
    }
}
