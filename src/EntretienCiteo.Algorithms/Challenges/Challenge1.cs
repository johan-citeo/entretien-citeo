namespace EntretienCiteo.Algorithms.Challenges;

public class Challenge1 : IChallenge
{
    public string Title => "Challenge1";

    public string Description => "Anagramme";
    
    public void RunTests()
    {
        var words = new[] { "bao", "abab", "aob", "meteor", "zzzz", "remote", "thing", "night", "marie", "aimer", "zzz"};
        var result = ProcessWords(words);

        var expectedResults = new List<List<string>>
        {
            new() {"bao", "aob"},
            new() {"abab"},
            new() {"meteor", "remote"},
            new() {"thing", "night"},
            new() {"marie", "aimer"},
            new() {"zzzz"},
            new() {"zzz"},
        };
        
        if(expectedResults.All(x => IsValid(result, x)))
        {
            Console.WriteLine("  [PASS] Challenge1");
            return;
        }
        
        Console.WriteLine("  [FAIL] Challenge1");
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    private static List<List<string>> ProcessWords(string[] words)
    {
        throw new NotImplementedException("TODO: Challenge1");
    }

    private static bool IsValid(List<List<string>> result, List<string> expectedValues)
    {
        return result.Any(r => r.OrderBy(x => x).SequenceEqual(expectedValues.OrderBy(y => y)));
    }
}