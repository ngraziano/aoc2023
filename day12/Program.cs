
using System.Diagnostics;

static long NbMatchI(Span<char> condition, int firstGroup, IEnumerable<int> conditionGroups, bool wasInGroup)
{
    return NbMatchC(condition, firstGroup, conditionGroups, wasInGroup, new Dictionary<(string, int, int, bool), long>());
}

static long NbMatchC(Span<char> condition, int firstGroup, IEnumerable<int> conditionGroups, bool wasInGroup, Dictionary<(string, int, int, bool), long> cache)
{
    var key = (condition.ToString(), firstGroup, conditionGroups.Aggregate(0, (agg, val) => (agg * 10) + val), wasInGroup);
    if (cache.TryGetValue(key, out var value))
    {
        return value;
    }

    if (condition.Length == 0)
    {
        return (firstGroup == 0 && !conditionGroups.Any()) ? 1 : 0;
    }
    
    var path1 = 0L;
    var currentState = condition[0];
    if (currentState == '.' || currentState == '?')
    {
        if (!wasInGroup)
        {
            path1 = NbMatchC(condition[1..], firstGroup, conditionGroups, false, cache);
        }
        else if (firstGroup > 0)
        {
            path1 = 0;
        }
        else
        {
            path1 = NbMatchC(condition[1..], conditionGroups.FirstOrDefault(0), conditionGroups.Skip(1), false, cache);

        }
    }
    var path2 = 0L;
    if (currentState == '#' || currentState == '?')
    {
        if (firstGroup == 0)
        {
            path2 = 0;
        }
        else
        {
            path2 = NbMatchC(condition[1..], firstGroup - 1, conditionGroups, true, cache);
        }
    }

    var result = path1 + path2;

    cache[key] = result;
    return result;
}

#if true

var sumNb = File.ReadLines("input.txt")
    .Select((line, index) =>
{
    var stopwatch = Stopwatch.StartNew();
    var linePart = line.Split(' ');
    var condition = linePart[0];
    var conditionGroups = linePart[1].Split(",").Select(int.Parse).ToList();


    // naive
    var nbMatch = NbMatchI(condition.ToCharArray(), conditionGroups[0], conditionGroups.Skip(1).ToList(), false);
    stopwatch.Stop();
    // Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
    return nbMatch;
}).Sum();
Console.WriteLine($"Part 1 {sumNb}");

var stopwatch0 = Stopwatch.StartNew();
var sumNbUnfold = File.ReadLines("input.txt")
  //  .AsParallel().AsUnordered()
    .Select((line, index) =>
    {

        var linePart = line.Split(' ');
        var condition0 = linePart[0];
        var conditionGroups0 = linePart[1].Split(",").Select(int.Parse).ToList();

        var condition = condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0;
        var conditionGroups = conditionGroups0.Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).ToList();

        // naive
        var nbMatch = NbMatchI(condition.ToCharArray(), conditionGroups[0], conditionGroups.Skip(1).ToList(), false);

        // Console.WriteLine($"({index:000}) {nbMatch,10}   {line} ");
        return nbMatch;
    }).Sum();
stopwatch0.Stop();
Console.WriteLine($"Part 2 {sumNbUnfold} {stopwatch0.Elapsed}");
#endif



static long NbMatchNew(string chars, List<int> conditionGroups)
{
    if (conditionGroups.Count == 0)
        return chars.Contains('#') ? 0L : 1L;

    chars = chars.TrimStart('.');
    var nbToPut = conditionGroups.Count - 1 + conditionGroups.Sum();
    if (chars.Length < nbToPut)
        return 0L;


    var nbDefect = conditionGroups[0];

    var nbMatch = 0L;
    for (int i = 0; i < chars.Length - (nbToPut - 1); i++)
    {
        // #??#.
        var isValid = chars[i..(i + nbDefect)].All(c => c == '#' || c == '?');

        if (!isValid)
        {
            if (chars[i] == '#')
            {
                break;
            }
            continue;
        }

        if (chars.Length == i + nbDefect)
        {
            nbMatch++;
            continue;
        }

        if (chars.Length == i + nbDefect || chars[i + nbDefect] == '.' || chars[i + nbDefect] == '?')
        {
            nbMatch += NbMatchNew(chars[(i + nbDefect + 1)..], conditionGroups.Skip(1).ToList());
        }

        if (chars[i] == '#')
        {
            break;
        }
    }
    return nbMatch;
}


static long NbMatchNewCached(string chars, List<int> conditionGroups)
{
    return NbMatchNewCachedC(chars, conditionGroups, new Dictionary<(string, int), long>());
}

static long NbMatchNewCachedC(string chars, List<int> conditionGroups, Dictionary<(string, int), long> cache)
{
    if (conditionGroups.Count == 0)
        return chars.Contains('#') ? 0L : 1L;

    chars = chars.TrimStart('.');
    var nbToPut = conditionGroups.Count - 1 + conditionGroups.Sum();
    if (chars.Length < nbToPut)
        return 0L;

    var key = (chars, conditionGroups.Aggregate(0, (acc, val) => (acc * 10) + val));
    if (cache.TryGetValue(key, out var val))
    {
        return val;
    }


    var nbDefect = conditionGroups[0];

    var nbMatch = 0L;
    for (int i = 0; i < chars.Length - (nbToPut - 1); i++)
    {
        // #??#.
        var isValid = chars[i..(i + nbDefect)].All(c => c == '#' || c == '?');

        if (!isValid)
        {
            if (chars[i] == '#')
            {
                break;
            }
            continue;
        }

        if (chars.Length == i + nbDefect)
        {
            nbMatch++;
            continue;
        }

        if (chars.Length == i + nbDefect || chars[i + nbDefect] == '.' || chars[i + nbDefect] == '?')
        {
            nbMatch += NbMatchNewCachedC(chars[(i + nbDefect + 1)..], conditionGroups.Skip(1).ToList(), cache);
        }

        if (chars[i] == '#')
        {
            break;
        }
    }
    cache[key] = nbMatch;
    return nbMatch;
}

var sumNb2 = File.ReadLines("input.txt")
    .Select((line, index) =>
    {
        var stopwatch = Stopwatch.StartNew();
        var linePart = line.Split(' ');
        var condition = linePart[0];
        var conditionGroups = linePart[1].Split(",").Select(int.Parse).ToList();

        var nbMatch = NbMatchNewCached(condition, conditionGroups);

        stopwatch.Stop();
        // Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
        return nbMatch;
    }).Sum();
Console.WriteLine($"Part 1 {sumNb2}");


var stopwatch = Stopwatch.StartNew();
var sumNbUnfoldN = File.ReadLines("input.txt")
    .Select((line, index) =>
    {
        var linePart = line.Split(' ');
        var condition0 = linePart[0];
        var conditionGroups0 = linePart[1].Split(",").Select(int.Parse).ToList();

        var condition = condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0;
        var conditionGroups = conditionGroups0.Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).ToList();


        var nbMatch = NbMatchNewCached(condition, conditionGroups);

        //  Console.WriteLine($"({index:000}) {nbMatch,10} {line} ");
        return nbMatch;
    }).Sum();
stopwatch.Stop();

Console.WriteLine($"Part 2 {sumNbUnfoldN}   {stopwatch.Elapsed}");

// Console.ReadLine();
