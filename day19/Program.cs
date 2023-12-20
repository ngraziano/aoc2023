

using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

var lineEnumerator = File.ReadLines("input.txt").GetEnumerator();

var rules = new Dictionary<string, List<(Predicate<Part> p, string nextRule)>>();
var breakX = new HashSet<int>() { 4000 };
var breakM = new HashSet<int>() { 4000 };
var breakA = new HashSet<int>() { 4000 };
var breakS = new HashSet<int>() { 4000 };
while (lineEnumerator.MoveNext() && !string.IsNullOrEmpty(lineEnumerator.Current))
{
    var line = lineEnumerator.Current;
    var (name, ruleString) = line.Split('{') switch
    {
        [var n, var rest] => (n, rest[..^1]),
        _ => throw new InvalidOperationException(),
    };

    rules[name] = ruleString.Split(',').Select(
        p =>
        {
            return p.Split(':') switch
            {
                [var rule] => (_ => true, rule),
                [var op, var rule] => (OperationToLambda(op), rule),
                _ => throw new InvalidOperationException(),
            };
        }

        ).ToList();
}

var parser = new Regex(@"\{x=(\d*),m=(\d*),a=(\d*),s=(\d*)\}");


long totalSum = 0;
while (lineEnumerator.MoveNext())
{
    var line = lineEnumerator.Current;
    var lineMatch = parser.Match(line);

    var part = new Part()
    {
        X = int.Parse(lineMatch.Groups[1].Value),
        M = int.Parse(lineMatch.Groups[2].Value),
        A = int.Parse(lineMatch.Groups[3].Value),
        S = int.Parse(lineMatch.Groups[4].Value),
        Weight = int.Parse(lineMatch.Groups[1].Value) + int.Parse(lineMatch.Groups[2].Value) + int.Parse(lineMatch.Groups[3].Value) + int.Parse(lineMatch.Groups[4].Value),
    };


    var currentRule = "in";

    while (currentRule != "A" && currentRule != "R")
    {
        currentRule = rules[currentRule].First(d => d.p(part)).nextRule;
    }

    if (currentRule == "A")
    {
        totalSum += part.Weight;
    }
}

Console.WriteLine($"Part 1 {totalSum}");
/*if (totalCombinaison % 10000000 == 0)
{
    Console.WriteLine($"State {totalAccepted} / {totalCombinaison} {max}");
}*/


long totalCombi = ((long)breakX.Count) * breakM.Count * breakA.Count * breakS.Count;

var breakXL = breakX.Order().ToList();
var breakML = breakM.Order().ToList();
var breakAL = breakA.Order().ToList();
var breakSL = breakS.Order().ToList();
long nbTested = 0L;

var time = Stopwatch.StartNew();
Int128 totalAccepted =
    ListWithPrev(breakXL).SelectMany(
        x => ListWithPrev(breakML).SelectMany(m =>
            ListWithPrev(breakAL).SelectMany(a =>
                ListWithPrev(breakSL).Select(s =>
                    new Part(x.v, m.v, a.v, s.v, x.diffpref * m.diffpref * a.diffpref * s.diffpref)

        ))))
    .AsParallel().AsUnordered()

        .Select(part =>
{
    nbTested++;
    const int interv = 10000000;
    if (nbTested % interv == 0)
    {
        Console.WriteLine($"{time.Elapsed} NbTested {nbTested / interv} / {totalCombi / interv}");
    }
    var currentRule = "in";

    while (currentRule != "A" && currentRule != "R")
    {
        currentRule = rules[currentRule].First(d => d.p(part)).nextRule;
    }
    return (currentRule == "A") ? part.Weight : 0L;
}).Sum();  // .Aggregate(BigInteger.Zero, (acc, newVal) => acc + newVal);

time.Stop();
Console.WriteLine($"Total combinaison accepted {totalAccepted}");


IEnumerable<(int diffpref, int v)> ListWithPrev(IEnumerable<int> t)
{
    int prev = 0;
    foreach (int v in t)
    {
        yield return (v - prev, v);
        prev = v;
    }
}


Predicate<Part> OperationToLambda(string op)
{
    bool isgreater = op.Contains('>');


    var temp = op.Split(['>', '<']) switch
    {
        [var var, var value] => (var, int.Parse(value)),
        _ => throw new InvalidOperationException(),


    };

    var hashToUpdate = temp.var switch
    {
        "x" => breakX,
        "m" => breakM,
        "a" => breakA,
        "s" => breakS,
        _ => throw new InvalidOperationException(),
    };

    // no thinking with isgreater ...
    if (isgreater)
    {
        hashToUpdate.Add(temp.Item2 - 1);
    }
    hashToUpdate.Add(temp.Item2);
    if (!isgreater)
    {
        hashToUpdate.Add(temp.Item2 + 1);
    }

    return temp switch
    {
        ("x", var var) => isgreater ? e => e.X > var : e => e.X < var,
        ("m", var var) => isgreater ? e => e.M > var : e => e.M < var,
        ("a", var var) => isgreater ? e => e.A > var : e => e.A < var,
        ("s", var var) => isgreater ? e => e.S > var : e => e.S < var,
        _ => throw new InvalidOperationException(),
    };
}

record struct Part(int X, int M, int A, int S, long Weight);