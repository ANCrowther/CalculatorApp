namespace ShuntingYardLibrary.Nodes;
public class NumberNode : INode {
    private readonly decimal value;

    public NumberNode(decimal value) {
        this.value = value;
    }
    public decimal Evaluate() {
        return this.value;
    }
}
