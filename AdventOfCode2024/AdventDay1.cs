namespace AdventOfCode2024;

internal class AdventDay1 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string[] rawInput = await File.ReadAllLinesAsync("C:\\repos\\days\\day1.txt");
        int[] left = new int[rawInput.Length];
        int[] right = new int[rawInput.Length];
        for (int i = 0; i < rawInput.Length; i++)
        {
            string row = rawInput[i];
            string[] values = row.Split("   ");
            left[i] = int.Parse(values[0].Trim());
            right[i] = int.Parse(values[1].Trim());
        }

        var orderedLeft = left.OrderBy(x => x).ToArray();
        var orderedRight = right.OrderBy(x => x).ToArray();
        var distances = new int[rawInput.Length];
        for (int i = 0; i < left.Length; i++)
        {
            distances[i] = Math.Abs(orderedRight[i] - orderedLeft[i]);
        }

        return distances.Sum();
    }

    public async Task<object> ExecuteTask2()
    {
        string[] rawInput = await File.ReadAllLinesAsync("C:\\repos\\days\\day1.txt");
        int[] left = new int[rawInput.Length];
        int[] right = new int[rawInput.Length];
        for (int i = 0; i < rawInput.Length; i++)
        {
            string row = rawInput[i];
            string[] values = row.Split("   ");
            left[i] = int.Parse(values[0].Trim());
            right[i] = int.Parse(values[1].Trim());
        }

        var groupedLeft = left.GroupBy(x => x);
        var groupedRight = right.GroupBy(x => x);
        List<long> similarityScores = [];

        foreach (var leftGroup in groupedLeft)
        {
            var rightGroup = groupedRight.FirstOrDefault(r => r.Key == leftGroup.Key);
            if (rightGroup is null)
            {
                continue;
            }

            similarityScores.Add(leftGroup.Key * rightGroup.Count());
        }

        return similarityScores.Sum();
    }
}
