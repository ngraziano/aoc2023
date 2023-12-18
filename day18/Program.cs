

using System.Globalization;

var instruction = File.ReadLines("input.txt").Select(line =>
{
    var linepart = line.Split(' ');
    var direction = linepart[0][0];
    var length = int.Parse(linepart[1]);

    return (direction, length);
});

var (maxX, maxY) = (461937, 2000);
var digged = Enumerable.Range(0, maxX).Select(_ => Enumerable.Range(0, maxY).Select(_ => new Space()).ToArray()).ToArray();

var (x, y) = (5, 5);
digged[x][y].IsDigged = true;
foreach (var (direction, length) in instruction)
{
    IEnumerable<(int x, int y)> rangeToDig;
    switch (direction)
    {
        case 'U':
            rangeToDig = Enumerable.Range(y - length, length).Select(ny => (x, ny));
            y -= length;
            break;
        case 'D':
            rangeToDig = Enumerable.Range(y + 1, length).Select(ny => (x, ny));
            y += length;
            break;
        case 'L':
            rangeToDig = Enumerable.Range(x - length, length).Select(nx => (nx, y));
            x -= length;
            break;
        case 'R':
            rangeToDig = Enumerable.Range(x + 1, length).Select(nx => (nx, y));
            x += length;
            break;
        default:
            throw new InvalidOperationException();
    }
    foreach (var coo in rangeToDig)
    {
        digged[coo.x][coo.y].IsDigged = true;
    }
}

bool modificationMade = true;
for (int i = 0; i < maxY; i++)
{
    digged[0][i].IsOutside = true;
    digged[maxX - 1][i].IsOutside = true;
}
for (int i = 0; i < maxX; i++)
{
    digged[i][0].IsOutside = true;
    digged[i][maxX - 1].IsOutside = true;
}
while (modificationMade)
{
    modificationMade = false;
    for (int i = 1; i < maxY - 1; i++)
    {
        for (int j = 1; j < maxX; j++)
        {
            var e = digged[i][j];
            if (!e.IsOutside && !e.IsDigged)
            {
                if (digged[i - 1][j].IsOutside || digged[i + 1][j].IsOutside || digged[i][j - 1].IsOutside || digged[i][j + 1].IsOutside)
                {
                    modificationMade = true;
                    e.IsOutside = true;
                }
            }
        }
    }
}
/*
foreach (var line in Enumerable.Range(0, maxY).Select(
    j => new string(Enumerable.Range(0, maxY).Select(i => digged[i][j].IsDigged ? '#' : '.').ToArray())
))
{
    Console.WriteLine(line);
}*/

var totaldigged = digged.Select(c => c.Count(e => !e.IsOutside)).Sum();

Console.WriteLine($"Part 1 Total digger {totaldigged}");



instruction = File.ReadLines("input0.txt").Select(line =>
{
    var linepart = line.Split(' ');
    var direction = linepart[2][7] switch
    {
        '0' => 'R',
        '1' => 'D',
        '2' => 'L',
        '3' => 'U',
        _ => throw new InvalidDataException(),
    } ;
    var length = int.Parse(linepart[2][2..7], NumberStyles.HexNumber);

    return (direction, length);
});

 (maxX, maxY) = (20000, 20000);
 digged = Enumerable.Range(0, maxX).Select(_ => Enumerable.Range(0, maxY).Select(_ => new Space()).ToArray()).ToArray();

 (x, y) = (maxX / 2, maxY / 2);
digged[x][y].IsDigged = true;
foreach (var (direction, length) in instruction)
{
    IEnumerable<(int x, int y)> rangeToDig;
    switch (direction)
    {
        case 'U':
            rangeToDig = Enumerable.Range(y - length, length).Select(ny => (x, ny));
            y -= length;
            break;
        case 'D':
            rangeToDig = Enumerable.Range(y + 1, length).Select(ny => (x, ny));
            y += length;
            break;
        case 'L':
            rangeToDig = Enumerable.Range(x - length, length).Select(nx => (nx, y));
            x -= length;
            break;
        case 'R':
            rangeToDig = Enumerable.Range(x + 1, length).Select(nx => (nx, y));
            x += length;
            break;
        default:
            throw new InvalidOperationException();
    }
    foreach (var coo in rangeToDig)
    {
        digged[coo.x][coo.y].IsDigged = true;
    }
}

 modificationMade = true;
for (int i = 0; i < maxY; i++)
{
    digged[0][i].IsOutside = true;
    digged[maxX - 1][i].IsOutside = true;
}
for (int i = 0; i < maxX; i++)
{
    digged[i][0].IsOutside = true;
    digged[i][maxX - 1].IsOutside = true;
}
while (modificationMade)
{
    modificationMade = false;
    for (int i = 1; i < maxY - 1; i++)
    {
        for (int j = 1; j < maxX; j++)
        {
            var e = digged[i][j];
            if (!e.IsOutside && !e.IsDigged)
            {
                if (digged[i - 1][j].IsOutside || digged[i + 1][j].IsOutside || digged[i][j - 1].IsOutside || digged[i][j + 1].IsOutside)
                {
                    modificationMade = true;
                    e.IsOutside = true;
                }
            }
        }
    }
}

foreach (var line in Enumerable.Range(0, maxY).Select(
    j => new string(Enumerable.Range(0, maxY).Select(i => digged[i][j].IsDigged ? '#' : '.').ToArray())
))
{
    Console.WriteLine(line);
}

var totaldigged2 = digged.Select(c => c.Count(e => !e.IsOutside)).Sum();

Console.WriteLine($"Part 2 Total digger {totaldigged2}");

public class Space
{
    public bool IsDigged { get; set; }

    public  bool IsOutside {  get; set; }
}