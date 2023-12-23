using P3 = (int X, int Y, int Z);


List<(P3 Start, P3 End)> bricks = File.ReadLines("input.txt").Select(l =>
{

    return l.Split('~') switch
    {
        [var c1, var c2] => (ToPoint(c1), ToPoint(c2)),
        _ => throw new InvalidOperationException()
    };

})
    .Select((P3 Start, P3 End) (b) => ((Math.Min(b.Item1.X, b.Item2.X), Math.Min(b.Item1.Y, b.Item2.Y), Math.Min(b.Item1.Z, b.Item2.Z)),
    (Math.Max(b.Item1.X, b.Item2.X), Math.Max(b.Item1.Y, b.Item2.Y), Math.Max(b.Item1.Z, b.Item2.Z))))
    .OrderBy(b => b.Start.Z)
    .ToList();

Pack(bricks);
Console.WriteLine("Pack Done");

int nbRemove = 0;

for (int i = 0; i < bricks.Count; i++)
{
    List<(P3 Start, P3 End)> simulBrick = [.. bricks[..i], .. bricks[(i + 1)..]];

    var canGoDown = false;
    for (int j = i; j < simulBrick.Count; j++)
    {
        if (CanGoDown(simulBrick, j))
        {
            canGoDown = true;
            break;
        }
    }
    if (!canGoDown)
    {
        nbRemove++;
    }
}
Console.WriteLine($"Nb Can go down {nbRemove}");

int sumMoved = 0;
for (int i = 0; i < bricks.Count; i++)
{
    var simulBrick = bricks.Where((_, idx) => idx != i).ToList();
    sumMoved += Pack(simulBrick, i);
}
Console.WriteLine($"Part 2 {sumMoved}");

int Pack(List<(P3 Start, P3 End)> bricks, int packFrom = 1)
{
    bool modificationDone = true;
    List<int> brickMoved = [];
    int firstMoved = packFrom;
    while (modificationDone)
    {
        modificationDone = false;
        for (int i = firstMoved; i < bricks.Count; i++)
        {
            while (CanGoDown(bricks, i))
            {
                var (Start, End) = bricks[i];
                bricks[i] = ((Start.X, Start.Y, Start.Z - 1), (End.X, End.Y, End.Z - 1));
                brickMoved.Add(i);
                if (!modificationDone)
                {
                    firstMoved = i;
                    modificationDone = true;
                }
            }
        }
    }
    return brickMoved.Distinct().Count();
}

bool CanGoDown(List<(P3 Start, P3 End)> bricks, int idx)
{

    var b = bricks[idx];
    if (b.Start.Z == 1)
    {
        // floor
        return false;
    }

    (int S, int E) x = (b.Start.X, b.End.X);
    (int S, int E) y = (b.Start.Y, b.End.Y);

    for (int i = idx - 1; i >= 0; i--)
    {
        var bUnder = bricks[i];

        if (bUnder.End.Z != b.Start.Z - 1)
            continue;
        (int S, int E) xUnder = (bUnder.Start.X, bUnder.End.X);
        (int S, int E) yUnder = (bUnder.Start.Y, bUnder.End.Y);

        if (x.S <= xUnder.E && xUnder.S <= x.E && y.S <= yUnder.E && yUnder.S <= y.E)
            return false;
    }
    return true;
}

P3 ToPoint(string c) => c.Split(",") switch
{
    [var sx, var sy, var sz] => (int.Parse(sx), int.Parse(sy), int.Parse(sz)),
    _ => throw new ArgumentException("Invalid data", nameof(c)),
};
