﻿using System.Runtime.ExceptionServices;

namespace ShuntingYardLibrary.Nodes;
internal class CschNode : FunctionNode {
    public override decimal Evaluate() {
        try {
            return 1 / (decimal)Math.Cosh((double)this.XNode.Evaluate());
        } catch (DivideByZeroException ex) {
            ExceptionDispatchInfo.Capture(ex).Throw();
            throw;
        }
    }
}
