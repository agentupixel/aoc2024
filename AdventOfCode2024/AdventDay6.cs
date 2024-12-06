namespace AdventOfCode2024;

internal class AdventDay6 : IAdventDay
{
    private readonly Position _up = new(-1, 0);
    private readonly Position _right = new(0, 1);
    private readonly Position _down = new(1, 0);
    private readonly Position _left = new(0, -1);

    private readonly List<Position> _directionsOrder;

    public AdventDay6()
    {
        _directionsOrder = [_up, _right, _down, _left];
    }

    public async Task<object> ExecuteTask1()
    {
        char[][] input = (await File.ReadAllLinesAsync("C:\\repos\\days\\day6.txt")).Select(l => l.ToArray()).ToArray();
        Position direction = _directionsOrder[0];
        var currentPosition = GetStartingPosition(input);
        List<Position> walkedCoordinates = [currentPosition];
        Position nextCoordinates = Move(currentPosition, ref direction, input);
        while (!IsLeaving(nextCoordinates, input))
        {
            currentPosition = nextCoordinates;
            walkedCoordinates.Add(currentPosition);
            nextCoordinates = Move(currentPosition, ref direction, input);
        }

        return walkedCoordinates.Distinct().Count();
    }

    public async Task<object> ExecuteTask2()
    {
        char[][] input = (await File.ReadAllLinesAsync("C:\\repos\\days\\day6.txt")).Select(l => l.ToArray()).ToArray();
        Position direction = _directionsOrder[0];
        var currentPosition = GetStartingPosition(input);
        List<Position> walkedCoordinates = [currentPosition];
        Position nextCoordinates = Move(currentPosition, ref direction, input);
        while (!IsLeaving(nextCoordinates, input))
        {
            currentPosition = nextCoordinates;
            walkedCoordinates.Add(currentPosition);
            nextCoordinates = Move(currentPosition, ref direction, input);
        }
        List<Position> successfulBlocades = [];
        foreach (var walkedCoordinate in walkedCoordinates.Skip(1).Distinct())
        {
            direction = _directionsOrder[0];
            currentPosition = GetStartingPosition(input);
            var originalChar = input[walkedCoordinate.Item1][walkedCoordinate.Item2];
            input[walkedCoordinate.Item1][walkedCoordinate.Item2] = 'O';
            nextCoordinates = Move(currentPosition, ref direction, input);

            var counter = 0;
            while (!IsLeaving(nextCoordinates, input) && counter < 100000)
            {
                currentPosition = nextCoordinates;
                nextCoordinates = Move(currentPosition, ref direction, input);
                counter++;
            }

            if (!IsLeaving(nextCoordinates, input))
            {
                successfulBlocades.Add(walkedCoordinate);
            }

            input[walkedCoordinate.Item1][walkedCoordinate.Item2] = originalChar;
        }

        return successfulBlocades.Count;
    }

    private static Position GetStartingPosition(char[][] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '^' || input[i][j] == 'v' || input[i][j] == '<' || input[i][j] == '>')
                {
                    return new Position(i, j);
                }
            }
        }

        throw new InvalidDataException();
    }

    private Position Move(Position currentPosition, ref Position direction, char[][] input)
    {
        var newPosition = new Position(currentPosition.Item1 + direction.Item1, currentPosition.Item2 + direction.Item2);
        if (IsLeaving(newPosition, input))
        {
            return newPosition;
        }
        if (input[newPosition.Item1][newPosition.Item2] == '#' || input[newPosition.Item1][newPosition.Item2] == 'O')
        {
            direction = TurnRight(direction);
            return Move(currentPosition, ref direction, input);
        }

        return newPosition;
    }

    private static bool IsLeaving(Position nextPosition, char[][] input)
    {
        return nextPosition.Item1 < 0 || nextPosition.Item2 < 0 || nextPosition.Item1 > input.Length - 1 || nextPosition.Item2 > input[0].Length - 1;
    }

    private Position TurnRight(Position currentDirection)
    {
        var indexOf = _directionsOrder.IndexOf(currentDirection);
        if (indexOf == _directionsOrder.Count - 1)
        {
            return _directionsOrder[0];
        }
        else
        {
            return _directionsOrder[indexOf + 1];
        }
    }

    private struct Position(int item1, int item2)
    {
        public int Item1 = item1;
        public int Item2 = item2;
    };
}
