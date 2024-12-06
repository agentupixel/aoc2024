using System.Text.RegularExpressions;

namespace AdventOfCode2024;

internal partial class AdventDay3 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string rawInput = await File.ReadAllTextAsync("C:\\repos\\days\\day3.txt");
        Regex regex = MulRegex();
        var matches = regex.Matches(rawInput).Select(m => m.Value.Replace("mul(", string.Empty).Replace(")", string.Empty));
        var sum = 0;
        foreach (var match in matches)
        {
            var split = match.Split(',');
            sum += int.Parse(split[0]) * int.Parse(split[1]);
        }

        return sum;
    }

    public async Task<object> ExecuteTask2()
    {
        string rawInput = await File.ReadAllTextAsync("C:\\repos\\days\\day3.txt");
        Regex mulRegex = MulRegex();
        var mulMatches = mulRegex.Matches(rawInput);
        var doMatches = DoRegex().Matches(rawInput);
        var dontMatches = DontRegex().Matches(rawInput);
        var doIndexes = doMatches.Select(m => m.Index).ToList();
        var dontIndexes = dontMatches.Select(m => m.Index).ToList();
        var sum = 0;
        foreach (Match match in mulMatches)
        {
            var maxDoIndex = doIndexes.Where(i => i < match.Index).OrderBy(x => x).LastOrDefault();
            var maxDontIndex = dontIndexes.Where(i => i < match.Index).OrderBy(x => x).LastOrDefault();

            if (maxDontIndex > maxDoIndex)
            {
                continue;
            }

            var stringMatch = match.Value.Replace("mul(", string.Empty).Replace(")", string.Empty);
            var split = stringMatch.Split(',');
            sum += int.Parse(split[0]) * int.Parse(split[1]);
        }

        return sum;
    }

    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex("do\\(\\)")]
    private static partial Regex DoRegex();

    [GeneratedRegex("don't\\(\\)")]
    private static partial Regex DontRegex();
}
