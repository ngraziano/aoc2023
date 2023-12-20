
var lineEnumerator = File.ReadLines("input.txt").GetEnumerator();

var rules = new Dictionary<string, List<((int var, Func<int, int, bool> o, int val), string nextRule)>>();

bool OpInf(int a, int b) => a < b;
bool OpSup(int a, int b) => a > b;
// XMAS

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
                [var rule] => ((0, OpSup, 0), rule),
                [var op, var rule] => (OperationDecomp(op), rule),
                _ => throw new InvalidOperationException(),
            };
        }
        ).ToList();
}

var stack = new Stack<Intervals>();

stack.Push(new Intervals([(1, 4000), (1, 4000), (1, 4000), (1, 4000)], "in"));

Int128 total = 0;
while (stack.Count > 0)
{
    var toHandle = stack.Pop();

    if (toHandle.NextRule == "R")
        continue;

    if (toHandle.NextRule == "A")
    {
        total += toHandle.Values.Select(c => c.max - c.min + 1).Aggregate((Int128)1, (agg, val) => agg * val);

        continue;
    }

    foreach (var r in rules[toHandle.NextRule])
    {
        var (var, op, val) = r.Item1;
        var testmin = op(toHandle.Values[var].min, val);
        var testmax = op(toHandle.Values[var].max, val);

        if (testmin && testmax)
        {
            stack.Push(new Intervals(toHandle.Values, r.nextRule));
            break;
        }
        else if (testmin)
        {
            var aarr = toHandle.Values.Clone() as (int min, int max)[];
            aarr![var] = (aarr[var].min, val - 1);
            stack.Push(new Intervals(aarr, r.nextRule));
            toHandle.Values[var] = (val, toHandle.Values[var].max);
        }
        else if (testmax)
        {
            var aarr = toHandle.Values.Clone() as (int min, int max)[];
            aarr![var] = (val + 1, aarr[var].max);
            stack.Push(new Intervals(aarr, r.nextRule));
            toHandle.Values[var] = (toHandle.Values[var].min, val);
        }
    }

}


Console.WriteLine($"Part 2 {total}");


(int var, Func<int, int, bool> o, int val) OperationDecomp(string operation)
{
    Func<int, int, bool> o = operation.Contains('>') ? OpSup : OpInf;

    var (var, val) = operation.Split(['>', '<']) switch
    {
        ["x", var value] => (0, int.Parse(value)),
        ["m", var value] => (1, int.Parse(value)),
        ["a", var value] => (2, int.Parse(value)),
        ["s", var value] => (3, int.Parse(value)),
        _ => throw new InvalidOperationException(),
    };
    return (var, o, val);
}

record Intervals((int min, int max)[] Values, string NextRule);
