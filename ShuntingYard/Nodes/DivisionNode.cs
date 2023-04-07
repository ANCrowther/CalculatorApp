namespace ShuntingYardLibrary.Nodes;
public class DivisionNode : OperatorNode {
    public override decimal Evaluate() {
        try {
            decimal result = (decimal)this.LeftNode.Evaluate() / (decimal)this.RightNode.Evaluate();
            //return (double)result;
            return result;
        } catch {
            throw new DivideByZeroException("Cannot divide by zero.");
        }
    }
}
