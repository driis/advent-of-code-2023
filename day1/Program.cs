var tokens = new (string,int)?[]
{
    ("one", 1), ("two", 2), ("three", 3), ("four", 4), ("five", 5), ("six", 6), ("seven", 7), ("eight", 8), ("nine", 9),
    ("1", 1), ("2", 2), ("3", 3), ("4", 4), ("5", 5), ("6", 6), ("7", 7), ("8", 8), ("9", 9)
};

(string,int)? PickTokenFromStart(string input) => tokens.SingleOrDefault(t => input.StartsWith(t!.Value.Item1));

int DigitsFromLine(string line)
{
    var digits = "";
    while(line.Length > 0)
    {
        var token = PickTokenFromStart(line);
        digits += token?.Item2.ToString() ?? "";
        line = line[1..];
    }
    
    return Convert.ToInt32($"{digits[0]}{digits[^1]}");
}

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var answer = input.Select(DigitsFromLine).Sum();
Console.WriteLine(answer);