// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var instructions = input[0];
int cur = 0;
var locations = input[2..].Select(Node.Parse).ToArray();
var lookup = locations.ToDictionary(x => x.Code);
var pos = "AAA";
int count = 0;
/*
while (pos != "ZZZ")
{
    var loc = lookup[pos];
    pos = instructions[cur] == 'R' ? loc.Right : loc.Left; 
    cur = ++cur % instructions.Length;
    count++;
}
WriteLine($"Part 1: Steps used {count}");
*/
// Part 2
count = 0;
cur = 0;
var nodes = locations.Where(x => x.IsStartingNode).ToArray();
while(!nodes.All(x => x.IsEndNode))
{
    var instr = instructions[cur];
    // Move all the nodes 
    nodes = nodes.Select(node => instr == 'R' ? lookup[node.Right] : lookup[node.Left]).ToArray();
    cur = ++cur % instructions.Length;
    count++;
    Console.WriteLine(new String(nodes.Select(x => x.EndChar).ToArray()));
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