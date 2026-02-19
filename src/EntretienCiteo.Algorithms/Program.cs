using EntretienCiteo.Algorithms.Challenges;

Console.WriteLine("=== Entretien Citeo — Algorithmic Challenges ===");
Console.WriteLine();

var challenges = new Dictionary<string, IChallenge>
{
    ["1"] = new Challenge1(),
    ["2"] = new Challenge2(),
    ["3"] = new Challenge3(),
    ["4"] = new Challenge4(),
};

while (true)
{
    Console.WriteLine("Select a challenge:");
    foreach (var (key, challenge) in challenges)
        Console.WriteLine($"  {key}. {challenge.Title}");
    Console.WriteLine("  q. Quit");
    Console.Write("> ");

    var input = Console.ReadLine()?.Trim().ToLower();
    if (input == "q") break;

    if (!challenges.TryGetValue(input ?? "", out var selected))
    {
        Console.WriteLine("Invalid choice.\n");
        continue;
    }

    Console.WriteLine();
    Console.WriteLine($"--- {selected.Title} ---");
    Console.WriteLine(selected.Description);
    Console.WriteLine();
    Console.WriteLine("Running tests...");

    selected.RunTests();

    Console.WriteLine();
}

Console.WriteLine("Goodbye!");
