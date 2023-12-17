#if true

using System.Collections.Concurrent;

const string file = "input.txt";

var board = File.ReadLines(file)
    .Select(l => l.Select(c => c - '0').ToArray()).ToArray();

PathTesting.MaxPos = (board[0].Length - 1, board.Length - 1);

// X=Y
// get a max 
var currentMinimal = Enumerable.Range(0, PathTesting.MaxPos.X)
    .Select(x => board[x][x + 1] + board[x + 1][x + 1]).Sum();
#if false
var pathTotest = new List<PathTesting>()
{
    new()
    {
        Direction = Direction.E,
        NbStraight = 0,
        Pos = (0,0),
    },
    new()
    {
        Direction = Direction.S,
        NbStraight = 0,
        Pos = (0,0),
  //      OldPos = []
    }
};

var alreadyThere = new ConcurrentDictionary<(int X, int Y, Direction D, int NbStraight), int>();

int i = 0;
while (pathTotest.Count > 0)
{
    i++;
    pathTotest = pathTotest.AsParallel().SelectMany(path => path.Move(board))
        .OrderBy(p => p.HeatLoss)
        .Where(p =>
    {
        if (alreadyThere.TryGetValue((p.Pos.X, p.Pos.Y, p.Direction, p.NbStraight), out var val))
        {
            if (val >= p.HeatLoss)
            {
                alreadyThere[(p.Pos.X, p.Pos.Y, p.Direction, p.NbStraight)] = p.HeatLoss;
                return true;
            }
            return false;
        }
        alreadyThere[(p.Pos.X, p.Pos.Y, p.Direction, p.NbStraight)] = p.HeatLoss;

        return true;
    }).ToList();

    currentMinimal = pathTotest.Where(e => e.Pos == PathTesting.MaxPos)
                            .Select(e => e.HeatLoss)
                            .DefaultIfEmpty(currentMinimal)
                            .Min();
    /*   foreach (var path in pathTotest.Where(p => p.Pos == PathTesting.MaxPos && p.HeatLoss == currentMinimal))
       {
           Console.WriteLine($"mini {path.HeatLoss} => {string.Join(" ", path.OldPos)}");

           var boardTest = File.ReadLines(file).Select(l => l.ToArray()).ToArray();
           foreach (var posd in path.OldPos)
           {
               boardTest[posd.Item1.Y][posd.Item1.X] = posd.Item2 switch
               {
                   Direction.E => '>',
                   Direction.N => '^',
                   Direction.S => 'v',
                   Direction.W => 'W',
                   _ => throw new NotImplementedException()
               };
           }
           foreach (var l in boardTest)
           {
               Console.WriteLine(new string(l));
           }
       }
    */
    pathTotest = pathTotest.Where(e => e.Pos != PathTesting.MaxPos && e.HeatLoss < currentMinimal)
                            .ToList();

    var currentMinLoss = pathTotest.Select(e => e.HeatLoss).FirstOrDefault(currentMinimal);
    var currentMaxLoss = pathTotest.Select(e => e.HeatLoss).LastOrDefault(currentMinimal);

    Console.WriteLine($"[{i}] Nb test path {pathTotest.Count}, current minimal {currentMinimal} current path loss {currentMinLoss} {currentMaxLoss}");
};
#endif


var pathTotest = new Stack<PathTesting>();
pathTotest.Push(
    new()
    {
        Direction = Direction.E,
        NbStraight = 0,
        Pos = (0, 0),
    });
pathTotest.Push(new()
    {
        Direction = Direction.S,
        NbStraight = 0,
        Pos = (0,0),
  //      OldPos = []
    }
);
int i = 0;

var alreadyThere = new Dictionary<(int X, int Y, Direction D, int NbStraight), int>();

while (pathTotest.TryPop(out var path))
{
    
    if (path.HeatLoss > currentMinimal)
        continue;

    if (alreadyThere.TryGetValue((path.Pos.X, path.Pos.Y, path.Direction, path.NbStraight), out var lostSeen) 
        && lostSeen <= path.HeatLoss)
    {
        continue;
    }

    alreadyThere[(path.Pos.X, path.Pos.Y, path.Direction, path.NbStraight)] = path.HeatLoss;

    if (path.Pos == PathTesting.MaxPos && path.HeatLoss < currentMinimal)
    {
        currentMinimal = path.HeatLoss;
    } 
    else
    {
        i++;
        if(i%100000==0)
        Console.WriteLine($"{i} {pathTotest.Count}    =>  {path.HeatLoss} | {currentMinimal}");

        foreach (var np in path.Move(board))
        {
            pathTotest.Push(np);
        }
    }
}


Console.WriteLine($"Part 1 {currentMinimal}");


enum Direction
{
    N = 0,
    E,
    S,
    W
}

struct PathTesting
{
    public static (int X, int Y) MaxPos { get; set; }
    public (int X, int Y) Pos { get; init; }
    public Direction Direction { get; init; }
    public int NbStraight { get; init; }

    //  public required List<((int X, int Y), Direction)> OldPos { get; init; }
    public int HeatLoss { get; init; }

    internal IEnumerable<PathTesting> Move(int[][] board)
    {
        var newPos = MovePos(Direction, Pos);
        if (!InBoard(newPos))
        {
            yield break;
        }

        if (NbStraight < 2)
        {
            yield return new PathTesting()
            {
                Direction = Direction,
                NbStraight = NbStraight + 1,
                HeatLoss = HeatLoss + board[newPos.Y][newPos.X],
                Pos = newPos,
                //             OldPos = [.. OldPos, (newPos, Direction)],
            };
        }

        yield return new PathTesting()
        {
            Direction = (Direction)(((int)Direction + 1) % 4),
            NbStraight = 0,
            HeatLoss = HeatLoss + board[newPos.Y][newPos.X],
            Pos = newPos,
            //        OldPos = [.. OldPos, (newPos, Direction)],
        };
        yield return new PathTesting()
        {
            Direction = (Direction)(((int)Direction + 3) % 4),
            NbStraight = 0,
            HeatLoss = HeatLoss + board[newPos.Y][newPos.X],
            Pos = newPos,
            //        OldPos = [.. OldPos, (newPos, Direction)],
        };
    }
    private static (int X, int Y) MovePos(Direction direction, (int X, int Y) pos)
    {
        return direction switch
        {
            Direction.N => (pos.X, pos.Y - 1),
            Direction.S => (pos.X, pos.Y + 1),
            Direction.E => (pos.X + 1, pos.Y),
            Direction.W => (pos.X - 1, pos.Y),
            _ => throw new NotImplementedException(),
        };
    }
    private static bool InBoard((int X, int Y) pos)
    {
        return pos.X >= 0 && pos.X <= MaxPos.X
            && pos.Y >= 0 && pos.Y <= MaxPos.Y;
    }
}
#endif