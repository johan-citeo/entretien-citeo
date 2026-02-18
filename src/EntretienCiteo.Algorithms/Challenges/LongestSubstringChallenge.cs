namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE 4 — Longest Substring Without Repeating Characters
///
/// TODO: Implement the LengthOfLongestSubstring method below.
/// </summary>
public class LongestSubstringChallenge : IChallenge
{
    public string Title => "Longest Substring Without Repeating Characters";

    public string Description => """
        Given a string s, find the length of the longest substring
        without repeating characters.

        Examples:
          "abcabcbb"  =>  3  ("abc")
          "bbbbb"     =>  1  ("b")
          "pwwkew"    =>  3  ("wke")
          ""          =>  0
        """;

    public void RunTests()
    {
        var cases = new (string Input, int Expected)[]
        {
            ("abcabcbb", 3),
            ("bbbbb", 1),
            ("pwwkew", 3),
            ("", 0),
            ("abcdef", 6),
            ("dvdf", 3),
        };

        foreach (var (input, expected) in cases)
        {
            var result = LengthOfLongestSubstring(input);
            var label = $"LengthOfLongestSubstring(\"{input}\")";

            if (result == expected)
                Console.WriteLine($"  [PASS] {label} => {result}");
            else
                Console.WriteLine($"  [FAIL] {label} => expected {expected}, got {result}");
        }
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    public static int LengthOfLongestSubstring(string s)
    {
        throw new NotImplementedException("TODO: implement LengthOfLongestSubstring");
    }
}
