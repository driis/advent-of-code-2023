List<int[]> Pyramid(int[] values)
{
    List<int[]> differences = new List<int[]> {values};
    while (differences[^1].Any(x => x != 0))
    {
        differences.Add(Differences(differences[^1]).ToArray());
    }

    differences.Reverse();
    return differences;
}
int Extrapolate(int[] values)
{
    var differences = Pyramid(values);
    return differences.Aggregate(0, (a, seq) => a + seq[^1]);
}

int ExtrapolateFirst(int[] values)
{
    var differences = Pyramid(values);
    return differences.Aggregate(0, (a, seq) => seq[0]-a);
}

IEnumerable<int> Differences(int[] values)
{
    for (int i = 0; i < values.Length - 1; i++)
        yield return values[i + 1] - values[i];
}

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var readings = input.Select(
    line => line.Split(' ').Select(x => x.ToInt())
        .ToArray())
    .ToArray();

var nextValues = readings.Select(Extrapolate).ToArray();
WriteLine($"Answer Part 1: {nextValues.Sum()}");

var firstValues = readings.Select(ExtrapolateFirst).ToArray();
firstValues.DumpConsole();
WriteLine($"Answer Part 2: {firstValues.Sum()}");