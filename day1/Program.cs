var tokens = new (string,int)?[]
{
    ("one", 1), ("two", 2), ("three", 3), ("four", 4), ("five", 5), ("six", 6), ("seven", 7), ("eight", 8), ("nine", 9),
    ("1", 1), ("2", 2), ("3", 3), ("4", 4), ("5", 5), ("6", 6), ("7", 7), ("8", 8), ("9", 9)
};

(string,int)? PickTokenFromStart(string input) => tokens.SingleOrDefault(t => input.StartsWith(t!.Value.Item1));

int DigitsFromLine(string line)
{
    var digits = new List<(string,int)>();
    int offset = 0;
    while(line.Length > 0)
    {
        var token = PickTokenFromStart(line);
        if (token != null)
            digits.Add(token.Value);
        line = line[1..];
        offset++;
    }
    
    return Convert.ToInt32($"{digits.First().Item2}{digits.Last().Item2}");

}

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var coords = input.Select(DigitsFromLine);
var answer = coords.Sum();
Console.WriteLine(answer);