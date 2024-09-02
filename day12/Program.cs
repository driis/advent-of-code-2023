// Normally we will start by reading lines from an input file
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var records = input.Select(Record.Parse).ToArray();

int sum = 0;
foreach (var rec in records)
{
    var permutations = rec.AllPermutations(0,0);
    sum += permutations.Count();
}

WriteLine($"Part 1: Sum of permutations {sum}");

record Record(char[] Condition, int[] ConsecutivePositions)
{
    public IEnumerable<Record> AllPermutations(int Pos, int Idx)
    {
        var chars = Condition[Pos..];
        var index = chars.IndexOf(ch => ch != '.');
        if (index == null)
            yield return this;
        
        var consec = ConsecutivePositions[Idx];
        if (chars[index!.Value] == '#')
        {
            foreach (var p in AllPermutations(Pos + consec, Idx + 1))
                yield return p;
        }
        else
        {
            // Unknown pos
            
        }
        
    }
    public static Record Parse(string line)
    {
        var parts = line.Split(' ');
        var pos = parts[1].Split(',').Select(n => n.ToInt()).ToArray();
        return new Record(parts[0].ToArray(), pos);
    }
}

public static class ExtensionsDay12
{
    public static int? IndexOf<T>(this T[] array, T what) => IndexOf(array, item => item.Equals(what));
    
    public static int? IndexOf<T>(this T[] array, Func<T,bool> condition)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (condition(array[i]))
                return i;
        }

        return null;
    }
}