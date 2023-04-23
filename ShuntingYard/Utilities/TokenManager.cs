using System.Text.RegularExpressions;

namespace ShuntingYardLibrary.Utilities;
internal class TokenManager {
    /// <summary>
    /// Breaks apart the inputString to its component parts.
    /// </summary>
    /// <param name="inputString">String formula</param>
    /// <returns>List of component parts.</returns>
    public static List<string> Tokenize(string inputString) {
        string @pattern = @"[\d]+\.?[\d]*|[-/\+\*\(\)\^]";
        Regex rgx = new Regex(@pattern);
        MatchCollection matches = Regex.Matches(inputString, @pattern);

        return matches.Cast<Match>().Select(match => match.Value).ToList();
    }
}
