


static int FindMirror(IList<string> data)
{
    var nbLigne = data.Count;
    for (int i = 0; i < nbLigne; i++)
    {
        bool isValid = true;
        for (int j = 0; i - j >= 0 && i + j + 1 < nbLigne; j++)
        {
            if (data[i - j] != data[i + j + 1])
            {
                isValid = false;
                break;
            }
        }
        if (isValid)
            return i +1;
    }
    return nbLigne;
}

static IEnumerable<int> FindAllMirrors(IList<string> data)
{
    var nbLigne = data.Count;
    for (int i = 0; i < nbLigne; i++)
    {
        bool isValid = true;
        for (int j = 0; i - j >= 0 && i + j + 1 < nbLigne; j++)
        {
            if (data[i - j] != data[i + j + 1])
            {
                isValid = false;
                break;
            }
        }
        if (isValid)
            yield return i +1;
    }
}

static List<string> TransposeStrings(List<string> data) =>
    Enumerable.Range(0, data[0].Length).Select(
        i => new string(data.Select(l => l[i]).ToArray())
    ).ToList();

static bool diffByOnlyOne(string a, string b)
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

var lineEnumerator = File.ReadLines("input.txt").GetEnumerator();




bool moreData;

var totalH = 0;
var totalV = 0;

do
{
    moreData = lineEnumerator.MoveNext();
    List<string> data = [];
    while (!string.IsNullOrWhiteSpace(lineEnumerator.Current))
    {
        data.Add(lineEnumerator.Current);
        moreData = lineEnumerator.MoveNext();
    }

    int hMirror = FindMirror(data);
    if (hMirror < data.Count)
    {
        totalH += hMirror;
    }
    else
    {
        data = TransposeStrings(data);
        int vMirror = FindMirror(data);
        totalV += vMirror;
    }
}
while (moreData);

Console.WriteLine($"Part1 {totalH * 100 + totalV}");

lineEnumerator.Dispose();
lineEnumerator = File.ReadLines("input.txt").GetEnumerator();

totalH = 0;
totalV = 0;
int patternNb = 0;
do
{
    patternNb++;
    moreData = lineEnumerator.MoveNext();
    List<string> data = [];
    while (!string.IsNullOrWhiteSpace(lineEnumerator.Current))
    {
        data.Add(lineEnumerator.Current);
        moreData = lineEnumerator.MoveNext();
    }

    bool findSmudge = false;
    int hOriginalMirror = FindMirror(data);
    for (int smudgeLine = 0; smudgeLine < data.Count - 1 && !findSmudge; smudgeLine++)
    {
        for (int copiedLine = smudgeLine + 1; copiedLine < data.Count && !findSmudge; copiedLine++)
        {
            if (diffByOnlyOne(data[smudgeLine], data[copiedLine]))
            {
                var dataSmuged = new List<string>(data);
                dataSmuged[smudgeLine] = data[copiedLine];

                foreach (var hMirror in FindAllMirrors(dataSmuged))
                {
                    if (hMirror < dataSmuged.Count && hMirror != hOriginalMirror)
                    {
                        Console.WriteLine($"({patternNb:00}) Smuged line {smudgeLine} with line {copiedLine} H mirror at {hMirror} original was {hOriginalMirror}");
                        totalH += hMirror;
                        findSmudge = true;
                    }
                }
            }

        }
    }
    if (!findSmudge)
    {
        data = TransposeStrings(data);
        int vOriginalMirror = FindMirror(data);

        for (int smudgeLine = 0; smudgeLine < data.Count - 1 && !findSmudge; smudgeLine++)
        {
            for (int copiedLine = smudgeLine + 1; copiedLine < data.Count && !findSmudge; copiedLine++)
            {
                if (diffByOnlyOne(data[smudgeLine], data[copiedLine]))
                {
                    var dataSmuged = new List<string>(data);
                    dataSmuged[smudgeLine] = data[copiedLine];
                    foreach (var vMirror in FindAllMirrors(dataSmuged))
                    {
                        if (vMirror < dataSmuged.Count && (vMirror != vOriginalMirror))
                        {
                            Console.WriteLine($"({patternNb:00}) Smuged colums {smudgeLine} with colums {copiedLine} V mirror at {vMirror}  original was {vOriginalMirror}");
                            totalV += vMirror;
                            findSmudge = true;
                        }
                    }
                }

            }
        }
    }
    if (!findSmudge)
    {
        throw new Exception("No smudge finded, check the code");
    }
}
while (moreData);

Console.WriteLine($"Part2 {totalH * 100 + totalV}");
