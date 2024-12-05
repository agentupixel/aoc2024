namespace AdventOfCode2024
{
    public class AdventDay5 : IAdventDay
    {
        public async Task<object> ExecuteTask1()
        {
            await GetInput(out List<Tuple<int, int>> pages, out List<List<int>> results);

            var counter = 0;
            List<int> middleNumbers = [];
            foreach (var result in results)
            {
                var matchingPages = pages.Where(p => result.Contains(p.Item2) && result.Contains(p.Item1)).ToList();
                bool containsInvalidPages = false;
                foreach (var page in matchingPages)
                {
                    if (result.IndexOf(page.Item1) > result.IndexOf(page.Item2))
                    {
                        containsInvalidPages = true;
                        break;
                    }
                }

                if (!containsInvalidPages)
                {
                    middleNumbers.Add(result[result.Count / 2]);
                    counter++;
                }
            }

            return middleNumbers.Sum();
        }

        public async Task<object> ExecuteTask2()
        {
            await GetInput(out List<Tuple<int, int>> pages, out List<List<int>> results);

            var counter = 0;
            List<List<int>> brokenResults = [];
            foreach (var result in results)
            {
                var matchingPages = pages.Where(p => result.Contains(p.Item2) && result.Contains(p.Item1)).ToList();
                bool containsInvalidPages = false;
                foreach (var page in matchingPages)
                {
                    if (result.IndexOf(page.Item1) > result.IndexOf(page.Item2))
                    {
                        containsInvalidPages = true;
                        break;
                    }
                }

                if (containsInvalidPages)
                {
                    brokenResults.Add(result);
                    counter++;
                }
            }

            List<int> fixedNumbers = [];

            for (int i = 0; i < brokenResults.Count; i++)
            {
                List<int> result = brokenResults[i];
                var matchingPages = pages.Where(p => result.Contains(p.Item2) && result.Contains(p.Item1)).ToList();
                bool containsInvalidPages = true;
                while (containsInvalidPages)
                {
                    bool anyPageInvalid = false;
                    foreach (var page in matchingPages)
                    {
                        var indexof1 = result.IndexOf(page.Item1);
                        var indexof2 = result.IndexOf(page.Item2);
                        if (result.IndexOf(page.Item1) > result.IndexOf(page.Item2))
                        {
                            brokenResults[i][indexof1] = page.Item2;
                            brokenResults[i][indexof2] = page.Item1;
                            anyPageInvalid = true;
                        }
                    }
                    containsInvalidPages = anyPageInvalid;
                }

                fixedNumbers.Add(result[result.Count / 2]);
            }

            return fixedNumbers.Sum();
        }

        private static async Task GetInput(out List<Tuple<int, int>> pages, out List<List<int>> results)
        {
            string[] rawInput = await File.ReadAllLinesAsync("C:\\repos\\days\\day5.txt");
            var indexOfSplit = Array.IndexOf(rawInput, string.Empty);
            pages = [];
            for (int i = 0; i < indexOfSplit; i++)
            {
                var split = rawInput[i].Split('|');
                pages.Add(new Tuple<int, int>(int.Parse(split[0]), int.Parse(split[1])));
            }

            results = [];
            for (int i = indexOfSplit + 1; i < rawInput.Length; i++)
            {
                var split = rawInput[i].Split(',');
                results.Add(split.Select(int.Parse).ToList());
            }
        }
    }
}
