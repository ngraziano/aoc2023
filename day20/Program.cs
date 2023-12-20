


using System.Numerics;

var configuration = File.ReadAllLines("input.txt").Select(line =>
{
    var (currentName, destNames) = line.Split(" -> ") switch
    {
        [var c, var d] => (c, d),
        _ => throw new InvalidOperationException(),
    };

    var dest = destNames.Split(",").Select(destNames => destNames.Trim()).ToList();

    return (INode)(currentName switch
    {
        "broadcaster" => new BroadCaster() { Name = "broadcaster", Dest = dest },
        ['%', .. var name] => new FlipFlop() { Name = name, Dest = dest },
        ['&', .. var name] => new Conjunction { Name = name, Dest = dest },
        _ => throw new InvalidOperationException(),
    });
}).ToDictionary(e => e.Name);


foreach (var node in configuration.Values)
{
    foreach (var dest in node.Dest)
    {
        if (configuration.ContainsKey(dest))
        {
            configuration[dest].AddSource(node.Name);
        }
    }
}

int nbLow = 0;
int nbHigh = 0;


for (int i = 0; i < 1000; i++)
{
    Queue<(string source, bool pulse, string dest)> cmds = [];
    cmds.Enqueue(("button", false, "broadcaster"));

    while (cmds.Count > 0)
    {
        var (source, pulse, dest) = cmds.Dequeue();

        if (pulse)
        {
            nbHigh++;
        }
        else
        {
            nbLow++;
        }
        if (configuration.TryGetValue(dest, out INode? value))
        {
            foreach (var newCmd in value.Pulse(pulse, source))
            {

                cmds.Enqueue(newCmd);
            }
        }

    }
}

Console.WriteLine($"L {nbLow} H {nbHigh} : L*H {nbLow * nbHigh}");


foreach (var node in configuration.Values)
{
    node.Reset();
}

Console.WriteLine("==================== MERMAID CHART ==================");
Console.WriteLine("flowchart LR");
foreach (var conf in configuration.Values)
{
    Console.WriteLine($"  {conf.Name}{conf.ChartName} --> {string.Join(" & ", conf.Dest)}");
}
Console.WriteLine("==================== MERMAID CHART ==================");


string lastConjunction = configuration.Values.Single(n => n.Dest.Contains("rx")).Name;

var watched = configuration.Values.Where(n => n.Dest.Contains(lastConjunction)).Select(n => n.Name).ToList();
List<long> cycles = [];

/*
long dh = 3877;
long bb = 3907;
long qd = 4001;
long dp = 4027;

Console.WriteLine($"Part 2 ?> {dh * bb * qd * dp}");
*/

int buttonPress = 0;
var allFound = false;
while (!allFound)
{
    buttonPress++;

    Queue<(string source, bool pulse, string dest)> cmds = [];
    cmds.Enqueue(("button", false, "broadcaster"));

    while (cmds.Count > 0)
    {
        var (source, pulse, dest) = cmds.Dequeue();

        if (pulse)
        {
            nbHigh++;
        }
        else
        {
            nbLow++;
        }
        if (!pulse && watched.Contains(dest))
        {
            cycles.Add(buttonPress);
            watched.Remove(dest);
            allFound = watched.Count == 0;
        }

        if (configuration.TryGetValue(dest, out INode? value))
        {
            foreach (var newCmd in value.Pulse(pulse, source))
            {
                cmds.Enqueue(newCmd);
            }
        }

    }
}

var result2 = cycles.Aggregate(new BigInteger(1), (acc, val) => acc / BigInteger.GreatestCommonDivisor(acc, val) * val);

Console.WriteLine($"Part 2 nb press {result2}");


interface INode
{
    public string Name { get; }
    public List<string> Dest { get; }

    public void AddSource(string name);
    IEnumerable<(string source, bool pulse, string dest)> Pulse(bool pulse, string source);

    void Reset();

    public string ChartName { get; }
}


class BroadCaster : INode
{
    public required string Name { get; init; }
    public required List<string> Dest { get; init; }

    public string ChartName => $"[{Name}]";

    public void AddSource(string name)
    {
        throw new InvalidOperationException();
    }

    public IEnumerable<(string source, bool pulse, string dest)> Pulse(bool pulse, string source)
    {
        return Dest.Select(d => (Name, pulse, d));
    }

    public void Reset()
    {
    }
}

class FlipFlop : INode
{
    public required string Name { get; init; }
    public required List<string> Dest { get; init; }

    public string ChartName => $"{{{Name}}}";

    public void AddSource(string name)
    {
    }

    public IEnumerable<(string source, bool pulse, string dest)> Pulse(bool pulse, string source)
    {
        if (pulse)
        {
            return [];
        }

        state = !state;
        return Dest.Select(d => (Name, state, d));
    }
    public void Reset()
    {
        state = false;
    }

    bool state = false;
}

class Conjunction : INode
{
    public required string Name { get; init; }
    public required List<string> Dest { get; init; }

    public string ChartName => $"({Name})";

    public void AddSource(string name)
    {
        sourcesState.Add(name, false);
    }

    public IEnumerable<(string source, bool pulse, string dest)> Pulse(bool pulse, string source)
    {
        sourcesState[source] = pulse;
        var output = !sourcesState.Values.All(v => v);
        return Dest.Select(d => (Name, output, d));
    }
    public void Reset()
    {
        foreach (var k in sourcesState.Keys)
        {
            sourcesState[k] = false;
        }
    }

    private readonly Dictionary<string, bool> sourcesState = [];
}