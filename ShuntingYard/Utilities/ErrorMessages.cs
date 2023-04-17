namespace ShuntingYardLibrary.Utilities;
internal class ErrorMessages {
    public static readonly string InvalidToken = $"{ 0 } is not valid";
    public static readonly string KeyNotFound =  $"{ 0 } is not present in the expression.";
    public static readonly string Mismatch =     $"ERR: mismatched parenthesis";
    public static readonly string DivideByZero = $"ERR: divide by zero";
    public static readonly string Unknown =      $"ERR: unknown reason";
}
