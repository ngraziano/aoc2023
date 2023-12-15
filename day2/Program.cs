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
    var (gameID, gameData) = line.Split(":") switch
    {
        [string id, string data] => (int.Parse(id.Split(' ')[1]), data),
        _ => throw new InvalidDataException("Line invalid"),
    };

    return gameData.Split(";").SelectMany(reaveal =>
         reaveal.Trim().Split(",").Select(
         (string color, int nbCube) (colorReveal) => colorReveal.Trim().Split(" ") switch
             {
                 [var nb, var c] => (c, int.Parse(nb)),
                 _ => throw new InvalidDataException(),
             }
         )
     )
     .Select((int maxNbCube, int nbCube) (e) => (MaxByColorName(e.color), e.nbCube))
     .Where((e) => e.nbCube > e.maxNbCube)
     .Select(_ => (int?)null)
     .FirstOrDefault(gameID);
})
    .Where(id => id is not null)
    .Sum();


Console.WriteLine($"Part 1 Sum of game ids {sumOfId}");


var sumOfPower = File.ReadAllLines("input.txt").Select(line =>
{
    var (gameId, gameData) = line.Split(":") switch
    {
        [string id, string data] => (int.Parse(id.Split(' ')[1]), data),
        _ => throw new InvalidDataException("Line invalid"),
    };

    (int red, int blue, int green) minByColor = (0, 0, 0);

    minByColor = gameData.Split(";").SelectMany(reaveal =>
         reaveal.Trim().Split(",").Select(
         (string color, int nbCube) (colorReveal) => colorReveal.Trim().Split(" ") switch
             {
                 [var nb, var c] => (c, int.Parse(nb)),
                 _ => throw new InvalidDataException(),
             }
        )
    )
     .Aggregate(minByColor, (agg, e) => e.color switch
     {
         "red" => (Math.Max(e.nbCube, agg.red), agg.blue, agg.green),
         "green" => (agg.red, agg.blue, Math.Max(agg.green, e.nbCube)),
         "blue" => (agg.red, Math.Max(agg.blue, e.nbCube), agg.green),
         _ => agg,
     });

    return minByColor.red * minByColor.blue * minByColor.green;
})
    .Sum();

Console.WriteLine($"Part 2 Sum of power {sumOfPower}");

