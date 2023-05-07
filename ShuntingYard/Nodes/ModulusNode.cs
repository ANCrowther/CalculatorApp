namespace ShuntingYardLibrary.Nodes;
internal class ModulusNode : OperatorNode {
    public override decimal Evaluate() {
        decimal output = this.LeftNode.Evaluate() % this.RightNode.Evaluate();
        return output;
    }
}
