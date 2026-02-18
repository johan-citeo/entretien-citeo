namespace EntretienCiteo.Algorithms.Challenges;

public interface IChallenge
{
    string Title { get; }
    string Description { get; }
    void RunTests();
}
