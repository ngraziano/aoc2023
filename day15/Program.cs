
using System.Reflection.Emit;
using System.Text.RegularExpressions;

static byte Hash(string input) =>
    input.Aggregate((byte)0, (hash, c) => (byte)((hash + c) * 17));

var sumOfHash = File.ReadLines("input.txt").First().Split(',')
                .Select(Hash).Select(x => (int)x).Sum();

Console.WriteLine($"Part 1 sum of hash = {sumOfHash}");

var hashmap = Enumerable.Range(0, 256).Select((_) => new List<(string Label, int Focal)>()).ToList();

var line = File.ReadLines("input.txt").First();
foreach (string intruction in line.Split(','))
{
    var (label, operation, focal) = intruction switch
    {
        [.. var l, '-'] => (l, Operation.REMOVE, 0),
        [.. var l, '=', var v] => (l, Operation.ASSIGN, v - '0'),
        _ => ("", Operation.NULL, 0)
    };

    var box = hashmap[Hash(label)];
    var slotIndex = box.FindIndex(e => e.Label == label);
    switch (operation)
    {
        case Operation.REMOVE when slotIndex >= 0:
            box.RemoveAt(slotIndex);
            break;
        case Operation.ASSIGN when slotIndex >= 0:
            box[slotIndex] = (label, focal);
            break;
        case Operation.ASSIGN:
            box.Add((label, focal));
            break;
    }
}

var sumFocus = hashmap.SelectMany((box, boxNumber) => box.Select((slot, slotNumber) => (boxNumber + 1) * (slotNumber + 1) * slot.Focal)).Sum();
Console.WriteLine($"Part 2 {sumFocus}");

enum Operation
{
    NULL,
    REMOVE,
    ASSIGN,
};