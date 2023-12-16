var symbolCoordinate = File.ReadLines("input.txt").SelectMany(
    (line, lineNumber) =>
    {
        return line.SelectMany(
            (car, columNumber) =>
                car != '.' && !char.IsDigit(car) ? [(lineNumber, columNumber)] : Array.Empty<(int lineNumber, int columNumber)>()
        );
    }
).ToList();



var numberList = File.ReadLines("input.txt").SelectMany(
    (line, lineNumber) =>
    {
        var nb = new List<(int number, int lineNumber, int startCol, int endCol)>();
        for (int col = 0; col < line.Length; col++)
        {
            int startcol = col;
            int endcol = -1;
            while (col < line.Length && char.IsDigit(line, col))
            {
                endcol = col;
                col++;
            }
            if (endcol > 0)
            {
                int number = int.Parse(line[startcol..(endcol + 1)]);
                nb.Add((number, lineNumber, startcol, endcol));
            }
        }
        return nb;
    }
    );
var sum = numberList.Where((elem) =>
{
    return symbolCoordinate.Any(
        c =>
            c.columNumber >= elem.startCol - 1 && c.columNumber <= elem.endCol + 1
            && c.lineNumber >= elem.lineNumber - 1 && c.lineNumber <= elem.lineNumber + 1

        );
}
).Select(e => e.number).Sum();
Console.WriteLine($"Sum partnumber {sum}");

var gearCoordinate = File.ReadLines("input.txt").SelectMany(
    (line, lineNumber) => line.SelectMany(
            (car, columNumber) => car == '*' ? [(lineNumber, columNumber)] : Array.Empty<(int lineNumber, int columNumber)>()
        ));

var sumratio = gearCoordinate.Select(g =>
        numberList.Where(elem =>
            g.columNumber >= elem.startCol - 1 &&
            g.columNumber <= elem.endCol + 1 &&
            g.lineNumber >= elem.lineNumber - 1 &&
            g.lineNumber <= elem.lineNumber + 1
        )
    ).Where(ratiolist => ratiolist.Count() == 2).Select(ratioList => ratioList.First().number * ratioList.Last().number).Sum();

Console.WriteLine($"Sum of gear ratio {sumratio}");
