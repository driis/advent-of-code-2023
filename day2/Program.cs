var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var games = input.Select(Game.Parse).ToArray();
var bag = new CubeSet(12,13,14);

var possibleGames = games.Where(g => g.Possible(bag));
var answer = possibleGames.Sum(g => g.Id);

WriteLine($"Part 1: {answer}\n");

var minimalSets = games.Select(g => g.MininmalSet);
var sumPower = minimalSets.Sum(s => s.Power);
WriteLine($"Part 2: {sumPower}");

record CubeSet(int Red, int Green, int Blue)
{
    private static Regex RegBlue = new Regex("(\\d+) blue");
    private static Regex RegRed = new Regex("(\\d+) red");
    private static Regex RegGreen = new Regex("(\\d+) green");
    public static CubeSet ParseDraw(string line)
    {
        return new CubeSet(
            ParseColor(RegRed, line),
            ParseColor(RegGreen, line),
            ParseColor(RegBlue, line)
        );        
    }

    static int ParseColor(Regex reg, string input)
    {
        var m = reg.Match(input);
        return m.Success ? 
            m.Groups[1].Value.ToInt() : 
            0;
    }   

    public int Power => Red * Green * Blue;     
};
record Game(int Id, CubeSet[] Draws)
{
    public bool Possible(CubeSet bag) => Draws.All(d => 
        d.Red <= bag.Red && 
        d.Blue <= bag.Blue &&
        d.Green <= bag.Green);

    public CubeSet MininmalSet => new CubeSet(
        Draws.Max(d => d.Red),
        Draws.Max(d => d.Green),
        Draws.Max(d => d.Blue));

    public static Game Parse(string line)
    {
        var match = GameReg.Match(line);
        int id = match.Groups[1].Value.ToInt();

        var drawStrings = line.Substring(match.Length).Split(';');

        return new Game(id, drawStrings.Select(CubeSet.ParseDraw).ToArray());
    }

    public override string ToString()
    {
        string d = String.Join("; ", Draws.Select(x => x.ToString()));
        return $"Id {Id}, {d}";
    }

    private static Regex GameReg = new Regex("^Game (\\d+):", RegexOptions.Compiled | RegexOptions.CultureInvariant);
};