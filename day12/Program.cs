
using day12;
using System;
using System.Diagnostics;
using System.Reflection;

static long NbMatch(Span<char> condition, int firstGroup, IEnumerable<int> conditionGroups, bool wasInGroup, Stats stats)
{
    if (!stats.CheckStat())
    {
        return 0;
    }

    if (condition.Length == 0)
    {
        return (!conditionGroups.Any() || (conditionGroups.Last() == 0)) ? 1 : 0;
    }

    var path1 = 0L;
    var currentState = condition[0];
    if (currentState == '.' || currentState == '?')
    {
        if (!wasInGroup)
        {
            path1 = NbMatch(condition[1..], firstGroup, conditionGroups, false, currentState == '.' ? stats : stats.DecPassed());
        }
        else if (firstGroup > 0)
        {
            path1 = 0;
        }
        else
        {
            path1 = NbMatch(condition[1..], conditionGroups.FirstOrDefault(0), conditionGroups.Skip(1), false, currentState == '.' ? stats : stats.DecPassed());

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
            path2 = NbMatch(condition[1..], firstGroup - 1, conditionGroups, true, currentState == '#' ? stats : stats.DecFailed());

        }
    }
    return path1 + path2;
}

#if METHOD_NAIVE_TOO_SLOW

var sumNb = File.ReadLines("input.txt")
    .Select((line,index) =>
{
    var stopwatch = Stopwatch.StartNew();
    var linePart = line.Split(' ');
    var condition = linePart[0];
    var conditionGroups = linePart[1].Split(",").Select(int.Parse).ToList();

    var stat = Stats.CalculateStats(condition.ToCharArray(), conditionGroups);
    if (!stat.CheckStat())
        throw new InvalidOperationException();
    // naive
    var nbMatch = NbMatch(condition.ToCharArray(), conditionGroups[0], conditionGroups.Skip(1).ToList(), false, stat);
    stopwatch.Stop();
    Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
    return nbMatch;
}).Sum();
Console.WriteLine($"Part 1 {sumNb}");

var sumNbUnfold = File.ReadLines("input.txt")
    .AsParallel().AsUnordered()
    .Select((line, index) =>
    {
        var stopwatch = Stopwatch.StartNew();
        var linePart = line.Split(' ');
        var condition0 = linePart[0];
        var conditionGroups0 = linePart[1].Split(",").Select(int.Parse).ToList();

        var condition = condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0;
        var conditionGroups = conditionGroups0.Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).ToList();


        var stat = Stats.CalculateStats(condition.ToCharArray(), conditionGroups);
        if (!stat.CheckStat())
            throw new InvalidOperationException();

        // naive
        var nbMatch = NbMatch(condition.ToCharArray(), conditionGroups[0], conditionGroups.Skip(1).ToList(), false, stat);

        stopwatch.Stop();
        Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
        return nbMatch;
    }).Sum();
Console.WriteLine($"Part 2 {sumNbUnfold}");
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
        Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
        return nbMatch;
    }).Sum();
Console.WriteLine($"Part 1 {sumNb2}");


var sumNbUnfold = File.ReadLines("input.txt")
    .AsParallel().AsUnordered()
    .Select((line, index) =>
    {
        var linePart = line.Split(' ');
        var condition0 = linePart[0];
        var conditionGroups0 = linePart[1].Split(",").Select(int.Parse).ToList();

        var condition = condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0 + '?' + condition0;
        var conditionGroups = conditionGroups0.Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).Concat(conditionGroups0).ToList();

        var stopwatch = Stopwatch.StartNew();
        var nbMatch = NbMatchNewCached(condition, conditionGroups);
        stopwatch.Stop();
        Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed} {line} ");
        return nbMatch;
    }).Sum();
Console.WriteLine($"Part 2 {sumNbUnfold}");

// Console.ReadLine();
