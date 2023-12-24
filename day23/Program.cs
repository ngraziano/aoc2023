using P2 = (int X, int Y);

var board = File.ReadLines("input.txt").ToList();

P2 maxBoard = (board[0].Length - 1, board.Count - 1);

P2 start = (1, 0);
P2 end = (maxBoard.X - 1, maxBoard.Y);

Stack<(P2 pt, List<P2> past, int length)> pathSearch = [];
pathSearch.Push((start, [], 0));

int solution = 0;
Dictionary<P2, List<(P2 dest, int length)>> singlePath =
    board.SelectMany((l, y) => l.Select((c, x) => (x, y, c))).Where(e => e.c != '#').Select(e =>
        ((e.x, e.y), NextStep((e.x, e.y), []).Select(nexStep =>
        {
            List<P2> past = [(e.x, e.y)];
            List<P2> list = [nexStep];
            while (list.Count == 1)
            {
                past.Add(list[0]);
                list = NextStep(list[0], past).ToList();
            }
            return (past[^1], past.Count - 1);
        }).ToList())
    ).ToDictionary(e => e.Item1, e => e.Item2);

Console.WriteLine("Cache path done");

while (pathSearch.Count > 0)
{
    var (pt, past, length) = pathSearch.Pop();

    if (pt == end)
    {
        solution = Math.Max(solution, length);
        continue;
    }

    if (singlePath.TryGetValue(pt, out var calculatedPast))
    {
        foreach (var interval in calculatedPast)
        {
            var newPos = interval.dest;
            if (!past.Contains(newPos))
            {
                pathSearch.Push((newPos, [.. past, newPos], length + interval.length));
            }
        }
    }
    else
    {
        foreach (var p in NextStep(pt, past))
        {
            pathSearch.Push((p, past.Append(pt).ToList(), length + 1));
        }
    }
}

Console.WriteLine($"Part 1 {solution}");
// solution part 2 is > solution part 1 do not reset solution

singlePath =
    board.SelectMany((l, y) => l.Select((c, x) => (x, y, c))).Where(e => e.c != '#').Select(e =>
        ((e.x, e.y), NextStepP2((e.x, e.y), []).Select(nexStep =>
        {
            List<P2> past = [(e.x, e.y)];
            List<P2> list = [nexStep];
            while (list.Count == 1)
            {
                past.Add(list[0]);
                list = NextStepP2(list[0], past).ToList();
            }
            return (past[^1], past.Count - 1);
        }).ToList())
    ).ToDictionary(e => e.Item1, e => e.Item2);

Console.WriteLine("Cache path done");


pathSearch.Push((start, [], 0));
int i = 0;
var minCount = int.MaxValue;
while (pathSearch.Count > 0)
{

    minCount = Math.Min(minCount, pathSearch.Count);
    if (++i % 100000 == 0)
    {
        Console.WriteLine($"({i,10}) To search {pathSearch.Count,4} min({minCount,3}), actual score {solution} : {string.Join(",", pathSearch.TakeLast(5).Select(t => $"{t.pt}").Reverse())}");
        minCount = int.MaxValue;
    }

    var (pt, past, length) = pathSearch.Pop();
    if (pt == end)
    {
        solution = Math.Max(solution, length);
        continue;
    }

    foreach (var (newPos, newLenth) in singlePath[pt])
    {
        if (!past.Contains(newPos))
        {
            pathSearch.Push((newPos, past.Append(newPos).ToList(), length + newLenth));
        }
    }
}

Console.WriteLine($"Part 2 {solution}");
IEnumerable<P2> NextStep(P2 pt, List<P2> past)
{
    return board![pt.Y][pt.X] switch
    {
        '.' => PtIfValid((pt.X + 1, pt.Y), past)
               .Concat(PtIfValid((pt.X - 1, pt.Y), past))
               .Concat(PtIfValid((pt.X, pt.Y + 1), past))
               .Concat(PtIfValid((pt.X, pt.Y - 1), past)),
        '>' => PtIfValid((pt.X + 1, pt.Y), past),
        '<' => PtIfValid((pt.X - 1, pt.Y), past),
        '^' => PtIfValid((pt.X, pt.Y - 1), past),
        'v' => PtIfValid((pt.X, pt.Y + 1), past),
        _ => throw new InvalidOperationException(),
    };
}

IEnumerable<P2> NextStepP2(P2 pt, List<P2> past)
{
    return PtIfValid((pt.X + 1, pt.Y), past)
               .Concat(PtIfValid((pt.X - 1, pt.Y), past))
               .Concat(PtIfValid((pt.X, pt.Y + 1), past))
               .Concat(PtIfValid((pt.X, pt.Y - 1), past));

}
IEnumerable<P2> PtIfValid(P2 pt, IEnumerable<P2> past)
{
    if (pt.X >= 0 && pt.Y >= 0 && pt.X <= maxBoard.X && pt.Y <= maxBoard.Y
        && board![pt.Y][pt.X] != '#' && !past.Contains(pt))
    {
        yield return pt;
    }
}


