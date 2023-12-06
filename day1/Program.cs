
/* first version 
var sum = File.ReadLines("input.txt").Select(
    line =>
    {
        var isNumber = (char x) => '0' <= x && x <= '9';
        var first = line.First(isNumber);
        var last = line.Last(isNumber);
        return int.Parse($"{first}{last}");

    }).Sum();
*/



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


var sum = File.ReadLines("input.txt").Select(
    line =>
    {
        var digitpos = digits.SelectMany(
             (elem) => line.AllIndexOf(elem.Key).Select(pos => new { pos, val = elem.Value })
            );
        var first = digitpos.MinBy(e => e.pos)?.val ?? 0;
        var last = digitpos.MaxBy(e => e.pos)?.val ?? 0;
        return (first * 10) + last;
    }).Sum();


Console.WriteLine(sum);