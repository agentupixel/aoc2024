using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2024;

internal class AdventDay8 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day8.txt");
        List<Pair> allPairs = GetInput(input);

        List<Antenna> antinodes = [];
        foreach (var pair in allPairs)
        {
            var xDistance = pair.Second.X - pair.First.X;
            var yDistance = pair.Second.Y - pair.First.Y;
            var antinode1 = new Antenna(pair.First.Frequency, pair.First.X - xDistance, pair.First.Y - yDistance);
            var antinode2 = new Antenna(pair.First.Frequency, pair.Second.X + xDistance, pair.Second.Y + yDistance);
            antinodes.Add(antinode1);
            antinodes.Add(antinode2);
        }

        var distinctLocations = antinodes
            .Where(a => a.X >= 0 && a.Y >= 0 && a.X < input[0].Length && a.Y < input.Length)
            .DistinctBy(a => new { a.X, a.Y });
        return distinctLocations.Count();
    }

    private static List<Pair> GetInput(string[] input)
    {
        List<Antenna> antennas = GetAllAntennas(input);
        var groupedAntennas = antennas.GroupBy(a => a.Frequency);
        List<Pair> allPairs = [];
        foreach (var group in groupedAntennas)
        {
            List<Pair> pairs = [];
            for (int i = 0; i < group.Count(); i++)
            {
                for (int j = 0; j < group.Count(); j++)
                {
                    if (j == i)
                    {
                        continue;
                    }

                    Pair pair = new(group.ElementAt(i), group.ElementAt(j));
                    if (!pairs.Contains(pair))
                    {
                        pairs.Add(pair);
                    }
                }
            }

            allPairs.AddRange(pairs);
        }

        return allPairs;
    }

    public async Task<object> ExecuteTask2()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day8.txt");
        List<Pair> allPairs = GetInput(input);

        List<Antenna> antinodes = [];
        foreach (var pair in allPairs)
        {
            antinodes.Add(pair.First);
            antinodes.Add(pair.Second);
            var xDistance = pair.Second.X - pair.First.X;
            var yDistance = pair.Second.Y - pair.First.Y;
            var antinode1 = new Antenna(pair.First.Frequency, pair.First.X - xDistance, pair.First.Y - yDistance);
            antinodes.Add(antinode1);
            while (antinode1.X >= 0 && antinode1.Y >= 0 && antinode1.X < input[0].Length && antinode1.Y < input.Length)
            {
                antinode1 = new Antenna(antinode1.Frequency, antinode1.X - xDistance, antinode1.Y - yDistance);
                antinodes.Add(antinode1);
            }
            var antinode2 = new Antenna(pair.First.Frequency, pair.Second.X + xDistance, pair.Second.Y + yDistance);
            antinodes.Add(antinode2);

            while (antinode2.X >= 0 && antinode2.Y >= 0 && antinode2.X < input[0].Length && antinode2.Y < input.Length)
            {
                antinode2 = new Antenna(antinode2.Frequency, antinode2.X + xDistance, antinode2.Y + yDistance);
                antinodes.Add(antinode2);
            }
        }

        var distinctLocations = antinodes
            .Where(a => a.X >= 0 && a.Y >= 0 && a.X < input[0].Length && a.Y < input.Length)
            .DistinctBy(a => new { a.X, a.Y });
        return distinctLocations.Count();
    }

    private static List<Antenna> GetAllAntennas(string[] input)
    {
        List<Antenna> result = [];

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] != '.' && input[i][j] != '#')
                {
                    result.Add(new Antenna(input[i][j], j, i));
                }
            }
        }

        return result;
    }



    private readonly struct Antenna(char frequency, int x, int y)
    {
        public char Frequency { get; } = frequency;
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private readonly struct Pair(AdventDay8.Antenna first, AdventDay8.Antenna second)
    {
        public Antenna First { get; } = first;
        public Antenna Second { get; } = second;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is Pair other)
            {
                return First.Equals(other.First) && Second.Equals(other.Second) || First.Equals(other.Second) && Second.Equals(other.First);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return First.GetHashCode() + Second.GetHashCode();
        }
    }
}
