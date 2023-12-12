
using day12;
using System.Collections.Concurrent;
using System.Diagnostics;

static long NbMatch(Span<char> condition, IEnumerable<int> conditionGroups, bool wasInGroup, Stats stats)
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
            path1 = NbMatch(condition[1..], conditionGroups, false, currentState == '.' ? stats : stats.DecPassed());
        }
        else if (conditionGroups.FirstOrDefault(1) > 0)
        {
            path1 = 0;
        }
        else
        {
            path1 = NbMatch(condition[1..], conditionGroups.Skip(1).ToList(), false, currentState == '.' ? stats : stats.DecPassed());

        }
    }
    var path2 = 0L;
    if (currentState == '#' || currentState == '?')
    {
        if (conditionGroups.FirstOrDefault(0) == 0)
        {
            path2 = 0;
        }
        else
        {
            path2 = NbMatch(condition[1..], conditionGroups.Skip(1).Prepend(conditionGroups.First() - 1).ToList(), true, currentState == '#' ? stats : stats.DecFailed());

        }
    }
    return path1 + path2;
}


var sumNb = File.ReadLines("input.txt")
    .Select(line =>
{
    var linePart = line.Split(' ');
    var condition = linePart[0];
    var conditionGroups = linePart[1].Split(",").Select(int.Parse).ToList();

    var stat = Stats.CalculateStats(condition.ToCharArray(), conditionGroups);
    if (!stat.CheckStat())
        throw new InvalidOperationException();
    // naive
    var nbMatch = NbMatch(condition.ToCharArray(), conditionGroups, false, stat);
    Console.WriteLine($"{line} \t\t{nbMatch}");
    return nbMatch;
}).Sum();


Console.WriteLine($"Part 1 {sumNb}");


var sumNbUnfold = File.ReadLines("input.txt")
    .AsParallel().WithDegreeOfParallelism(24).AsUnordered()
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
        var nbMatch = NbMatch(condition.ToCharArray(), conditionGroups, false, stat);

        stopwatch.Stop();
        Console.WriteLine($"({index:000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
        return nbMatch;
    }).Sum();


Console.WriteLine($"Part 2 {sumNbUnfold}");
