namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE4 
///
/// TODO: Implement the LengthOfLongestSubstring method below.
/// </summary>
public class Challenge4 : IChallenge
{
    public string Title => "Palindrome";

    public string Description => "";

    public void RunTests()
    {
        var words = new[] {"madam", "test", "tenet", "okapi", "bob"};
        var result = ProcessWords(words);
        
        Console.WriteLine(result.OrderBy(r => r).SequenceEqual(["bob", "madam", "tenet"]) ?
            "  [PASS] Challenge4" :
            "  [FAIL] Challenge4"
            );
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    private static List<string> ProcessWords(string[] words)
    {
        throw new NotImplementedException("TODO: Challenge4");
    }
}
