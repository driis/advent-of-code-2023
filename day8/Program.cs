// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var instructions = input[0];
var locations = input[2..].Select(Node.Parse).ToArray();
var lookup = locations.ToDictionary(x => x.Code);
bool doPartOne = lookup.TryGetValue("AAA", out var pos);
int count = 0;

while (doPartOne && pos.Code != "ZZZ")
{
    var instr = instructions[count++ % instructions.Length];
    var next = instr == 'R' ? pos.Right : pos.Left;
    pos = lookup[next];
}
WriteLine($"Part 1: Steps used {count}");
// Part 2
count = 0;
var nodes = locations.Where(x => x.IsStartingNode).ToArray();
WriteLine($"Part 2 starting with {nodes.Length} nodes: {String.Join(", ", nodes.Select(x=>x.Code))}.");
while(!nodes.All(x => x.IsEndNode))
{
    var instr = instructions[count++ % instructions.Length];
    // Move all the nodes
    var nextCodes = nodes.Select(node => instr == 'R' ? node.Right : node.Left).ToArray();
    nodes = nextCodes.Select(n => lookup[n]).ToArray();
}
WriteLine($"Part 2: Steps used {count}");
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