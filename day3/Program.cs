// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var engine = new Engine(input);
var partNumbers = engine.PartNumbers();
WriteLine("Part 1:");
WriteLine(partNumbers.Sum(x => x.Value));

WriteLine("\nPart 2:");
var gears = engine.Gears();
WriteLine(gears.Sum(x => x.Ratio));

record Engine(string[] Data)
{
    public IEnumerable<Gear> Gears()
    {
        var partNumbers = PartNumbers().ToArray();
        for (int y = 0; y < Data.Length; y++)
        {
            string line = Data[y];
            for (int x = 0; x < line.Length; x++)
            {
                char c = line[x];
                if (c == '*')
                {
                    var parts = partNumbers.Where(p => p.IsAdjacent(x, y)).ToArray();
                    if (parts.Length == 2)
                    {
                        yield return new Gear(x, y, parts[0], parts[1]);
                    }
                }
            }
        }
    }
    
    public IEnumerable<PartNumber> PartNumbers()
    {
        for (int l = 0; l < Data.Length; l++)
        {
            int offset = 0;
            string line = Data[l];
            while (line.Length > 0)
            {
                var digitsHere = line.TakeWhile(Char.IsDigit).ToArray();
                if (digitsHere.Any())
                {
                    int n = new string(digitsHere).ToInt();
                    if (IsPartNumber(offset, l, digitsHere))
                        yield return new PartNumber(n, offset, l);
                }
                
                var symbbolsHere = line.TakeWhile(ch => !Char.IsDigit(ch)).ToArray();
                int advance = digitsHere.Length + symbbolsHere.Length;
                offset += advance;
                line = line[advance..];
            }
        }
    }

    public bool IsPartNumber(int x, int y, char[] candidate)
    {
        int beginLine = y > 0 ? y - 1 : y;
        int endLine = y + 2 < Data.Length ? y + 2 : y;
        int beginX = x > 0 ? x - 1 : x;
        int endX = x + candidate.Length < Data[y].Length ? x + candidate.Length + 1 : Data[y].Length;  
        var lines = Data[beginLine..endLine];
        var partsOfLines = lines.Select(line => line[beginX..endX]).ToArray();
        var charsToCheck = partsOfLines.SelectMany(ch => ch);
        return charsToCheck.Any(x => !Char.IsDigit(x) && x != '.');
    }
}

record PartNumber(int Value, int X, int Y)
{
    private int Length => Value.ToString().Length;
    public bool IsAdjacent(int x, int y)
    {
        int beginX = X - 1;
        int endX = X + Length;

        return (y >= Y - 1 && y <= Y + 1) &&
               (x >= beginX && x <= endX);

    }
}

record Gear(int X, int Y, PartNumber First, PartNumber Second)
{
    public int Ratio => First.Value * Second.Value;
}