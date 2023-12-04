// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var cards = input.Select(Card.Parse).ToArray();
var answer = cards.Sum(x => x.Points);

WriteLine("Part 1:");
WriteLine(answer);

WriteLine("Part 2:");
var lookup = cards.ToDictionary(x => x.Id);

void CountWinners(Card c)
{
    int win = c.WinningCount;
    for (int i = 1; i <= win; i++)
    {
        int id = c.Id + i;
        if (id > cards.Length)
            break;
        lookup[id].CopiesWon++;
    }
}

for (int i = 1; i <= cards.Length; i++)
{
    var process = lookup[i];
    for(int n = 0 ; n < process.CopiesWon ; n++)
        CountWinners(process);
}

int answerTwo = lookup.Values.Sum(x => x.CopiesWon);
WriteLine(answerTwo);

record Card(int Id, int[] Winning, int[] Numbers)
{
    private static readonly Regex regParse = new Regex(@"^Card\s+(\d+):(.*)$");
    public static Card Parse(string line)
    {
        var match = regParse.Match(line);
        if (!match.Success)
            throw new ApplicationException($"invalid input {line}");
        int id = match.Groups[1].Value.ToInt();
        var split = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        var parts = match.Groups[2].Value.Split('|', split);
        var winning = parts[0].Split(' ', split).Select(w => w.Trim().ToInt());
        var numbers = parts[1].Split(' ', split).Select(n => n.Trim().ToInt());
        return new Card(id, winning.ToArray(), numbers.ToArray());
    }

    public int WinningCount => Winning.Intersect(Numbers).Count();

    public int Points => WinningCount == 0 ? 0 : (int)Math.Pow(2, WinningCount-1);

    public int CopiesWon { get; set; } = 1;

};