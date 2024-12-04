namespace AdventOfCode2024
{
    internal class AdventDay4 : IAdventDay
    {
        public async Task<object> ExecuteTask1()
        {
            string[] rawInput = await File.ReadAllLinesAsync("C:\\repos\\days\\day4.txt");
            var count = 0;
            var coordinates = new List<Tuple<int, int>>();
            for (int i = 0; i < rawInput.Length; i++)
            {
                for (int j = 0; j < rawInput[i].Length; j++)
                {
                    if (rawInput[i][j] != 'X')
                        continue;

                    if (i > 2 && j > 2)
                    {
                        if (CheckLeftUp(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (i < rawInput.Length - 3 && j > 2)
                    {
                        if (CheckLeftDown(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (i < rawInput.Length - 3 && j < rawInput[i].Length - 3)
                    {
                        if (CheckRightDown(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (i > 2 && j < rawInput[i].Length - 3)
                    {
                        if (CheckRightUp(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (i > 2)
                    {
                        if (CheckUp(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (i < rawInput.Length - 3)
                    {
                        if (CheckDown(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (j > 2)
                    {
                        if (CheckLeft(i, j, rawInput))
                        {
                            count++;
                        }
                    }

                    if (j < rawInput[i].Length - 3)
                    {
                        if (CheckRight(i, j, rawInput))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static bool CheckRight(int i, int j, string[] rawInput) => rawInput[i][j + 1] == 'M' && rawInput[i][j + 2] == 'A' && rawInput[i][j + 3] == 'S';

        private static bool CheckLeft(int i, int j, string[] rawInput) => rawInput[i][j - 1] == 'M' && rawInput[i][j - 2] == 'A' && rawInput[i][j - 3] == 'S';

        private static bool CheckUp(int i, int j, string[] rawInput) => rawInput[i - 1][j] == 'M' && rawInput[i - 2][j] == 'A' && rawInput[i - 3][j] == 'S';

        private static bool CheckDown(int i, int j, string[] rawInput) => rawInput[i + 1][j] == 'M' && rawInput[i + 2][j] == 'A' && rawInput[i + 3][j] == 'S';

        private static bool CheckLeftUp(int i, int j, string[] rawInput) => rawInput[i - 1][j - 1] == 'M' && rawInput[i - 2][j - 2] == 'A' && rawInput[i - 3][j - 3] == 'S';

        private static bool CheckLeftDown(int i, int j, string[] rawInput) => rawInput[i + 1][j - 1] == 'M' && rawInput[i + 2][j - 2] == 'A' && rawInput[i + 3][j - 3] == 'S';

        private static bool CheckRightUp(int i, int j, string[] rawInput) => rawInput[i - 1][j + 1] == 'M' && rawInput[i - 2][j + 2] == 'A' && rawInput[i - 3][j + 3] == 'S';

        private static bool CheckRightDown(int i, int j, string[] rawInput) => rawInput[i + 1][j + 1] == 'M' && rawInput[i + 2][j + 2] == 'A' && rawInput[i + 3][j + 3] == 'S';

        public async Task<object> ExecuteTask2()
        {
            string[] rawInput = await File.ReadAllLinesAsync("C:\\repos\\days\\day4.txt");
            var count = 0;
            for (int i = 1; i < rawInput.Length - 1; i++)
            {
                for (int j = 1; j < rawInput[i].Length - 1; j++)
                {
                    if (rawInput[i][j] != 'A')
                        continue;

                    if ((rawInput[i - 1][j - 1] == 'S' && rawInput[i + 1][j + 1] == 'M' && rawInput[i - 1][j + 1] == 'S' && rawInput[i + 1][j - 1] == 'M')
                        || (rawInput[i - 1][j - 1] == 'M' && rawInput[i + 1][j + 1] == 'S' && rawInput[i - 1][j + 1] == 'M' && rawInput[i + 1][j - 1] == 'S')
                        || (rawInput[i - 1][j - 1] == 'S' && rawInput[i + 1][j + 1] == 'M' && rawInput[i - 1][j + 1] == 'M' && rawInput[i + 1][j - 1] == 'S')
                        || (rawInput[i - 1][j - 1] == 'M' && rawInput[i + 1][j + 1] == 'S' && rawInput[i - 1][j + 1] == 'S' && rawInput[i + 1][j - 1] == 'M')
                        )
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
