
var sum = File.ReadLines("input.txt").Select(
    line =>
    {
        var first = line.First(char.IsDigit);
        var last = line.Last(char.IsDigit);
        return int.Parse($"{first}{last}");
    }).Sum();

Console.WriteLine($"Part 1 {sum}");

var digits = new Dictionary<string, int> {
    { "zero",0 },
    { "one",1 },
    { "two",2},
    { "three",3},
    { "four",4},
    { "five",5},
    { "six",6},
    { "seven",7},
    { "eight",8},
    { "nine",9},
    { "0",0},
    { "1",1},
    { "2",2},
    { "3",3},
    { "4",4},
    { "5",5},
    { "6",6},
    { "7",7},
    { "8",8},
    { "9",9},
};

var sum2 = File.ReadLines("input.txt").Select(
    line =>
    {
        var digitpos = digits.SelectMany(
             (elem) => line.AllIndexOf(elem.Key).Select(pos => (pos, elem.Value))
            );
        var first = digitpos.MinBy(e => e.pos).Value;
        var last = digitpos.MaxBy(e => e.pos).Value;
        return (first * 10) + last;
    }).Sum();

Console.WriteLine($"Part 2 {sum2}");