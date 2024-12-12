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
                var result = Execute3(j, i.ToString());
                dictionary[i][j + 1] = result.First();
            }
        }
    }

    public async Task<object> ExecuteTask1()
    {
        int iterations = 25;
        string input = await File.ReadAllTextAsync("C:\\repos\\days\\day11.txt");
        //Console.WriteLine(await Execute(iterations));
        Console.WriteLine(Execute2(iterations, input));
        var result = Execute4(iterations, input);
        return result.Sum();
    }

    public async Task<object> ExecuteTask2()
    {
        int iterations = 75;
        //string input = await File.ReadAllTextAsync("C:\\repos\\days\\day11.txt");
        //Dictionary<long, Dictionary<int, long>> dictionary = [];
        for (int i = 0; i < 100; i++)
        {
            dictionary[i] = [];
            for (int j = 0; j < 35; j++)
            {
                var result = Execute3(j, i.ToString());
                dictionary[i][j + 1] = result.First();
            }
        }
        string input = await File.ReadAllTextAsync("C:\\repos\\days\\day11.txt");
        var result2 = Execute3(iterations, input);

        return result2;
    }

    private object Execute2(int iterations, string input)
    {
        string[] values = input.Split(' ');
        List<long> allStones = new(values.Length);
        for (int i = 0; i < values.Length; i++)
        {
            long stone = long.Parse(values[i]);
            allStones.Add(stone);
        }

        long[] stonesCounters = new long[allStones.Count];
        for (int i = 0; i < allStones.Count; i++)
        {
            int counter = 0;
            List<long> stones = [allStones[i]];
            List<long> tempStones = new(stones.Count * 2);
            while (counter < iterations)
            {
                counter++;
                for (int j = 0; j < stones.Count; j++)
                {
                    long stone = stones[j];
                    if (stone == 0)
                    {
                        long newStone = 1;
                        tempStones.Add(newStone);
                    }
                    else
                    {
                        int numberOfDigits = 0;
                        long value = stone;

                        while (value != 0)
                        {
                            value /= 10;
                            numberOfDigits++;
                        }

                        if (numberOfDigits % 2 != 1)
                        {
                            long divider = _dividers[numberOfDigits / 2];
                            tempStones.Add(stone / divider);
                            tempStones.Add(stone % divider);
                        }
                        else
                        {
                            long newStone = stone * 2024;
                            tempStones.Add(newStone);
                        }
                    }
                }

                stones = tempStones;
                tempStones = new(stones.Count);
            }

            stonesCounters[i] = stones.Count;
            Console.WriteLine(stones.Count);
        }

        return stonesCounters.Sum();
    }

    private IEnumerable<long> Execute3(int iterations, string input)
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
            List<long> stones = [allStones[i]];
            List<long> tempStones = new(stones.Count * 2);
            while (counter < 40 && counter < iterations)
            {
                counter++;
                for (int j = 0; j < stones.Count; j++)
                {
                    long stone = stones[j];
                    if (stone == 0)
                    {
                        long newStone = 1;
                        tempStones.Add(newStone);
                    }
                    else
                    {
                        int numberOfDigits = 0;
                        long value = stone;

                        while (value != 0)
                        {
                            value /= 10;
                            numberOfDigits++;
                        }

                        if (numberOfDigits % 2 != 1)
                        {
                            long divider = _dividers[numberOfDigits / 2];
                            tempStones.Add(stone / divider);
                            tempStones.Add(stone % divider);
                        }
                        else
                        {
                            long newStone = stone * 2024;
                            tempStones.Add(newStone);
                        }
                    }
                }

                stones = tempStones;
                tempStones = new(stones.Count);
            }

            stonesCounters[i] = stones.Count;
            if (counter < iterations)
            {
                Console.WriteLine("recursive processing");
                stonesCounters[i] = stones.AsParallel().Sum(stone => ProcessStone(stone, counter, iterations));
            }

            //Console.WriteLine(DateTime.Now);
            //Console.WriteLine(stonesCounters[i]);
        }

        return stonesCounters;
    }

    private IEnumerable<long> Execute4(int iterations, string input)
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
            List<long> stones = [allStones[i]];
            Console.WriteLine("recursive processing");
            stonesCounters[i] = stones.AsParallel().Sum(stone => ProcessStone(stone, counter, iterations));

            //Console.WriteLine(DateTime.Now);
            //Console.WriteLine(stonesCounters[i]);
        }

        return stonesCounters;
    }

    private long ProcessStone(long value, int counter, int iterations)
    {
        if (dictionary.ContainsKey(value) && dictionary[value].ContainsKey(iterations - counter))
        {
            return dictionary[value][iterations - counter];
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

    /*
     If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1.
If the stone is engraved with a number that has an even number of digits, it is replaced by two stones. The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone. (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.)
If none of the other rules apply, the stone is replaced by a new stone; the old stone's number multiplied by 2024 is engraved on the new stone.
     */
}
