﻿using ShuntingYardLibrary.Nodes;

namespace ShuntingYardLibrary.Utilities;
public static class NodeGenerator
{
    /// <summary>
    /// Turns inputString into nodes for compiling.
    /// </summary>
    /// <param name="inputString">string to be converted</param>
    /// <returns>Operator or Number node.</returns>
    public static INode MakeNode(string inputString) {
        OperatorNode operatorNode = OperationsNodeFactory.MakeNode(inputString);

        if (operatorNode != null) {
            return operatorNode;
        }
        else {
            //bool isDouble = double.TryParse(inputString, out double value);
            bool isDecimal = decimal.TryParse(inputString, out decimal value);

            return (isDecimal == true) ? new NumberNode(value) : null;
        }
    }
}
