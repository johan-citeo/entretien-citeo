namespace EntretienCiteo.Algorithms.Challenges;

/// <summary>
/// CHALLENGE 3 — Top K Frequent Elements
///
/// TODO: Implement the TopKFrequent method below.
/// </summary>
public class TopKFrequentChallenge : IChallenge
{
    public string Title => "Top K Frequent Elements";

    public string Description => """
        Given an integer array and an integer k, return the k most frequent elements.
        The answer may be returned in any order.

        Examples:
          [1,1,1,2,2,3], k=2  =>  [1, 2]
          [1], k=1             =>  [1]

        Constraint: Your solution must be better than O(n log n).
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

            if (result.SequenceEqual(expectedSorted))
                Console.WriteLine($"  [PASS] {label} => [{string.Join(", ", result)}]");
            else
                Console.WriteLine($"  [FAIL] {label} => expected [{string.Join(", ", expectedSorted)}], got [{string.Join(", ", result)}]");
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
