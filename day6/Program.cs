
var lines = File.ReadLines("input.txt").ToList();

var times = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
var distances = lines[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

// bruteforce
var productMarg = times.Zip(distances).Select(e =>
{
    var nbWin = 0;
    var time = e.First;
    var distance = e.Second;
    for (int speed = 0; speed < time; speed++)
    {
        if(speed*(time - speed) > distance)
        {
            nbWin++;
        }
    }
    return nbWin;

}).Aggregate(1, (acc, val) => acc * val);


Console.WriteLine($"Part1 {productMarg}");

// part 2
long realTime = 40817772;
long realDistance = 219101213651089;
long nbWin = 0;
for (long speed = 0; speed < realTime; speed++)
{
    if (speed * (realTime - speed) > realDistance)
    {
        nbWin++;
    }
}
Console.WriteLine($"Part 2 {nbWin}");

// Math method (no program needed)
/*
 
  speed * (time - speed) = distance
  -speed² + 40817772*seep - 219101213651089 = 0
 6358212.9 < speed < 34459559.08

 nbOfWay to win : 34459559- 6358212 => 28101347

*/

