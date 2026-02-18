namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE 1 — FizzBuzz (Warm-up)
///
/// TODO: Implement the FizzBuzz method below.
/// </summary>
public class FizzBuzzChallenge : IChallenge
{
    public string Title => "FizzBuzz";

    public string Description => """
        Afficher les nombres de 1 to n.
        - Si divisible par 3, afficher "Fizz".
        - Si divisible par 5, afficher "Buzz".
        - Si divisible par 3 et 5, afficher "FizzBuzz".

        Renvoyer les résultats dans une List<string>.
        """;

    public void RunTests()
    {
        var result = FizzBuzz(15);

        var expected = new List<string>
        {
            "1", "2", "Fizz", "4", "Buzz",
            "Fizz", "7", "8", "Fizz", "Buzz",
            "11", "Fizz", "13", "14", "FizzBuzz"
        };

        AssertEqual(result, expected, "FizzBuzz(15)");
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    public static List<string> FizzBuzz(int n)
    {
        throw new NotImplementedException("TODO: implement FizzBuzz");
    }

    private static void AssertEqual(List<string> actual, List<string> expected, string testName)
    {
        if (actual.SequenceEqual(expected))
        {
            Console.WriteLine($"  [PASS] {testName}");
        }
        else
        {
            Console.WriteLine($"  [FAIL] {testName}");
            Console.WriteLine($"    Expected : [{string.Join(", ", expected)}]");
            Console.WriteLine($"    Got      : [{string.Join(", ", actual)}]");
        }
    }
}
