
using day5;
using System.Text.RegularExpressions;

IEnumerable<long> CreateLongRange(long start, long count)
{

    return new LongRange(start, count);
}

var lineEnumerator = File.ReadLines("input.txt").GetEnumerator();

lineEnumerator.MoveNext();
var seedLine = lineEnumerator.Current;
var seedsId = seedLine.Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();



// empty line
lineEnumerator.MoveNext();

var mapRegex = new Regex("(.*)-to-(.*) map:");

var mapperList = new List<Mapper>();
while (lineEnumerator.MoveNext())
{
    var mapNameLine = lineEnumerator.Current;
    var match = mapRegex.Match(mapNameLine);
    if (!match.Success) throw new InvalidDataException("Fail to parse");
    var currentMapper = new Mapper(match.Groups[1].Value, match.Groups[2].Value);
    mapperList.Add(currentMapper);

    while (lineEnumerator.MoveNext() && !string.IsNullOrWhiteSpace(lineEnumerator.Current))
    {
        var mapLineValues = lineEnumerator.Current.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        currentMapper.AddMapping(mapLineValues[0], mapLineValues[1], mapLineValues[2]);
    }
}

var minLocation = seedsId.Select(value =>
{
    foreach (var mapper in mapperList)
    {
        value = mapper.Map(value);
    }
    return value;
}).Min();

Console.WriteLine($"Part 1 Min location = {minLocation}");

#if BRUTE_FORCE

var seedInterval = seedsId
    .Select((value, index) => (value, index))
    .GroupBy(e => e.index / 2)
    .Select(e => new { start = e.First().value, length = e.Last().value });

var bruteForceMin = seedInterval.AsParallel().AsUnordered().SelectMany(i => CreateLongRange(i.start, i.length)).Min(origValue =>
{
    var value = origValue;
    foreach (var mapper in mapperList)
    {
        value = mapper.Map(value);
    }

    if (origValue % 1000000 == 0)
    {
        Console.WriteLine($"{origValue} =>{value}");
    };
    return value;
});


Console.WriteLine($"Part 2 Min location = {bruteForceMin}");
#else

var dataIntervals = seedsId
    .Select((value, index) => (value, index))
    .GroupBy(e => e.index / 2)
    .Select(e => new Interval { Start = e.First().value, End = e.First().value + e.Last().value - 1 })
    .OrderBy(i => i.Start)
    .ToList();

foreach (var mapper in mapperList)
{

    Console.WriteLine($"===============  {mapper.Source} =>  {mapper.Destination} ===================");
    dataIntervals = dataIntervals.SelectMany(interval =>
    {
        Console.Write($"[{interval.Start} .. {interval.End}] =>");

        var newinterval = mapper.Map(interval).ToList();
        foreach (var i in newinterval)
        {
            Console.Write($"[{i.Start} .. {i.End}] ");
        }
        Console.WriteLine();
        return newinterval;
    }).OrderBy(i => i.Start).ToList();
}

var minValue = dataIntervals.Min(inteval => inteval.Start);

Console.WriteLine($"Part 2 Min location = {minValue}");

#endif




