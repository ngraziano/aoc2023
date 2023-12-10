
using System.Numerics;

var fileData = File.ReadLines("input.txt");

var instruction = fileData.First();

var mapping = fileData.Skip(2).Select(line =>
{
    var key = line[0..3];
    var left = line[7..10];
    var right = line[12..15];
    return (key, left, right);

}).ToDictionary((e) => e.key, e => (e.left, e.right));


int nbStep = 0;
var pos = "AAA";

var nbInstruction = instruction.Length;

while (pos != "ZZZ")
{
    if (instruction[nbStep % nbInstruction] == 'L')
    {
        pos = mapping[pos].left;
    }
    else
    {
        pos = mapping[pos].right;
    }
    nbStep++;
}
Console.WriteLine($"Part1 step {nbStep}");


var multiplePositons = mapping.Keys.Where(k => k[2] == 'A').ToList();
// Naive
#if false
var nbMultipleSteps = 0;
while (!multiplePositons.All(k => k[2] =='Z'))
{
    var goLeft = instruction[nbMultipleSteps % nbInstruction] == 'L';

    if (goLeft)
    {
        multiplePositons = multiplePositons.ConvertAll(pos => mapping[pos].left);
    } else
    {
        multiplePositons = multiplePositons.ConvertAll(pos => mapping[pos].right);
    }
    if (multiplePositons.Any(k => k[2]=='Z'))
    {
        Console.WriteLine($"{nbMultipleSteps} => " + string.Join(", ", multiplePositons));
    }


    nbMultipleSteps++;

}
Console.WriteLine($"Part 2 step {nbMultipleSteps}");
#endif


var data = multiplePositons.ConvertAll(pos =>
{
    int nbStep = 0;

    void moveNext()
    {
        if (instruction[nbStep % nbInstruction] == 'L')
        {
            pos = mapping[pos].left;
        }
        else
        {
            pos = mapping[pos].right;
        }
        nbStep++;
    }

    while (pos[2] != 'Z')
    {
        moveNext();
    }

    var initialSteps = nbStep;
    moveNext();
    while (pos[2] != 'Z')
    {
        moveNext();
    }
    var cyle = nbStep - initialSteps;
    return (initialSteps, cyle);
});

foreach (var e in data)
{
    Console.WriteLine(e);
}

static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b) => (a / BigInteger.GreatestCommonDivisor(a, b)) * b;

var thelcm = data.Select(e => new BigInteger(e.cyle)).Aggregate(new BigInteger(1), LeastCommonMultiple);

Console.WriteLine($"Part 2 {thelcm}");