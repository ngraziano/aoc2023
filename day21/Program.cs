
var board = File.ReadLines("input.txt").ToList();


(int Y, int X) start = board.Select((l, y) => (y, l.IndexOf('S'))).First(p => p.Item2 > 0);
var maxY = board.Count;
var maxX = board[0].Length;

HashSet<(int Y, int X)> pos = [start];
for (int i = 0; i < 6; i++)
{
    pos = pos.SelectMany(s => NextStep(s)).ToHashSet();
}

Console.WriteLine($"Part 1 {pos.Count}");


// part2

int count = 26501365;

int rest = count % maxX;

Console.WriteLine($"dim : {maxX},{maxY} : rest = {rest}");
pos = [start];
List<int> dataForRegression = [];
for (int i = 1; i <= count; i++)
{
    pos = pos.SelectMany(NextStep2).ToHashSet();
    if ((i - rest) % maxX == 0)
    {
        Console.WriteLine($"Step {i,4} => ({(i - rest) / maxX},{pos.Count,10})");
        dataForRegression.Add(pos.Count);
        if ((i - rest) / maxX == 3)
            break;
    }
}

Console.WriteLine("Calculate a*x²+b*x+c for this values : ");
int v = 0;
foreach (var data in dataForRegression)
{
    Console.WriteLine($"{v++},{data}");
}

var c = dataForRegression[0];
Console.WriteLine($"c= {c} ");

/*
 * a+b + c = x1
 * 4a+ 2b + c = x2
 * 2a  = x2-2*x1 + c
 */
var a = ((dataForRegression[2] + c) / 2) - dataForRegression[1];
Console.WriteLine($"a={a}");

var b = dataForRegression[1] - c - a;

for (int i = 0; i < 4; i++)
{
    Console.WriteLine($"Check {i}=>{i * i * a + i * b + c}");
}

long j = (count - rest) / maxX;
Console.WriteLine($"Part 2 {count} => {j} => {j * j * a + j * b + c} ");


void Print(HashSet<(int Y, int X)> pos)
{

    int ym = (pos.Min(p => p.Y) - maxY + 1) / maxY * maxY;
    int yp = (pos.Max(p => p.Y) + maxY - 1) / maxY * maxY;
    int xm = (pos.Min(p => p.X) - maxX + 1) / maxX * maxX;
    int xp = (pos.Max(p => p.X) + maxX - 1) / maxX * maxX;

    for (int y = ym; y < yp; y++)
    {
        for (int x = xm; x < xp; x++)
        {
            if (pos.Contains((y, x)))
            {
                Console.Write("O");
            }
            else
            {
                var testX = ((x % maxX) + maxX) % maxX;
                var testY = ((y % maxY) + maxY) % maxY;
                Console.Write(board![testY][testX]);
            }
        }
        Console.WriteLine();
    }
}


IEnumerable<(int Y, int X)> NextStep((int Y, int X) start)
{
    if (IsValid((start.Y - 1, start.X)))
        yield return (start.Y - 1, start.X);

    if (IsValid((start.Y + 1, start.X)))
        yield return (start.Y + 1, start.X);

    if (IsValid((start.Y, start.X - 1)))
        yield return (start.Y, start.X - 1);

    if (IsValid((start.Y, start.X + 1)))
        yield return (start.Y, start.X + 1);
}

bool IsValid((int Y, int X) p) =>
        p.X >= 0 && p.X < maxY && p.Y >= 0 && p.Y < maxY
        && board[p.Y][p.X] != '#';



IEnumerable<(int Y, int X)> NextStep2((int Y, int X) start)
{
    if (IsValid2((start.Y - 1, start.X)))
        yield return (start.Y - 1, start.X);

    if (IsValid2((start.Y + 1, start.X)))
        yield return (start.Y + 1, start.X);

    if (IsValid2((start.Y, start.X - 1)))
        yield return (start.Y, start.X - 1);

    if (IsValid2((start.Y, start.X + 1)))
        yield return (start.Y, start.X + 1);
}

bool IsValid2((int Y, int X) p)
{
    var testX = ((p.X % maxX) + maxX) % maxX;
    var testY = ((p.Y % maxY) + maxY) % maxY;
    return board![testY][testX] != '#';
}
