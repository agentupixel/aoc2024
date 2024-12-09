namespace AdventOfCode2024;

internal class AdventDay9 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string input = await File.ReadAllTextAsync("C:\\repos\\days\\day9.txt");
        List<int> discMap = new(input.Length);

        int idCounter = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 != 1)
            {
                var length = int.Parse(input[i].ToString());
                for (int j = 0; j < length; j++)
                {
                    discMap.Add(idCounter);
                }
            }
            else
            {

                var length = int.Parse(input[i].ToString());
                for (int j = 0; j < length; j++)
                {
                    discMap.Add(-1);
                }
                idCounter++;
            }
        }

        var emptySpaceIndexOf = discMap.IndexOf(-1);
        var position = discMap.FindLastIndex(x => x != -1);

        while (emptySpaceIndexOf < position)
        {
            position = discMap.FindLastIndex(x => x != -1);
            var value = discMap[position];
            discMap[emptySpaceIndexOf] = value;
            discMap[position] = -1;
            emptySpaceIndexOf = discMap.IndexOf(-1);
        }

        long checksum = 0;

        for (int i = 0; i < discMap.Count; i++)
        {
            if (discMap[i] == -1)
            {
                continue;
            }

            checksum += i * discMap[i];
        }

        return checksum;
    }

    public async Task<object> ExecuteTask2()
    {
        string input = await File.ReadAllTextAsync("C:\\repos\\days\\day9.txt");
        List<int> discMap = new(input.Length);
        Dictionary<int, int> discFiles = [];
        Dictionary<int, int> emptySpaces = [];
        int idCounter = 0;
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 != 1)
            {
                var length = int.Parse(input[i].ToString());
                for (int j = 0; j < length; j++)
                {
                    discMap.Add(idCounter);
                }
                discFiles[idCounter] = length;
            }
            else
            {
                var length = int.Parse(input[i].ToString());
                emptySpaces[discMap.Count] = length;
                for (int j = 0; j < length; j++)
                {
                    discMap.Add(-1);
                }

                idCounter++;
            }
        }

        var processableFiles = discFiles.OrderBy(x => x.Key).ToList();
        for (int i = processableFiles.Count - 1; i >= 0; i--)
        {
            var length = processableFiles[i].Value;
            var previousPosition = discMap.IndexOf(processableFiles[i].Key);
            var matchingEmptySpaces = emptySpaces.Where(es => es.Value >= length);
            if (!matchingEmptySpaces.Any())
            {
                continue;
            }
            var firstEmptySpace = matchingEmptySpaces.OrderBy(es => es.Key).FirstOrDefault();
            if (firstEmptySpace.Key > previousPosition)
            {
                continue;
            }
            for (int j = 0; j < length; j++)
            {
                discMap[firstEmptySpace.Key + j] = processableFiles[i].Key;
            }

            for (int j = 0; j < length; j++)
            {
                discMap[previousPosition + j] = -1;
            }

            emptySpaces = [];
            List<EmptySpace> tempEmptySpaces = [];
            for (int j = 0; j < discMap.Count; j++)
            {
                if (discMap[j] == -1)
                {
                    if (j > 0 && discMap[j - 1] == -1)
                    {
                        tempEmptySpaces.Last().Length++;
                    }
                    else
                    {
                        tempEmptySpaces.Add(new EmptySpace(j, 1));
                    }
                }
            }

            emptySpaces = new Dictionary<int, int>(tempEmptySpaces.Select(es => new KeyValuePair<int, int>(es.StartPosition, es.Length)));
        }

        long checksum = 0;
        var emptySpaceIndexOf = discMap.IndexOf(-1);
        var position = discMap.FindLastIndex(x => x != -1);

        for (int i = 0; i < discMap.Count; i++)
        {
            if (discMap[i] == -1)
            {
                continue;
            }

            checksum += i * discMap[i];
        }

        return checksum;
    }

    public class EmptySpace(int startPosition, int length)
    {
        public int StartPosition { get; } = startPosition;
        public int Length { get; set; } = length;
    }
    private readonly struct DiscFile(int id, int length)
    {
        public int Id { get; } = id;
        public int Length { get; } = length;
    }
}
