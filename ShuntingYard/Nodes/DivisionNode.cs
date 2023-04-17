using System.Runtime.ExceptionServices;

namespace ShuntingYardLibrary.Nodes;
public class DivisionNode : OperatorNode {
    public override decimal Evaluate() {
        try {
            decimal result = this.LeftNode.Evaluate() / this.RightNode.Evaluate();
            return result;
        } catch (DivideByZeroException ex) {
            ExceptionDispatchInfo.Capture(ex).Throw();
            throw;
        }
    }
}
