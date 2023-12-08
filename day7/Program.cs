



using day7;

var total = File.ReadLines("input.txt").Select(line =>
{
    var linePart = line.Split(' ');
    return new Hand(linePart[0], int.Parse(linePart[1]));
}).Order()
.Select((hand, index) => (hand, index))
.Aggregate(
    0, (agg, elem) => agg + (elem.index +1)* elem.hand.Bid
);

Console.WriteLine($"part1 total {total}");


 var listSorted  = File.ReadLines("input.txt").Select(line =>
{
    var linePart = line.Split(' ');
    return new Hand2(linePart[0], int.Parse(linePart[1]));
}).Order()
.Select((hand, index) => (hand, index));
var total2 = listSorted.Aggregate(
    0, (agg, elem) => agg + ((elem.index + 1) * elem.hand.Bid)
);

foreach (var (hand, index) in listSorted)
{
    Console.WriteLine($"{index}\t{hand.HandString}");
}

Console.WriteLine($"part2 total {total2}");
