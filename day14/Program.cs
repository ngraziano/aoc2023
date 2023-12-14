
using System.Numerics;

var data = File.ReadLines("input.txt").ToList();

// to array of array with fence
data = data.Prepend(new string('#', data[0].Length)).Append(new string('#', data[0].Length)).ToList();

char[][] dataA = data.Select(l => l.Select(c => c).Prepend('#').Append('#').ToArray()).ToArray();

static void MoveStoneRight(char[][] data)
{
    foreach (var lineArray in data)
    {
        for (int i = 1; i < lineArray.Length - 1; i++)
        {
            if (lineArray[i] != 'O')
                continue;

            var next = Array.FindIndex(lineArray, i + 1, c => c == '#');
            var freePosition = Array.FindLastIndex(lineArray, next, c => c == '.');
            if (freePosition > i)
            {
                lineArray[i] = '.';
                lineArray[freePosition] = 'O';
            }
        }
    }
}

static void MoveStoneLeft(char[][] data)
{

    foreach (var lineArray in data[1..^1])
    {
        for (int i = 1; i < lineArray.Length - 1; i++)
        {
            if (lineArray[i] != 'O')
                continue;

            var next = Array.FindLastIndex(lineArray, i - 1, c => c == '#');
            var freePosition = Array.FindIndex(lineArray, next, c => c == '.');
            if (freePosition < i && freePosition > 0)
            {
                lineArray[i] = '.';
                lineArray[freePosition] = 'O';
            }
        }
    }
}

static BigInteger GetId(char[][] data)
{
    return data.Aggregate(
        new BigInteger(0),
        (aggLine, line) => line.Aggregate(aggLine, (agg, c) => (agg << 1) + (c == 'O' ? 1 : 0))
        );
}

static char[][] Transpose(char[][] data) =>
    Enumerable.Range(0, data[0].Length).Select(
        i => data.Select(l => l[i]).ToArray()
    ).ToArray();

static void Print(char[][] data)
{
    foreach (var lineArray in data)
    {
        Console.WriteLine(lineArray);
    }
    Console.WriteLine();

}


// PART 1
dataA = Transpose(dataA);
MoveStoneLeft(dataA);

var sum = dataA.Select(line =>
{
    var max = line.Length - 1;
    return line.Select((c, i) => (c, i)).Where(e => e.c == 'O').Select(e => max - e.i).Sum();

}).Sum();
dataA = Transpose(dataA);

Console.WriteLine($"Part 1 Sum  = {sum}");

Dictionary<BigInteger, (BigInteger next, int cycle)> cacheNext = [];
Dictionary<BigInteger, long> cacheSum = [];


dataA = data.Select(l => l.Select(c => c).Prepend('#').Append('#').ToArray()).ToArray();

var currentId = GetId(dataA);
var end = 1000000000;
for (int cylce = 0; cylce < end; cylce++)
{
    if (cacheNext.TryGetValue(currentId, out var cachedNext))
    {
        currentId = cachedNext.next;
        var periode = cylce - cachedNext.cycle;
        cylce += ((end - cylce) / periode) * periode;
        continue;
    }

    // north
    dataA = Transpose(dataA);
    MoveStoneLeft(dataA);

    // west
    dataA = Transpose(dataA);
    MoveStoneLeft(dataA);

    // south
    dataA = Transpose(dataA);
    MoveStoneRight(dataA);

    // east
    dataA = Transpose(dataA);
    MoveStoneRight(dataA);

    var nextId = GetId(dataA);
    cacheNext[currentId] = (nextId, cylce);

    if (cylce < 3)
    {
        Console.WriteLine($"Cycle {cylce}");
        Print(dataA);
    }


    var dataCalc = Transpose(dataA);
    cacheSum[nextId] = dataCalc.Select(line =>
    {
        var max = line.Length - 1;
        return line.Select((c, i) => (c, i)).Where(e => e.c == 'O').Select(e => max - e.i).Sum();

    }).Sum();

    // Console.WriteLine($"{currentId} => {nextId} P: {cacheSum[nextId]}");
    currentId = nextId;
}

var sum2 = cacheSum[currentId];
Console.WriteLine($"Part 2 Sum = {sum2}");

