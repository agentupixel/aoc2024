namespace AdventOfCode2024;

internal class AdventDay11 : IAdventDay
{
    private readonly long[] _dividers =
    [
        1, 10, 100, 1000, 10000, 100000, 1_000_000, 10_000_000, 100_000_000, 1_000_000_000, 10_000_000_000, 100_000_000_000, 1_000_000_000_000, 10_000_000_000_000, 100_000_000_000_000, 1_000_000_000_000_000, 10_000_000_000_000_000, 100_000_000_000_000_000, 1_000_000_000_000_000_000
    ];

    private readonly Dictionary<long, Dictionary<int, long>> dictionary = [];

    public AdventDay11()
    {
        for (int i = 0; i < 100; i++)
        {
            dictionary[i] = [];
            for (int j = 0; j < 35; j++)
            {
                var result = Execute(j, i.ToString());
                dictionary[i][j + 1] = result.First();
            }
        }
    }

    public async Task<object> ExecuteTask1()
    {
        int iterations = 25;
        string input = await File.ReadAllTextAsync("C:\\repos\\days\\day11.txt");
        var result = Execute(iterations, input);
        return result.Sum();
    }

    public async Task<object> ExecuteTask2()
    {
        int iterations = 75;
        string input = await File.ReadAllTextAsync("C:\\repos\\days\\day11.txt");
        var result = Execute(iterations, input);

        return result.Sum();
    }

    private long[] Execute(int iterations, string input)
    {
        string[] values = input.Split(' ');
        List<long> allStones = new(values.Length);
        for (int i = 0; i < values.Length; i++)
        {
            var stone = long.Parse(values[i]);
            allStones.Add(stone);
        }

        long[] stonesCounters = new long[allStones.Count];
        for (int i = 0; i < allStones.Count; i++)
        {
            int counter = 0;
            stonesCounters[i] = ProcessStone(allStones[i], counter, iterations);
        }

        return stonesCounters;
    }

    private long ProcessStone(long value, int counter, int iterations)
    {
        if (dictionary.TryGetValue(value, out Dictionary<int, long>? item) && item.ContainsKey(iterations - counter + 1))
        {
            return item[iterations - counter + 1];
        }
        if (counter >= iterations)
        {
            return 1;
        }
        long numberOfStones = 0;
        var stone = value;
        if (stone == 0)
        {
            long newStone = 1;
            numberOfStones += ProcessStone(newStone, counter + 1, iterations);
        }
        else
        {
            int numberOfDigits = 0;
            long dividedVal = stone;

            while (dividedVal != 0)
            {
                dividedVal /= 10;
                numberOfDigits++;
            }

            if (numberOfDigits % 2 != 1)
            {
                long divider = _dividers[numberOfDigits / 2];
                numberOfStones += ProcessStone(stone / divider, counter + 1, iterations);
                numberOfStones += ProcessStone(stone % divider, counter + 1, iterations);
            }
            else
            {
                long newStone = stone * 2024;
                numberOfStones += ProcessStone(newStone, counter + 1, iterations);
            }
        }

        return numberOfStones;
    }
}
