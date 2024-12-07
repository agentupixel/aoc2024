namespace AdventOfCode2024;

internal class AdventDay7 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        var input = await File.ReadAllLinesAsync("C:\\repos\\days\\day7.txt");
        char[] chars = ['+', '*'];
        var rows = GetEquations(input);
        var validOnes = rows.Where(r => IsValidEquation(r, chars)).ToArray();

        return validOnes.Sum(r => r.Result);
    }

    private static bool IsValidEquation(Equation equation, char[] chars)
    {
        List<string> matrix = [];
        GeneratePermutations("", equation.Partials.Length - 1, chars, matrix);
        foreach (var permutation in matrix)
        {
            var result = equation.Partials[0];
            for (int i = 0; i < equation.Partials.Length - 1; i++)
            {
                switch (permutation[i])
                {
                    case '+':
                        result += equation.Partials[i + 1];
                        break;
                    case '*':
                        result *= equation.Partials[i + 1];
                        break;
                    case '|':
                        result = long.Parse(result.ToString() + equation.Partials[i + 1].ToString());
                        break;
                }
            }

            if (result == equation.Result)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<object> ExecuteTask2()
    {
        var input = await File.ReadAllLinesAsync("C:\\repos\\days\\day7.txt");
        char[] chars = ['+', '*', '|'];
        var rows = GetEquations(input);
        var validOnes = rows.Where(r => IsValidEquation(r, chars)).ToArray();

        return validOnes.Sum(r => r.Result);
    }

    static void GeneratePermutations(string prefix, int x, char[] chars, List<string> permutations)
    {
        if (x == 0)
        {
            permutations.Add(prefix);
            return;
        }

        foreach (var c in chars)
        {
            GeneratePermutations(prefix + c, x - 1, chars, permutations);
        }
    }

    private struct Equation(long result, long[] partials)
    {
        public long Result = result;
        public long[] Partials = partials;
    }

    private static Equation[] GetEquations(string[] input)
    {
        Equation[] result = new Equation[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            string? row = input[i];
            var initialSplit = row.Split(':');
            var valuesSplit = initialSplit[1].Trim().Split(' ');
            result[i] = new(long.Parse(initialSplit[0]), valuesSplit.Select(long.Parse).ToArray());
        }

        return result;
    }
}
