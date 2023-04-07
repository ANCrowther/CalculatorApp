namespace ShuntingYardLibrary.Utilities;
internal class RegexStrings {
    public static string Match { get; private set; } = @"[A-Za-z]+[0-9]+";
    public static string TokenPattern { get; private set; } = @"[\d]+\.?[\d]*|[A-Za-z]+[0-9]+|[-/\+\*\(\)\^]";
    public static string TokenPatternExtended { get; private set; } = @"[\d]+\.?[\d]*|[A-Za-z]+[0-9]+|[-/\+\*\(\)\^]|.+";
}
