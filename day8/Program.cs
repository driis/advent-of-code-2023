// Normally we will start by reading lines from an input file


using System.Diagnostics;

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var instructions = input[0];
var locations = input[2..].Select(Node.Parse).ToArray();
var lookup = locations.ToDictionary(x => x.Code);

long Solve(Node pos, Func<Node, bool> goal)
{
    int count = 0;
    while (!goal(pos))
    {
        var instr = instructions[count++ % instructions.Length];
        var next = instr == 'R' ? pos.Right : pos.Left;
        pos = lookup[next];
    }

    return count;
}
bool doPartOne = lookup.TryGetValue("AAA", out var node);
if (doPartOne)
{ 
    var answer = Solve(node!, n => n.Code == "ZZZ");
    WriteLine($"Part 1: Steps used {answer}");
}

long gcf(long a, long b)
{
    while (b != 0)
    {
        var temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

long lcm(long a, long b) => (a / gcf(a, b)) * b;

// Part 2
var nodes = locations.Where(x => x.IsStartingNode).ToArray();
WriteLine($"Part 2 starting with {nodes.Length} nodes: {String.Join(", ", nodes.Select(x=>x.Code))}.");
var answers = nodes.Select(n => Solve(n, x => x.IsEndNode)).ToArray();
WriteLine("Individual counts:");
answers.DumpConsole();
WriteLine("Finding least common multiple");
var answerPartTwo = answers.Aggregate(1L, (a, b) => lcm(a, b));
WriteLine($"Part 2 Answer: {answerPartTwo}");
public record Node(string Code, string Left, string Right)
{
    private static readonly Regex regParse = new Regex(@"(\w+) = \((\w+), (\w+)\)");
    public static Node Parse(string value)
    {
        var m = regParse.Match(value);
        if (!m.Success)
            throw new ApplicationException("parse failure");

        return new Node(m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value);
    }

    public bool IsStartingNode { get; } = Code.EndsWith('A');
    public bool IsEndNode { get; } = Code.EndsWith('Z');

    public char EndChar { get; } = Code[^1];
}