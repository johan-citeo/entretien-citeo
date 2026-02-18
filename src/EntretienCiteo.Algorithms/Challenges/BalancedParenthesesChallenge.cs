namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE 2 — Balanced Parentheses
///
/// TODO: Implement the IsBalanced method below.
/// </summary>
public class BalancedParenthesesChallenge : IChallenge
{
    public string Title => "Balanced Parentheses";

    public string Description => """
        Given a string containing only '(', ')', '{', '}', '[', ']',
        determine if the input string is valid.

        A string is valid if:
        - Open brackets must be closed by the same type of bracket.
        - Open brackets must be closed in the correct order.

        Examples:
          "()"       => true
          "()[]{}"   => true
          "(]"       => false
          "([)]"     => false
          "{[]}"     => true
        """;

    public void RunTests()
    {
        var cases = new (string Input, bool Expected)[]
        {
            ("()", true),
            ("()[]{}", true),
            ("(]", false),
            ("([)]", false),
            ("{[]}", true),
            ("", true),
            ("{", false),
        };

        foreach (var (input, expected) in cases)
        {
            var result = IsBalanced(input);
            var label = $"IsBalanced(\"{input}\")";
            if (result == expected)
                Console.WriteLine($"  [PASS] {label} => {result}");
            else
                Console.WriteLine($"  [FAIL] {label} => expected {expected}, got {result}");
        }
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    public static bool IsBalanced(string s)
    {
        throw new NotImplementedException("TODO: implement IsBalanced");
    }
}
