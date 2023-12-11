


var file = File.ReadLines("input.txt").ToList();

var emptyLine = file.Select((l, index) => (l, index)).Where(e => e.l.All(c => c == '.')).Select(e => e.index).ToList();
var emptyColums = new List<int>();

for (int j = 0; j < file[0].Length; j++)
{
    bool isEmpty = true;
    for (int i = 0; i < file.Count; i++)
    {
        if (file[i][j] != '.')
        {
            isEmpty = false; break;
        }
    }
    if (isEmpty)
    {
        emptyColums.Add(j);
    }
}

List<(int i, int j)> galaxy = [];
for (int i = 0; i < file.Count; i++)
{
    for (int j = 0; j < file[i].Length; j++)
    {
        if (file[i][j] == '#')
        {
            galaxy.Add((i, j));
        }
    }
}


var sumPart1 = galaxy.SelectMany(g1 =>
{
    return galaxy.Select(g2 =>
    {
        if (g1 == g2)
        {
            return 0;
        }

        var maxGi = int.Max(g1.i, g2.i);
        var minGi = int.Min(g1.i, g2.i);

        int distance = maxGi - minGi;
        distance += emptyLine.Count(i => i > minGi && i < maxGi);

        var maxGj = int.Max(g1.j, g2.j);
        var mjnGj = int.Min(g1.j, g2.j);

        distance += maxGj - mjnGj;
        distance += emptyColums.Count(j => j > mjnGj && j < maxGj);
        return distance;
    });
}).Sum() / 2;

Console.WriteLine($"Part 1 {sumPart1}");


var sumPart2 = galaxy.SelectMany(g1 =>
{
    return galaxy.Select(g2 =>
    {
        if (g1 == g2)
        {
            return 0;
        }

        var maxGi = int.Max(g1.i, g2.i);
        var minGi = int.Min(g1.i, g2.i);

        long distance = maxGi - minGi;
        distance += emptyLine.Count(i => i > minGi && i < maxGi) * (1000000 - 1);

        var maxGj = int.Max(g1.j, g2.j);
        var mjnGj = int.Min(g1.j, g2.j);

        distance += maxGj - mjnGj;
        distance += emptyColums.Count(j => j > mjnGj && j < maxGj) * (1000000 - 1);
        return distance;
    });
}).Sum() / 2;

Console.WriteLine($"Part 2 {sumPart2}");