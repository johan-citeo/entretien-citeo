namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE 1 — HelloWorld
///
/// TODO: Implement the HelloWorld method below.
/// </summary>
public class Challenge2 : IChallenge
{
    public string Title => "Challenge2";

    public string Description => """
        Afficher les nombres de 1 to n.
        - Si divisible par 3, afficher "Hello".
        - Si divisible par 5, afficher "World".
        - Si divisible par 3 et 5, afficher "HelloWorld".

        Renvoyer les résultats dans une List<string>.
        """;

    public void RunTests()
    {
        var result = FizzBuzz(15);

        var expected = new List<string>
        {
            "1", "2", "Hello", "4", "World",
            "Hello", "7", "8", "Hello", "World",
            "11", "Hello", "13", "14", "HelloWorld"
        };

        AssertEqual(result, expected, "HelloWorld(15)");
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    public static List<string> FizzBuzz(int n)
    {
        throw new NotImplementedException("TODO: implement HelloWorld");
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
