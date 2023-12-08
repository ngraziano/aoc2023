
var numberOfWinBycard =  File.ReadLines("input.txt").Select(
    line =>
    {
        var cardData = line.Split(':')[1].Trim().Split('|');
        var winingNumbers = cardData[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s=>int.Parse(s)).ToList();
        var carNumbers = cardData[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s));

        return carNumbers.Count(n => winingNumbers.Contains(n));
    }).ToList();

var sumOfCardScore = numberOfWinBycard.Select(nbWiningNumber=>nbWiningNumber > 0 ? Math.Pow(2, nbWiningNumber - 1) : 0).Sum();
Console.WriteLine($"Sum of card score {sumOfCardScore}");

var cardCountAndWining = numberOfWinBycard.ConvertAll(wining =>  new int[2] { 1, wining });


for(int i = 0; i < cardCountAndWining.Count; i++)
{
    for(int j = i+1; j< i + 1 + cardCountAndWining[i][1]; j++)
    {
        cardCountAndWining[j][0] += cardCountAndWining[i][0];
    }
}

var totalNbOfCard = cardCountAndWining.Select(e => e[0]).Sum();
Console.WriteLine($"Total Nb of card {totalNbOfCard}");


