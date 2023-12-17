#if false
const string file = "input0.txt";

var board = File.ReadLines(file)
    .Select(l => l.Select(c => c - '0').ToArray()).ToArray();





(int X, int Y) maxPos = (board[0].Length - 1, board.Length - 1);





// X=Y
// get a trivial max to limit search
var currentMinimal = Enumerable.Range(0, maxPos.X)
    .Select(x => board[x][x + 1] + board[x + 1][x + 1]).Sum();
 currentMinimal = 120;
bool InBoard(int x, int y)
{
    return x >= 0 && x <= maxPos.X
        && y >= 0 && y <= maxPos.Y;
}


IEnumerable<PathPos> Move(PathPos pos)
{
    var (newX, newY) = pos.Dir switch
    {
        Direction.N => (pos.X, pos.Y - 1),
        Direction.S => (pos.X, pos.Y + 1),
        Direction.E => (pos.X + 1, pos.Y),
        Direction.W => (pos.X - 1, pos.Y),
        _ => throw new NotImplementedException(),
    };


    if (!InBoard(newX, newY))
    {
        yield break;
    }

    if (pos.NbStraight < 2)
    {
        yield return new PathPos(newX, newY, pos.Dir, pos.NbStraight + 1);

    }

    yield return new PathPos(newX, newY, (Direction)(((int)pos.Dir + 1) % 4), 0);
    yield return new PathPos(newX, newY, (Direction)(((int)pos.Dir + 3) % 4), 0);
}


var cache = new Dictionary<PathPos, int>();

int CalculateMinToEndFrom(PathPos pos, HashSet<PathPos> history, int currentLost)
{
    if ((pos.X, pos.Y) == maxPos)
        return board![pos.Y][pos.X];

 //   if (currentLost > currentMinimal)
 //       return -1;

    if (cache!.TryGetValue(pos, out var cachedValue))
    {
        return cachedValue;
    }

    var value = Move(pos)
        .Where(p=> !history.Contains(p))
        .Select(p => {
            var newLost = currentLost + board![p.Y][p.X];
            // Console.WriteLine($" {p} => {currentLost} / { string.Join(",", history.Select(p => $"({p.X},{p.Y},{p.Dir})"))} "  );
            Console.WriteLine($" {p} => {currentLost}");
            var child = CalculateMinToEndFrom(p, [.. history, p], newLost);

            if (child > 0) {
                return board![p.Y][p.X] + child;
            } else
            {
                return -1;
            }
    }
    
    ).Where(x=>x>0).DefaultIfEmpty(-1).Min();

    if(value>0)
    {
        cache[pos] = value;
        return value;

    }
    else
    {
        return -1;
    }
}

var min = Math.Min(
    CalculateMinToEndFrom(new PathPos(0, 0, Direction.E, 0), [],0),
    CalculateMinToEndFrom(new PathPos(0, 0, Direction.S, 0), [], 0)
    );

Console.WriteLine($"Part 1 {min}");


enum Direction
{
    N = 0,
    E,
    S,
    W
}


record PathPos(int X, int Y, Direction Dir, int NbStraight);
#endif