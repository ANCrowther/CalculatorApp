namespace ShuntingYardLibrary.Utilities;
internal class ErrorMessages {
    public static readonly string EmptyTree = "There is no expression to evaluate.";
    public static readonly string InvalidToken = $"{ 0 } is not valid";
    public static readonly string KeyNotFound = $"{ 0 } is not present in the expression.";
    public static readonly string Mismatch = "Expression has mismatched parenthesis.";
    public static readonly string UnknownValue = $"Cell { 0 }'s value is unknown.";
}
