const int maxRedCubes = 12;
const int maxGreenCubes = 13;
const int maxBlueCubes = 14;

static int MaxByColorName(string colorName) => colorName switch
{
    "red" => maxRedCubes,
    "green" => maxGreenCubes,
    "blue" => maxBlueCubes,
    _ => 0,
};

var sumOfId = File.ReadAllLines("input.txt").Select(line =>
{
    var part = line.Split(":");
    int gameID = int.Parse(part[0].Split(" ")[1]);

    foreach (var reaveal in part[1].Split(";"))
    {
        foreach (var colorReveal in reaveal.Trim().Split(","))
        {
            var colorPart = colorReveal.Trim().Split(" ");
            var nbCube = int.Parse(colorPart[0]);
            var maxNbCube = MaxByColorName(colorPart[1]);
            if (nbCube > maxNbCube)
            {
                return null;
            }
        }
    }
    return (int?)gameID;
})
    .Where(id => id is not null)
    .Sum();


Console.WriteLine($"Part 1 Sum of game ids {sumOfId}");


var sumOfPower = File.ReadAllLines("input.txt").Select(line =>
{
    var part = line.Split(":");
    int gameID = int.Parse(part[0].Split(" ")[1]);


    (int red, int blue, int green) minByColor = (0, 0, 0);

    foreach (var reaveal in part[1].Split(";"))
    {
        foreach (var colorReveal in reaveal.Trim().Split(","))
        {
            var colorPart = colorReveal.Trim().Split(" ");
            var nbCube = int.Parse(colorPart[0]);

            minByColor = colorPart[1] switch
            {
                "red" => (Math.Max(nbCube, minByColor.red), minByColor.blue, minByColor.green),
                "green" => (minByColor.red, minByColor.blue, Math.Max(minByColor.green, nbCube)),
                "blue" => (minByColor.red, Math.Max(minByColor.blue, nbCube), minByColor.green),
                _ => minByColor,
            };

        }
    }
    return minByColor.red * minByColor.blue * minByColor.green;

})
    .Sum();

Console.WriteLine($"Part 2 Sum of power {sumOfPower}");

