
using System.Collections.Generic;
using System.Linq;
using Link = (string source, string dest);

var links = File.ReadLines("input.txt").Select(line => line.Split(": ") switch
{
    [var source, var dests] => (source, dests.Split(' ')),
    _ => throw new InvalidDataException(),
}).SelectMany(e =>

    e.Item2.Select(dest => (e.source, dest))
 ).Select(e => ToUnique(e.source, e.dest))
 .OrderBy(e => e.source)
 .ToList();

Dictionary<string, List<string>> linksDic = [];


foreach (var (source, dest) in links)
{
    if (!linksDic.TryGetValue(source, out var list))
    {
        list = [];
        linksDic[source] = list;
    }
    list.Add(dest);


    if (!linksDic.TryGetValue(dest, out list))
    {
        list = [];
        linksDic[dest] = list;
    }
    list.Add(source);
}

Dictionary<Link, int> weightLink = links.ToDictionary(e => e, _ => 0);


Random rnd = new();
for (int round = 0; round < 100; round++)
{
    var src = linksDic.Keys.Skip(rnd.Next(linksDic.Keys.Count)).First();
    var dest = linksDic.Keys.Skip(rnd.Next(linksDic.Keys.Count)).First();
    var searchQueue = new PriorityQueue<(string, List<string>), int>();

    searchQueue.Enqueue((src, [src]), 0);
    List<string> findedPath = [];

    while (searchQueue.Count > 0)
    {
        var (curr, path) = searchQueue.Dequeue();

        if (curr == dest)
        {
            findedPath = path;
            break;
        }

        foreach (var newCurr in linksDic[curr].Where(e => !path.Contains(e)))
        {
            var newPath = path.Append(newCurr).ToList();
            searchQueue.Enqueue((newCurr, newPath), newPath.Count);
        }

    }
    for (int i = 1; i < findedPath.Count; i++)
    {
        weightLink[ToUnique(findedPath[i - 1], findedPath[i])]+= findedPath.Count;
    }

    Console.WriteLine($"{round,5} {src}->{dest} Path => " + string.Join(" | ", weightLink.OrderByDescending(e => e.Value).Take(4).Select(e => $"{e.Key}  {e.Value,5}")));
}

var listToTest = weightLink.OrderByDescending(e => e.Value).Select(e => links.IndexOf(e.Key)).ToList();

var theTrheeList = listToTest.SelectMany(link1 =>
listToTest.Where(e => e > link1).SelectMany(link2 =>
    listToTest.Where(e => e != link1 && e != link2).Select(
        link3 =>
        {
            int maxGroup = 1;
            Dictionary<string, int> groups = [];

            foreach (var (link, idx) in links.Select((l, index) => (l, index))
            .Where(e => e.index != link1 && e.index != link2 && e.index != link3))
            {
                var f = groups.GetValueOrDefault(link.source);
                var s = groups.GetValueOrDefault(link.dest);

                if (f == 0 && s > 0)
                {
                    groups[link.source] = s;
                }
                else if (f > 0 && s == 0)
                {
                    groups[link.dest] = f;
                }
                else if (f > 0 && s > 0 && f < s)
                {
                    foreach (var (k, v) in groups)
                    {
                        if (v == s)
                        {
                            groups[k] = f;
                        }
                    }

                }
                else if (f > 0 && s > 0 && s < f)
                {
                    foreach (var (k, v) in groups)
                    {
                        if (v == f)
                        {
                            groups[k] = s;
                        }
                    }
                }
                else if (f == 0 && s == 0)
                {
                    groups[link.source] = maxGroup;
                    groups[link.dest] = maxGroup;
                    maxGroup++;
                }
            }
            if (link3 % 1000 == 0 || groups.Values.Distinct().Count() > 1)
            {
                Console.WriteLine($"{links[link1]} {links[link2]} {links[link3]} => {groups.Values.Distinct().Count()} ... {string.Join(",",groups.Values.Distinct())}");
            }
            return groups;
        }
)

    )
).First(groups => groups.Values.Distinct().Count() == 2);

var part1 = theTrheeList.Values.Distinct().Select(v => theTrheeList.Values.Count(a => a == v)).Aggregate(1, (agg, v) => agg * v);
// var part1 = theTrheeList[0].Count * theTrheeList[1].Count * theTrheeList[2].Count;

Console.WriteLine($"Part 1 {part1}");



(string source, string dest) ToUnique(string p1, string p2) => string.CompareOrdinal(p1, p2) < 0 ? (p1, p2) : (p2, p1);
