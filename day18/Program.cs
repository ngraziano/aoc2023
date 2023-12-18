

using System.Globalization;

var instruction = File.ReadLines("input.txt").Select(line =>
{
    var linepart = line.Split(' ');
    var direction = linepart[0][0];
    var length = int.Parse(linepart[1]);

    return (direction, length);
});

var (x, y) = (0, 0);

List<Segment> segments = instruction.Select(instruction =>
{
    Segment segment;
    switch (instruction.direction)
    {
        case 'U':
            segment = new() { Direction = instruction.direction, XMin = x, XMax = x, YMin = y - instruction.length, YMax = y };
            y -= instruction.length;
            break;
        case 'D':
            segment = new() { Direction = instruction.direction, XMin = x, XMax = x, YMin = y, YMax = y + instruction.length };
            y += instruction.length;
            break;
        case 'L':
            segment = new() { Direction = instruction.direction, XMin = x - instruction.length, XMax = x, YMin = y, YMax = y };
            x -= instruction.length;
            break;
        case 'R':
            segment = new() { Direction = instruction.direction, XMin = x, XMax = x + instruction.length, YMin = y, YMax = y };
            x += instruction.length;
            break;
        default:
            throw new InvalidOperationException();
    }
    return segment;
})
//.OrderBy(e => e.Direction switch
//{
//    'U' => 0,
//    'D' => 0,
//   _ => 1,
//
//})
.ToList();

(int xmin, int xmax, int ymin, int ymax) limit = segments.Aggregate((int.MaxValue, int.MinValue, int.MaxValue, int.MinValue),
    ((int xmin, int xmax, int ymin, int ymax) agg, Segment s)
    => (Math.Min(agg.xmin, s.XMin), Math.Max(agg.xmax, s.XMax), Math.Min(agg.ymin, s.YMin), Math.Max(agg.ymax, s.YMax)));


var nbInside = 0L;
for (int j = limit.ymin; j < limit.ymax + 1; j++)
{
    bool isInside = false;
    bool previousInside = false;
    bool isTop = false;
    bool isBottom = false;
    int i = limit.xmin - 1;
    foreach (Segment segment in segments.Where(s => s.PointBelongToLine(j) && (s.Direction == 'U' || s.Direction == 'D')).OrderBy(s => s.XMin))
    {

        if (isInside)
        {
            nbInside += segment.XMin - i - 1;
//            Console.Write(new string('#', segment.XMin - i - 1));
        }
        else
        {
//            Console.Write(new string('.', segment.XMin - i - 1));
        }
        nbInside++;
      //  Console.Write('*');
        bool wasTop = isTop;
        isTop = segment.YMax == j;
        bool wasBottom = isBottom;
        isBottom = segment.YMin == j;

        if (!(isTop || isBottom))
        {

            isInside = !isInside;
        }
        else if (!wasBottom && !wasTop)
        {
            previousInside = isInside;
            isInside = true;
        }
        else if ((isBottom && wasBottom) || (isTop && wasTop))
        {
            isInside = previousInside;
            isTop = false;
            isBottom = false;
        }
        else if ((isTop && wasBottom) || (isBottom && wasTop))
        {
            isInside = !previousInside;
            isTop = false;
            isBottom = false;
        }


        i = segment.XMin;
    }
//    Console.WriteLine();
}

Console.WriteLine($"Part 1 Total digger {nbInside}");
#if false
for (int j = limit.ymin; j < limit.ymax + 1; j++)
{
    bool isOuside = true;
    for (int i = limit.xmin - 1; i < limit.xmax + 1; i++)
    {

        var s1 = segments.FindIndex(s => s.PointBelong(i, j));
        if (s1 >= 0 || !isOuside)
        {

            nbInside++;
            if (s1 >= 0)
            {
                //       Console.Write('*');
            }
            else
            {
                //     Console.Write('#');
            }
        }
        else
        {
            //     Console.Write('.');
        }

        if (s1 >= 0)
        {
            if ((segments[s1].Direction == 'U' || segments[s1].Direction == 'D') && segments[s1].YMax != j)
            {
                isOuside = !isOuside;
            }

        }
    }
    Console.WriteLine();
}


Console.WriteLine($"Part 1 Total digger {nbInside}");
#endif


instruction = File.ReadLines("input.txt").Select(line =>
{
    var linepart = line.Split(' ');
    var direction = linepart[2][7] switch
    {
        '0' => 'R',
        '1' => 'D',
        '2' => 'L',
        '3' => 'U',
        _ => throw new InvalidDataException(),
    };
    var length = int.Parse(linepart[2][2..7], NumberStyles.HexNumber);

    return (direction, length);
});





(x, y) = (0, 0);

segments = instruction.Select(instruction =>
{
   Segment segment;
   switch (instruction.direction)
   {
       case 'U':
           segment = new() { Direction = instruction.direction, XMin = x, XMax = x, YMin = y - instruction.length, YMax = y };
           y -= instruction.length;
           break;
       case 'D':
           segment = new() { Direction = instruction.direction, XMin = x, XMax = x, YMin = y, YMax = y + instruction.length };
           y += instruction.length;
           break;
       case 'L':
           segment = new() { Direction = instruction.direction, XMin = x - instruction.length, XMax = x, YMin = y, YMax = y };
           x -= instruction.length;
           break;
       case 'R':
           segment = new() { Direction = instruction.direction, XMin = x, XMax = x + instruction.length, YMin = y, YMax = y };
           x += instruction.length;
           break;
       default:
           throw new InvalidOperationException();
   }
   return segment;
}).ToList();

limit = segments.Aggregate((int.MaxValue, int.MinValue, int.MaxValue, int.MinValue),
   ((int xmin, int xmax, int ymin, int ymax) agg, Segment s)
   => (Math.Min(agg.xmin, s.XMin), Math.Max(agg.xmax, s.XMax), Math.Min(agg.ymin, s.YMin), Math.Max(agg.ymax, s.YMax)));

nbInside = 0;

Console.WriteLine($"Line Range {limit.ymin}..{limit.ymax}");

for (int j = limit.ymin; j < limit.ymax + 1; j++)
{
    if (j % 1000 == 0)
    {
        Console.WriteLine($"Line {j}");
    }
    bool isInside = false;
    bool previousInside = false;
    bool isTop = false;
    bool isBottom = false;
    int i = limit.xmin - 1;
    foreach (Segment segment in segments.Where(s => s.PointBelongToLine(j) && (s.Direction == 'U' || s.Direction == 'D')).OrderBy(s => s.XMin))
    {
        if (isInside)
        {
            nbInside += segment.XMin - i - 1;
        }
        nbInside++;
        bool wasTop = isTop;
        isTop = segment.YMax == j;
        bool wasBottom = isBottom;
        isBottom = segment.YMin == j;

        if (!(isTop || isBottom))
        {
            isInside = !isInside;
        }
        else if (!wasBottom && !wasTop)
        {
            previousInside = isInside;
            isInside = true;
        }
        else if ((isBottom && wasBottom) || (isTop && wasTop))
        {
            isInside = previousInside;
            isTop = false;
            isBottom = false;
        }
        else if ((isTop && wasBottom) || (isBottom && wasTop))
        {
            isInside = !previousInside;
            isTop = false;
            isBottom = false;
        }


        i = segment.XMin;
    }
}

Console.WriteLine($"Part 2 Total digger {nbInside}");

public record Segment
{
    public required int XMin { get; init; }
    public required int XMax { get; init; }
    public required int YMin { get; init; }
    public required int YMax { get; init; }
    public required char Direction { get; init; }

    public bool PointBelong(int x, int y)
        => x >= XMin && y >= YMin && x <= XMax && y <= YMax;

    public bool PointBelongToLine(int y) => y >= YMin && y <= YMax;
}

