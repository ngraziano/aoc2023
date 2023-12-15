
using System.ComponentModel;
using System.Reflection.Emit;

static byte Hash(string input)
{

    return input.Aggregate((byte)0, (hash, c) =>
    {
        int val = (hash + c) * 17;
        return (byte)val;
    });
}

var sumOfHash = File.ReadLines("input.txt").First().Split(',')
                .Select(Hash).Select(x => (int)x).Sum();

Console.WriteLine($"Part 1 sum of hash = {sumOfHash}");


var hashmap = Enumerable.Range(0, 256).Select((_) => new List<Slot>()).ToList();

var line = File.ReadLines("input.txt").First();
//var line = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
foreach (string intruction in line.Split(','))
{
    if (intruction.EndsWith('-'))
    {
        var label = intruction[..^1];
        var box = hashmap[Hash(label)];
        box.RemoveAll(e => e.Label == label);
    } else if(intruction.Contains('='))
    {
        var label = intruction.Split('=')[0];
        var focal = int.Parse(intruction.Split("=")[1]);
        var box = hashmap[Hash(label)];
        
        var place = box.Find(e => e.Label == label);
        if(place is null)
        {
            box.Add(new Slot { Label = label, Focal = focal });
        } else
        {
            place.Focal = focal;
        }
    }
}

var sumFocus = hashmap.SelectMany((box, boxNumber) => box.Select((slot, slotNumber) => (boxNumber + 1) * (slotNumber + 1) * slot.Focal)).Sum();
Console.WriteLine($"Part 2 {sumFocus}");


public record Slot
{
    public string Label { get; set; } = "";
    public int Focal { get; set; }
}