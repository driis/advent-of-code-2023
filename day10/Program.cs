
var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var start = input.Select((s,y) => new Position('S', s.IndexOf('S'), y))
    .Single(line => line.X >= 0);

Position Move(Position from, Direction direction)
{
    (int x,int y) = direction switch{
        Direction.Up => (from.X, from.Y-1),
        Direction.Down => (from.X, from.Y+1),
        Direction.Left => (from.X - 1, from.Y),
        Direction.Right => (from.X + 1, from.Y),
        _ => throw new ApplicationException("Unexpected input")
    };
    return new Position(input[y][x],x,y);
}
var upConnectors = new[]{'|', '7', 'F'};
var downConnectors = new[]{'|', 'L', 'J'};
var rightConnectors = new []{'-','7', 'J'};
var leftConnectors = new []{'-', 'L', 'F'}; 
var allowedDirections = new Dictionary<char, Direction[]>{
    {'|',new []{Direction.Up,Direction.Down}},
    {'-',new []{Direction.Left,Direction.Right}},
    {'L',new []{Direction.Up,Direction.Right}},
    {'J',new []{Direction.Up,Direction.Left}},
    {'7',new []{Direction.Down,Direction.Left}},
    {'F',new []{Direction.Down,Direction.Right}},
    {'.',new Direction[]{}},
    {'S', new []{Direction.Down, Direction.Up, Direction.Left, Direction.Right}}
};

Position previous = start;
Direction? [] directions = [
    upConnectors.Contains(input[previous.Y-1][previous.X]) ? Direction.Up : null,
    downConnectors.Contains(input[previous.Y+1][previous.X]) ? Direction.Down : null,
    leftConnectors.Contains(input[previous.Y][previous.X-1]) ? Direction.Left : null,
    rightConnectors.Contains(input[previous.Y][previous.X+1]) ? Direction.Right : null
];
Direction firstMove = (Direction)directions.First(x => x != null)!;
Position current = Move(start, firstMove);
WriteLine($"Begin at: {start}");
int maxDistance = 1;
while(current != start)
{
    var next = allowedDirections[current.Pipe].Select(d => Move(current,d)).Single(x => x != previous);
    previous = current;
    current = next;
    maxDistance++;
}

WriteLine($"Distance travelled: {maxDistance}");
WriteLine($"Furthest away: {maxDistance/2}");

enum Direction {Up, Down, Left, Right}
public record Position(char Pipe, int X, int Y){
    
};