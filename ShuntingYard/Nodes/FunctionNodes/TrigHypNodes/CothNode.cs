using System.Runtime.ExceptionServices;

namespace ShuntingYardLibrary.Nodes;
internal class CothNode : FunctionNode {
    public override decimal Evaluate() {
        try {
            return 1 / (decimal)Math.Tanh((double)this.XNode.Evaluate());
        } catch (DivideByZeroException ex) {
            ExceptionDispatchInfo.Capture(ex).Throw();
            throw;
        }
    }
}
