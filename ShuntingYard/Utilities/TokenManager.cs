﻿using System.Text.RegularExpressions;

namespace ShuntingYardLibrary.Utilities;
internal class TokenManager {
    public static List<string> Tokenize(string inputString) {
        string @pattern = @"[\d]+\.?[\d]*|[-/\+\*\(\)\^]";
        Regex rgx = new Regex(@pattern);
        MatchCollection matches = Regex.Matches(inputString, @pattern);

        return matches.Cast<Match>().Select(match => match.Value).ToList();
    }
}
