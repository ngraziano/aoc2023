

var board = File.ReadLines("input.txt").ToList();

var maxY = board.Count;
var maxX = board[0].Length;
Beam.MaxY = maxY;
Beam.MaxX = maxX;

static void Energize(List<string> board, List<Beam> beams, SeenBeam[,] seen)
{
    int i = 0;
    while (beams.Count > 0)
    {
        i++;
        if (i % 100 == 0)
        {
            Console.WriteLine($"Nb Loop :{i}, nb beam {beams.Count}");
        }
        foreach (var beam in beams.ToList())
        {
            Beam? newBeam = null;


            switch (beam.DX, beam.DY)
            {
                case (0, 1): seen[beam.Y, beam.X].YPos = true; break;
                case (1, 0): seen[beam.Y, beam.X].XPos = true; break;
                case (0, -1): seen[beam.Y, beam.X].YNeg = true; break;
                case (-1, 0): seen[beam.Y, beam.X].XNeg = true; break;
            }


            switch (board[beam.Y][beam.X])
            {
                case '.':
                case '|' when (beam.DY == 1 || beam.DY == -1):
                case '-' when (beam.DX == 1 || beam.DX == -1):
                    beam.Y += beam.DY;
                    beam.X += beam.DX;
                    break;
                case '|':
                    newBeam = new() { X = beam.X, Y = beam.Y + 1, DX = 0, DY = 1 };
                    beam.DY = -1;
                    beam.DX = 0;
                    beam.Y += -1;
                    break;
                case '-':
                    newBeam = new() { X = beam.X + 1, Y = beam.Y, DX = 1, DY = 0 };
                    beam.DY = 0;
                    beam.DX = -1;
                    beam.X += -1;
                    break;
                case '/':
                    (beam.DY, beam.DX) = (-beam.DX, -beam.DY);
                    beam.X += beam.DX;
                    beam.Y += beam.DY;
                    break;
                case '\\':
                    (beam.DY, beam.DX) = (beam.DX, beam.DY);
                    beam.X += beam.DX;
                    beam.Y += beam.DY;
                    break;
            }

            if (newBeam?.IsInBoard == true && newBeam?.IsSeen(seen[newBeam.Y, newBeam.X]) == false)
            {
                beams.Add(newBeam);
            }
            if (!beam.IsInBoard || beam.IsSeen(seen[beam.Y, beam.X]))
            {
                beams.Remove(beam);
            }
        }
    }
}


var seen = new SeenBeam[maxY, maxX];
var beams = new List<Beam>() { new() { Y = 0, X = 0, DY = 0, DX = 1 } };
Energize(board, beams, seen);
var nbSeen = seen.Cast<SeenBeam>().Where(x => x.IsSeen).Count();

Console.WriteLine($"Nb Seen {nbSeen}");


var maxForY = Enumerable.Range(0, maxY).Select(
    (startY) =>
    {
        var seenLeft = new SeenBeam[maxY, maxX];
        var beamsLeft = new List<Beam>() { new() { Y = startY, X = 0, DY = 0, DX = 1 } };
        Energize(board, beamsLeft, seenLeft);
        var nbSeenLeft = seenLeft.Cast<SeenBeam>().Count(x => x.IsSeen);

        var seenRight = new SeenBeam[maxY, maxX];
        var beamsRight = new List<Beam>() { new() { Y = startY, X = maxX-1, DY = 0, DX = -1 } };
        Energize(board, beamsRight, seenRight);
        var nbSeenRight = seenLeft.Cast<SeenBeam>().Count(x => x.IsSeen);


        return Math.Max(nbSeenLeft, nbSeenRight);
    }).Max();


var maxForX = Enumerable.Range(0, maxX).Select(
    (startX) =>
    {
        var seenUp = new SeenBeam[maxY, maxX];
        var beamsUp = new List<Beam>() { new() { Y = 0, X = startX, DY = 1, DX = 0 } };
        Energize(board, beamsUp, seenUp);
        var nbSeenUp = seenUp.Cast<SeenBeam>().Count(x => x.IsSeen);

        var seenDown = new SeenBeam[maxY, maxX];
        var beamsDown = new List<Beam>() { new() { Y = maxY-1, X = 0, DY = -1, DX = 0 } };
        Energize(board, beamsDown, seenDown);
        var nbSeenDown = seenDown.Cast<SeenBeam>().Count(x => x.IsSeen);


        return Math.Max(nbSeenUp, nbSeenDown);
    }).Max();

Console.WriteLine($"Max for Y {maxForY}, X {maxForX}");

struct SeenBeam
{
    public bool XPos { get; set; }
    public bool XNeg { get; set; }
    public bool YPos { get; set; }
    public bool YNeg { get; set; }

    public readonly bool IsSeen => XPos || XNeg || YPos || YNeg;
}

record Beam
{
    public static int MaxX;
    public static int MaxY;
    public int Y { get; set; }
    public int X { get; set; }
    public int DY { get; set; }
    public int DX { get; set; }

    public bool IsInBoard => X >= 0 && Y >= 0 && X < MaxY && Y < MaxY;


    public bool IsSeen(SeenBeam s)
    {
        return (DX, DY) switch
        {
            (1, 0) => s.XPos,
            (0, 1) => s.YPos,
            (-1, 0) => s.XNeg,
            (0, -1) => s.YNeg,
            _ => throw new InvalidOperationException(),
        };
    }

}