namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE 3 
///
/// TODO: Implement the TopKFrequent method below.
/// </summary>
public class Challenge3 : IChallenge
{
    public string Title => "Challenge3";

    public string Description => """
        En se basant sur un tableau d'entiers et un entier k, il faut retourner les k
        éléments les plus fréquents. (Peu importe l'ordre)

        Exemples:
          [1,1,1,2,2,3], k=2  =>  [1, 2]
          [1], k=1             =>  [1]
          
        """;

    public void RunTests()
    {
        var cases = new (int[] Nums, int K, int[] Expected)[]
        {
            (new[] { 1, 1, 1, 2, 2, 3 }, 2, new[] { 1, 2 }),
            (new[] { 1 }, 1, new[] { 1 }),
            (new[] { 4, 4, 4, 3, 3, 2, 1 }, 2, new[] { 4, 3 }),
        };

        foreach (var (nums, k, expected) in cases)
        {
            var result = TopKFrequent(nums, k).OrderByDescending(x => x).ToArray();
            var expectedSorted = expected.OrderByDescending(x => x).ToArray();
            var label = $"TopKFrequent([{string.Join(",", nums)}], {k})";

            Console.WriteLine(result.SequenceEqual(expectedSorted)
                ? $"  [PASS] {label} => [{string.Join(", ", result)}]"
                : $"  [FAIL] {label} => expected [{string.Join(", ", expectedSorted)}], got [{string.Join(", ", result)}]");
        }
    }

    // =============================================
    // TODO: Implement this method
    // =============================================
    public static int[] TopKFrequent(int[] nums, int k)
    {
        throw new NotImplementedException("TODO: implement TopKFrequent");
    }
}
