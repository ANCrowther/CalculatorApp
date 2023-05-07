// See https://aka.ms/new-console-template for more information

using ShuntingYardLibrary;
using ShuntingYardLibrary.Nodes;
using ShuntingYardLibrary.Utilities;

Console.WriteLine("Hello, World!");
string answer = ExpressionTree.Evaluate("3.2-2");
Console.WriteLine($"correct: 1, actual: {answer}");

Console.WriteLine("INFIX");
List<INode> inFix = Infix.Compile("3.2-2");
foreach (INode node in inFix) {
    if (node is NumberNode) {
        Console.WriteLine($"{node.Evaluate()}");
    } else {
        Console.WriteLine(node.ToString());
    }
}
Console.WriteLine("POSTFIX");
List<INode> postFix = PostFix.Compile("3.2-2");
foreach (INode node in postFix) {
    if (node is NumberNode) {
        Console.WriteLine($"{node.Evaluate()}");
    } else {
        Console.WriteLine(node.ToString());
    }
}

//answer = ExpressionTree.Evaluate("cos(0)/2");
//Console.WriteLine($"correct: 0.5, actual: {answer}");
////answer = ExpressionTree.Evaluate("8762.984 / 24.6743");
////Console.WriteLine($"correct: 355.14620475555537543111658689, actual: {answer}");
////answer = ExpressionTree.Evaluate("2.5 ^ 3");
////Console.WriteLine($"correct: 15.625, actual: {answer}");
////answer = ExpressionTree.Evaluate("10 / (2 + 3)");
////Console.WriteLine($"correct: 2, actual: {answer}");

//List<INode> postFix1 = PostFix.Compile("cos(0) / 2");
//foreach (INode node in postFix1) {
//    if (node is NumberNode) {
//        Console.WriteLine($"{node.Evaluate()}");
//    } else {
//        Console.WriteLine(node.ToString());
//    }
//}

Console.ReadLine();
