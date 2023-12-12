﻿
using System.Diagnostics;
using System.Linq;

int NbMatch(Span<char> condition, IEnumerable<int> conditionGroups, char previousState)
{
    if (condition.Length == 0)
    {
        return (!conditionGroups.Any() || (conditionGroups.Last() == 0)) ? 1 : 0;
    }

    var path1 = 0;
    if (condition[0] == '.' || condition[0] == '?')
    {
        if (previousState == '.' || previousState == ' ')
        {
            path1 = NbMatch(condition[1..], conditionGroups, '.');
        }
        else if (conditionGroups.FirstOrDefault(1) > 0)
        {
            path1 = 0;
        }
        else
        {
            path1 = NbMatch(condition[1..], conditionGroups.Skip(1).ToList(), '.');

        }
        // Console.WriteLine($"/// =>.{condition[1..]} {string.Join(',', conditionGroups)}\t\t{path1}");

    }
    var path2 = 0;
    if (condition[0] == '#' || condition[0] == '?')
    {
        if (conditionGroups.FirstOrDefault(0) == 0)
        {
            path2 = 0;
        }
        else
        {
            path2 = NbMatch(condition[1..], conditionGroups.Skip(1).Prepend(conditionGroups.First() - 1).ToList(), '#');

        }
        // Console.WriteLine($"/// =>#{condition[1..]} {string.Join(',', conditionGroups)}\t\t{path2}");

    }

    // Console.WriteLine($"PPP =>{condition} {string.Join(',', conditionGroups)}\t\t{path1 + path2}");

    return path1 + path2;
}


var sumNb = File.ReadLines("input.txt")
    .Select(line =>
{
    var linePart = line.Split(' ');
    var condition = linePart[0];
    var conditionGroups = linePart[1].Split(",").Select(int.Parse).ToList();

    // naive
    var nbMatch = NbMatch(condition.ToCharArray(), conditionGroups, ' ');
    Console.WriteLine($"{line} \t\t{nbMatch}");
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

        // naive
        var nbMatch = NbMatch(condition.ToCharArray(), conditionGroups, ' ');

        stopwatch.Stop();
        Console.WriteLine($"({index:0000}) {nbMatch,10} {stopwatch.Elapsed}  {line} ");
        return nbMatch;
    }).Sum();


Console.WriteLine($"Part 2 {sumNbUnfold}");