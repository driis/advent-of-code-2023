var digitStrings = new[]
{
    ("one", 1), ("two", 2), ("three", 3), ("four", 4), ("five", 5), ("six", 6), ("seven", 7), ("eight", 8), ("nine", 9),
    ("1", 1), ("2", 2), ("3", 3), ("4", 4), ("5", 5), ("6", 6), ("7", 7), ("8", 8), ("9", 9)
};
int[] SelectDigitsFromLine(string line)
{
    List<Digit> digits = new List<Digit>();
    foreach (var t in digitStrings)
    {
        var offset = line.IndexOf(t.Item1);
        if (offset < 0)
            continue;
        digits.Add(new(offset, t));
    }

    return digits.OrderBy(d => d.Offset).Select(d => d.Tuple.Item2).ToArray();
}
var input = File.ReadAllLines("input.txt");
var coords = input.Select(line =>
{
    var digits = SelectDigitsFromLine(line);
    return Convert.ToInt32($"{digits.First()}{digits.Last()}");
});
var answer = coords.Sum();
Console.WriteLine(answer);

record Digit(int Offset,(string,int) Tuple);