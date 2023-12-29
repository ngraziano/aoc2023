static int FindMirror(IList<string> data)
{
    return FindAllMirrors(data).First();
}

static IEnumerable<int> FindAllMirrors(IList<string> data)
{
    var nbLigne = data.Count;
    return Enumerable.Range(0, nbLigne)
        .Where(i =>
        Enumerable.Range(0, Math.Min(i + 1, nbLigne - 1 - i))
        .All(j => data[i - j] == data[i + j + 1])).Select(i => i + 1);
}

static List<string> TransposeStrings(List<string> data) =>
    Enumerable.Range(0, data[0].Length).Select(
        i => new string(data.Select(l => l[i]).ToArray())
    ).ToList();

static bool diffByOnlyOne(ReadOnlySpan<char> a, ReadOnlySpan<char> b)
{
    int nbDiff = 0;
    for (int i = 0; i < a.Length; i++)
    {
        if (a[i] != b[i])
            nbDiff++;
        if (nbDiff > 1)
            return false;
    }
    return nbDiff == 1;
}

var total = File.ReadLines("input.txt").Split(string.IsNullOrEmpty).Select(block =>
{
    var data = block.ToList();

    // check horizontal
    int hMirror = FindMirror(data);
    if (hMirror < data.Count)
    {
        return hMirror * 100;
    }
    // check vertical
    data = TransposeStrings(data);
    return FindMirror(data);
}).Sum();

Console.WriteLine($"Part1 {total}");


var totalPart2 = File.ReadLines("input.txt").Split(string.IsNullOrEmpty).Select((block, patternNb) =>
{
    var data = block.ToList();

    int hOriginalMirror = FindMirror(data);
    var newHMirrors = data[..^1].SelectMany((smudgeLine, smudgeLineIndex) =>
        data[(smudgeLineIndex + 1)..]
            .Where(copiedLine => diffByOnlyOne(smudgeLine, copiedLine))
            .Select(copiedLine => new List<string>(data) { [smudgeLineIndex] = copiedLine })
            .SelectMany(dataSmuged => FindAllMirrors(dataSmuged).Where(hMirror => hMirror < dataSmuged.Count && hMirror != hOriginalMirror))
            .Select(hMirror =>
            {
                Console.WriteLine($"({patternNb:00}) Smuged line {smudgeLineIndex} H mirror at {hMirror} original was {hOriginalMirror}");
                return 100 * hMirror;
            })
    ).Take(1);
    if (newHMirrors.Any())
    {
        return newHMirrors.First();
    }

    data = TransposeStrings(data);
    int vOriginalMirror = FindMirror(data);
    var newVMirrors = data[..^1].SelectMany((smudgeLine, smudgeLineIndex) =>
    data[(smudgeLineIndex + 1)..]
        .Where(copiedLine => diffByOnlyOne(smudgeLine, copiedLine))
        .Select(copiedLine => new List<string>(data) { [smudgeLineIndex] = copiedLine })
        .SelectMany(dataSmuged => FindAllMirrors(dataSmuged).Where(vMirror => vMirror < dataSmuged.Count && vMirror != vOriginalMirror))
        .Select(vMirror =>
        {
            Console.WriteLine($"({patternNb:00}) Smuged line {smudgeLineIndex} V mirror at {vMirror} original was {hOriginalMirror}");
            return vMirror;
        })
    ).Take(1);
    if (newVMirrors.Any())
    {
        return newVMirrors.First();
    }
    throw new Exception("No smudge finded, check the code");
}).Sum();
Console.WriteLine($"Part2 {totalPart2}");


static class SplitEnum
{
    public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> separatorFunc)
    {
        List<TSource>? items = null;
        foreach (var item in source)
        {
            if (separatorFunc(item))
            {
                yield return items ?? Enumerable.Empty<TSource>();
                items = null;
            }
            else
            {
                items ??= [];
                items.Add(item);
            }
        }

        if (items?.Count > 0)
            yield return items;
    }
}