// See https://aka.ms/new-console-template for more information

using ShuntingYardLibrary;
using ShuntingYardLibrary.Nodes;
using ShuntingYardLibrary.Utilities;

Console.WriteLine("Hello, World!");
//string answer = ExpressionTree.Evaluate("(2+1)^2/3");
//Console.WriteLine($"correct: 3, actual: {answer}");
//answer = ExpressionTree.Evaluate("1746.45627 - 844.78763");
//Console.WriteLine($"correct: 901.66864, actual: {answer}");
//answer = ExpressionTree.Evaluate("8762.984 / 24.6743");
//Console.WriteLine($"correct: 355.14620475555537543111658689, actual: {answer}");
//answer = ExpressionTree.Evaluate("2.5 ^ 3");
//Console.WriteLine($"correct: 15.625, actual: {answer}");
//answer = ExpressionTree.Evaluate("10 / (2 + 3)");
//Console.WriteLine($"correct: 2, actual: {answer}");

List<INode> postFix = PostFix.Compile("((4+2)/(4-1))^3");
foreach (INode node in postFix) {
    if (node is NumberNode) {
        Console.WriteLine($"{node.Evaluate()}");
    } else {
        Console.WriteLine(node.ToString());
    }
}

Console.ReadLine();
