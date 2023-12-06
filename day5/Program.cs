using System.ComponentModel.DataAnnotations;

Map ReadMapNamed(string name, string[] lines)
{
    var maps = lines.SkipWhile(x => !x.StartsWith(name)).Skip(1).TakeWhile(x => !String.IsNullOrWhiteSpace(x));
    return new Map(name, maps.Select(MapLine.Parse).OrderBy(x => x.Source).ToArray());
}
long MapThroughAll(Map[] chain, long value)
{
    foreach (var map in chain)
    {
        value = map.MapValue(value);
    }

    return value;
}

IEnumerable<LongRange> RangesFromLine(string line)
{
    var numbers = line.Split(' ')[1..].Select(x => x.ToLong()).ToArray();
    for (int i = 0; i < numbers.Length; i += 2)
        yield return new LongRange(numbers[i], numbers[i + 1]);
}

long LowestLocation(Map[] chain, LongRange range)
{
    long lowest = long.MaxValue;
    foreach (long value in range.Values())
    {
        var loc = MapThroughAll(chain, value);
        if (loc < lowest)
            lowest = loc;
    }

    return lowest;
}

var input = File.ReadAllLines(args.FirstOrDefault() ?? "input.txt");
var seeds = input[0].Split(' ').Skip(1).Select(x => x.ToLong()).ToArray();
var seedToSoil = ReadMapNamed("seed-to-soil", input);
var soilToFertilizer = ReadMapNamed("soil-to-fertilizer", input);
var fertilizerToWater = ReadMapNamed("fertilizer-to-water", input);
var waterToLight = ReadMapNamed("water-to-light", input);
var lightToTemperature = ReadMapNamed("light-to-temperature", input);
var temperatureToHumidity = ReadMapNamed("temperature-to-humidity", input);
var humidityToLocation = ReadMapNamed("humidity-to-location", input);

var maps = new[]
{
    seedToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity,
    humidityToLocation
};

/* Part 1 */ 
var locations = seeds.Select(s => new {Seed = s, Location = MapThroughAll(maps, s)})
    .OrderBy(x => x.Location)
    .ToArray();
WriteLine("Part 1"); 
WriteLine($"Answer: {locations.First().Location}");

WriteLine("\nPart 2");
var ranges = RangesFromLine(input[0]).ToArray();
var rangeLocations = ranges.Select(r => new {Range = r, Location = LowestLocation(maps, r)})
    .OrderBy(x => x.Location).ToArray();
ranges.DumpConsole();
WriteLine($"Answer: {rangeLocations.First().Location}");
record MapLine(long Destination, long Source, long Length)
{
    public long End => Source + Length;

    public long MapValue(long value)
    {
        long offset = value - Source;
        if (offset < 0)
            throw new ApplicationException("Wrong line picked");
        return Destination + offset;
    }
    public static MapLine Parse(string data)
    {
        var parts = data.Split(' ').Select(x => x.ToLong()).ToArray();
        return new MapLine(parts[0], parts[1], parts[2]);
    }
};

record Map(string Name, MapLine[] Mappings)
{
    public long MapValue(long value)
    {
        var lineToUse = Mappings.FirstOrDefault(x =>value >= x.Source && value < x.End);
        return lineToUse?.MapValue(value) ?? value;
    }
    public override string ToString()
    {
        return String.Join(Environment.NewLine, Mappings.Select(x => x.ToString()).Prepend(Name));
    }
}

record LongRange(long Begin, long Length)
{
    public IEnumerable<long> Values()
    {
        for (long n = Begin; n < Begin + Length; n++)
            yield return n;
    }
};