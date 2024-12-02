namespace AdventOfCode2024;

internal class AdventDay2 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        return await ProcessDay2(false);
    }

    public async Task<object> ExecuteTask2()
    {
        return await ProcessDay2(true);
    }

    private static bool IsValid(int[] report, int distance, bool useProblemDampener)
    {
        bool? isAscending = null;
        for (int i = 0; i < report.Length; i++)
        {
            if (isAscending == true && report[i] <= report[i - 1])
            {
                if (!HandleWrongValue(report, useProblemDampener))
                {
                    return false;
                }

                continue;
            }

            if (isAscending == false && report[i] >= report[i - 1])
            {
                if (!HandleWrongValue(report, useProblemDampener))
                {
                    return false;
                }

                continue;
            }

            if (i == 1)
            {
                if (report[i] > report[i - 1])
                {
                    isAscending = true;
                }
                else
                if (report[i] < report[i - 1])
                {
                    isAscending = false;
                }
                else
                {
                    if (!HandleWrongValue(report, useProblemDampener))
                    {
                        return false;
                    }

                    continue;
                }
            }

            if (i > 0 && Math.Abs(report[i] - report[i - 1]) > distance)
            {
                if (!HandleWrongValue(report, useProblemDampener))
                {
                    return false;
                }

                continue;
            }
        }

        return true;
    }

    private static bool HandleWrongValue(int[] report, bool useProblemDampener)
    {
        if (!useProblemDampener)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < report.Length; i++)
            {
                var modifiedReport = report.ToList();
                modifiedReport.RemoveAt(i);
                if (IsValid(modifiedReport.ToArray(), 3, false))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static async Task<object> ProcessDay2(bool shouldUseProblemDampener)
    {
        string[] rawInput = await File.ReadAllLinesAsync("C:\\repos\\days\\day2.txt");
        int[][] reports = new int[rawInput.Length][];
        for (int i = 0; i < rawInput.Length; i++)
        {
            string row = rawInput[i];
            string[] values = row.Split(' ');
            reports[i] = values.Select(int.Parse).ToArray();
        }

        int safeReports = 0;
        foreach (int[] report in reports)
        {
            if (IsValid(report, 3, shouldUseProblemDampener))
            {
                safeReports++;
            }
        }

        return safeReports;
    }
}
