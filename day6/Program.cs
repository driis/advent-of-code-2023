// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var parts = input.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..]).ToArray();
var times = parts[0];
var distances = parts[1];
var races = times.Select((t, i) => new Race(t.ToInt(), distances[i].ToInt()));
var answer = races.Aggregate(1, (a, r) => a * r.WinningOptions);
WriteLine($"Part 1: Answer {answer}");

/* Part 2 */
var timeBig = String.Join("", times).ToInt();
var distanceBig = String.Join("", distances).ToLong();
var longRace = new Race(timeBig, distanceBig);
WriteLine(longRace);
WriteLine($"Part 2: Answer {longRace.WinningOptions}");
record Race(int Time, long Record)
{
    public long Distance(long timeAcc) => Time * timeAcc - timeAcc * timeAcc;

    public IEnumerable<long> WinningAccelerateTimes()
    {
        // Naive approach
        var distances = Enumerable.Range(1, Time).Select(t => Distance(t))
            .Where(d => d > Record);
        return distances;
    }

    public int WinningOptions => WinningAccelerateTimes().Count();
};
