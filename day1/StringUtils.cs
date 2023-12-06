

static class StringUtils
{
    public static IEnumerable<int> AllIndexOf(this string line, string valString)
    {
        int pos = -1;
        while (true)
        {
            pos = line.IndexOf(valString, pos + 1);
            if (pos < 0)
            {
                yield break;
            }
            yield return pos;
        }
    }
}
