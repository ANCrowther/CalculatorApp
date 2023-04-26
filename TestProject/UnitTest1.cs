using ShuntingYardLibrary;
using ShuntingYardLibrary.Nodes;
using ShuntingYardLibrary.Utilities;
using System.Collections.Generic;
using Xunit;

namespace TestProject;
public class UnitTest1 {
    [Theory]
    [InlineData(1.234)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(84532.4652)]
    public void TestNumberNode(decimal value) {
        INode node = new NumberNode(value);
        Assert.Equal(value, node.Evaluate());
    }

    [Theory]
    [InlineData("40 + 2")]
    [InlineData("29.25 + 12.75")]
    [InlineData("44 + -2")]
    [InlineData("0 + 42")]
    [InlineData("-11.75 + 53.75")]
    public void TestAdditionNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new AdditionNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Equal(42, node.Evaluate());
    }

    [Theory]
    [InlineData("44 - 2")]
    [InlineData("0 - -42")]
    [InlineData("1643.25 - 1601.25")]
    [InlineData("65 - 23")]
    [InlineData("540.5 - 498.5")]
    public void TestSubtractionNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new SubtractionNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Equal(42, node.Evaluate());
    }

    [Theory]
    [InlineData("21 * 2")]
    [InlineData("4 * 10.5")]
    [InlineData("-2 * -21")]
    [InlineData("0.5 * 84")]
    [InlineData("-28 * -1.5")]
    public void TestMultiplicationNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new MultiplicationNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Equal(42, node.Evaluate());
    }

    [Theory]
    [InlineData("84 / 2")]
    [InlineData("231 / 5.5")]
    [InlineData("52.5 / 1.25")]
    [InlineData("-31.5 / -0.75")]
    [InlineData("42 / 1")]
    public void TestDivisionNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new DivisionNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Equal(42, node.Evaluate());
    }

    [Theory]
    [InlineData("84 / 0")]
    [InlineData("231 / 0")]
    [InlineData("-52.5 / 0")]
    [InlineData("0 / 0")]
    public void TestDivisionByZeroNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new DivisionNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Throws<System.DivideByZeroException>(() => node.Evaluate());
    }

    [Theory]
    [InlineData("2 ^ 12")]
    [InlineData("4 ^ 6")]
    [InlineData("8 ^ 4")]
    public void TestExponentNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new ExponentNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Equal(4096, node.Evaluate());
    }

    [Theory]
    [InlineData("2 ^ 0")]
    [InlineData("4 ^ 0")]
    [InlineData("8 ^ 0")]
    [InlineData("0 ^ 0")]
    public void TestExponentZeroNode(string value) {
        string[] tempValue = value.Split(' ');
        OperatorNode node = new ExponentNode();
        node.LeftNode = new NumberNode(decimal.Parse(tempValue[0]));
        node.RightNode = new NumberNode(decimal.Parse(tempValue[2]));
        Assert.Equal(1, node.Evaluate());
    }

    [Theory]
    [InlineData("60+4")]
    [InlineData("120-56")]
    [InlineData("32*2")]
    [InlineData("128/2")]
    [InlineData("2^6")]
    [InlineData("(1 + 2 + 1)^4 / 4")]
    [InlineData("(((2+12)^2+68)/5.5)+16")]
    public void TestExpressionTreeEvaluation(string inputValue) {
        Assert.Equal("64", ExpressionTree.Evaluate(inputValue));
    }

    [Fact]
    public void TestNegativeEvaluation1() {
        Assert.Equal("-4", ExpressionTree.Evaluate("(-4)"));
    }

    [Fact]
    public void TestNegativeEvaluation2() {
        Assert.Equal("8", ExpressionTree.Evaluate("(4 + -2)^3"));
    }

    [Theory]
    [InlineData("(-5)+9")]
    [InlineData("(-16)/(-4)")]
    public void TestExpressionTreeNegativeNumbersEvaluation(string inputValue) {
        Assert.Equal("4", ExpressionTree.Evaluate(inputValue));
    }

    [Fact]
    public void TestInfixNodeCreation1() {
        List<INode> temp = Infix.Compile("-2");
        Assert.Equal(-1, temp[0].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.MultiplicationNode", temp[1].ToString());
        Assert.Equal(2, temp[2].Evaluate());
        OperatorNode answer = (OperatorNode)temp[1];
        answer.LeftNode = temp[0];
        answer.RightNode = temp[2];
        Assert.Equal(-2, answer.Evaluate());
    }

    [Fact]
    public void TestInfixNodeCreation2() {
        List<INode> temp = Infix.Compile("-(2+2)");
        Assert.Equal(-1, temp[0].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.MultiplicationNode", temp[1].ToString());
        Assert.Equal("ShuntingYardLibrary.Nodes.OpenParenthesisNode", temp[2].ToString());
        Assert.Equal(2, temp[3].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.AdditionNode", temp[4].ToString());
        Assert.Equal(2, temp[5].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.ClosedParenthesisNode", temp[6].ToString());
    }

    [Fact]
    public void TestPostFixNodeCreation1() {
        List<INode> temp = PostFix.Compile("2+2");
        Assert.Equal(2, temp[0].Evaluate());
        Assert.Equal(2, temp[1].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.AdditionNode", temp[2].ToString());
    }

    [Fact]
    public void TestPostFixNodeCreation2() {
        List<INode> temp = PostFix.Compile("(42+6)/2"); // 42 6 + 2 /
        Assert.Equal(42, temp[0].Evaluate());
        Assert.Equal(6, temp[1].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.AdditionNode", temp[2].ToString());
        Assert.Equal(2, temp[3].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.DivisionNode", temp[4].ToString());
    }
    [Fact]
    public void TestPostFixNodeCreation3() {
        List<INode> temp = PostFix.Compile("((4+2)/(4-1))^3"); // 4 2 + 4 1 - / 3 ^
        Assert.Equal(4, temp[0].Evaluate());
        Assert.Equal(2, temp[1].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.AdditionNode", temp[2].ToString());
        Assert.Equal(4, temp[3].Evaluate());
        Assert.Equal(1, temp[4].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.SubtractionNode", temp[5].ToString());
        Assert.Equal("ShuntingYardLibrary.Nodes.DivisionNode", temp[6].ToString());
        Assert.Equal(3, temp[7].Evaluate());
        Assert.Equal("ShuntingYardLibrary.Nodes.ExponentNode", temp[8].ToString());
    }

    [Fact]
    public void TestPostFixNodeCreation4() {
        string output = ExpressionTree.Evaluate("((4+2)/(4-1))^3");
        Assert.Equal("8", output);
    }
}