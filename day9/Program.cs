

var result =
    File.ReadLines("input.txt").Select(line =>
    {
        var sensorValues = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();

        var nextValue = 0;
        while (!sensorValues.All(v => v == 0) && sensorValues.Count > 1)
        {
            for (int i = 0; i < sensorValues.Count - 1; i++)
            {
                sensorValues[i] = sensorValues[i + 1] - sensorValues[i];
            }
            nextValue += sensorValues[^1];
            sensorValues.RemoveAt(sensorValues.Count - 1);
        }
        return nextValue;
    }).Sum();

Console.WriteLine($"Part 1 {result}");

var result2 =
    File.ReadLines("input.txt").Select(line =>
    {
        var sensorValues = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToList();
        var nextValue = 0;
        int mul = 1;
        while (!sensorValues.All(v => v == 0) && sensorValues.Count > 1)
        {
            nextValue += mul*sensorValues[0];
            mul *= -1;
            for (int i = 0; i < sensorValues.Count - 1; i++)
            {
                sensorValues[i] = sensorValues[i + 1] - sensorValues[i];
            }
            sensorValues.RemoveAt(sensorValues.Count - 1);
        }
        return nextValue;
    }).Sum();

Console.WriteLine($"Part 2 {result2}");