

var data = File.ReadLines("input.txt").ToList();
var maxY = data.Count;
var maxX = data[0].Length;

int directionX;
int directionY;

var startX = 0;
var startY = 0;

(startX, startY) = data.Select((l, y) => (l, y))
    .Where(e => e.l.Contains('S'))
    .Select(e => (e.l.IndexOf('S'), e.y)).First();
Console.WriteLine($"Start {startX},{startY}");

var currentX = startX;
var currentY = startY;

(int, int) NextPosition()
{
    return (currentX + directionX, currentY + directionY);
}

bool IsNextOk()
{
    var (nextX, nextY) = NextPosition();
    if (nextX >= maxX || nextX < 0)
        return false;
    if (nextY >= maxY || nextY < 0)
        return false;

    char next = data[nextY][nextX];

    return next switch
    {
        'S' => true,
        '|' when directionX == 0 => true,
        '-' when directionY == 0 => true,
        'L' when directionX == -1 || directionY == 1 => true,
        'J' when directionX == 1 || directionY == 1 => true,
        '7' when directionX == 1 || directionY == -1 => true,
        'F' when directionX == -1 || directionY == -1 => true,
        _ => false,
    };
}

(int, int) NextDirection()
{
    char next = data[currentY][currentX];
    return next switch
    {
        'S' => (0, 0),
        '|' => (directionX, directionY),
        '-' => (directionX, directionY),
        'L' => (directionY, directionX),
        'J' => (-directionY, -directionX),
        '7' => (directionY, directionX),
        'F' => (-directionY, -directionX),
        _ => throw new InvalidOperationException(),
    };
}

List<(int, int)> path = [];
var data2 = data.Select(l => l.ToCharArray()).ToArray();

for (int test = 0; test < 4; test++)
{
    data2 = data.Select(l => l.ToCharArray()).ToArray();
    path = [];
    int nbStep = 0;
    bool hasFinished = false;

    (directionX, directionY) = test switch
    {
        0 => (1, 0),
        1 => (0, 1),
        2 => (-1, 0),
        3 => (0, -1),
        _ => throw new NotImplementedException(),
    };
    Console.WriteLine($"Starting test for ({directionX},{directionY}) ");

    while (!hasFinished)
    {
        path.Add((currentX, currentY));
        nbStep++;
        if (!IsNextOk())
        {
            break;
        }
        (currentX, currentY) = NextPosition();

        data2[currentY][currentX] = 'P';
        if (directionY > 0)
        {
            data2[currentY][currentX] = 'D';
        }

        (directionX, directionY) = NextDirection();
        if (directionY < 0)
        {
            data2[currentY][currentX] = 'U';
        }

        hasFinished = currentX == startX && currentY == startY;
    }

    if (hasFinished)
    {
        Console.WriteLine($"Total Step {nbStep} half total {nbStep / 2}");
        break;
    }
}

for (int x = 0; x < maxX; x++)
{
    for (int y = 0; y < maxY; y++)
    {
        if (data2[y][x] is 'D' or 'U' or 'P')
            continue;

        var nbPath = data2[y].Take(x).Aggregate(0, (nbPath, c) =>
        {
            if (c == 'D') nbPath--;
            if (c == 'U') nbPath++;
            return nbPath;
        });
        if (Math.Abs(nbPath) > 1)
        {
            throw new InvalidOperationException("Number of Path invalid");
        }
        data2[y][x] = nbPath == 0 ? 'O' : 'I';
    }
}

void PrintTable()
{
    for (var y = 0; y < maxY; y++)
    {
        Console.WriteLine(new string(data2[y]));
    }
    Console.WriteLine();
}
var totalInside = data2.Select(l => l.Count(c => c == 'I')).Sum();


PrintTable();
Console.WriteLine($"Part2 {totalInside} ");