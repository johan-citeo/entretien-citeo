namespace EntretienCiteo.Algorithms.Challenges;

public class Challenge1 : IChallenge
{
    public string Title => "Challenge 1";

    public string Description => "Surprise :)";
    
    public void RunTests()
    {
        var words = new[] { "bao", "abab", "aob", "meteor", "zzzz", "remote", "thing", "night", "marie", "aimer", "zzz"};
        var result = ProcessWords(words);

        if (result.Count != 7)
        {
            Console.WriteLine($"  [FAIL] Challenge1");
            return;
        }

        var expectedResults = new List<List<string>>
        {
            new() {"bao", "aob"},
            new() {"abab"},
            new() {"meteor", "remote"},
            new() {"thing", "night"},
            new() {"marie", "aimer"},
            new() {"zzzz"},
        };
        
        if(expectedResults.All(x => IsValid(result, x)))
        {
            Console.WriteLine($"  [PASS] Challenge1");
            return;
        }
        
        Console.WriteLine($"  [FAIL] Challenge1");
    }

    private static List<List<string>> ProcessWords(string[] words)
    {
        throw new NotImplementedException("TODO: implement FizzBuzz");
    }

    private static bool IsValid(List<List<string>> result, List<string> values)
    {
        return result.Any(r => r.SequenceEqual(values));
    }
}