namespace AdventOfCode2024;

internal class AdventDay10 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day10.txt");
        List<Point> startingPoints = [];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '0')
                {
                    startingPoints.Add(new Point('0', j, i, null));
                }
            }
        }

        Dictionary<Point, int> result = [];

        foreach (Point point in startingPoints)
        {
            BuildTree(point, input);
            result[point] = GetFullRoutesCount(point, true);
        }

        return result.Sum(r => r.Value);
    }

    public async Task<object> ExecuteTask2()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day10.txt");
        List<Point> startingPoints = [];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '0')
                {
                    startingPoints.Add(new Point('0', j, i, null));
                }
            }
        }

        Dictionary<Point, int> result = [];

        foreach (Point point in startingPoints)
        {
            BuildTree(point, input);
            result[point] = GetFullRoutesCount(point, false);
        }

        return result.Sum(r => r.Value);
    }

    private static int GetFullRoutesCount(Point point, bool shouldDistinctBy)
    {
        List<IEnumerable<Point>> routes = [];
        IEnumerable<IEnumerable<Point>> result = routes;
        GetRoutes(point, routes);
        if (shouldDistinctBy)
        {
            result = result.DistinctBy(r => new { First = r.First(), Last = r.Last() });
        }

        return result.Count(r => r.Count() == 10);
    }

    private static void GetRoutes(Point point, List<IEnumerable<Point>> routes)
    {
        if (point.Children.Count == 0)
        {
            routes.Add(point.GetAncestorsAndCurrent());
        }
        else
        {
            foreach (var child in point.Children)
            {
                GetRoutes(child, routes);
            }
        }
    }

    private static void BuildTree(Point point, string[] input)
    {
        if (point.Y > 0 && input[point.Y - 1][point.X] == point.Value + 1)
        {
            var child = new Point(input[point.Y - 1][point.X], point.X, point.Y - 1, point);
            BuildTree(child, input);
            point.Children.Add(child);
        }

        if (point.Y < input.Length - 1 && input[point.Y + 1][point.X] == point.Value + 1)
        {
            var child = new Point(input[point.Y + 1][point.X], point.X, point.Y + 1, point);
            BuildTree(child, input);
            point.Children.Add(child);
        }

        if (point.X > 0 && input[point.Y][point.X - 1] == point.Value + 1)
        {
            var child = new Point(input[point.Y][point.X - 1], point.X - 1, point.Y, point);
            BuildTree(child, input);
            point.Children.Add(child);
        }

        if (point.X < input[0].Length - 1 && input[point.Y][point.X + 1] == point.Value + 1)
        {
            var child = new Point(input[point.Y][point.X + 1], point.X + 1, point.Y, point);
            BuildTree(child, input);
            point.Children.Add(child);
        }
    }

    private class Point(char value, int x, int y, Point? parent)
    {
        public readonly char Value = value;
        public readonly int X = x;
        public readonly int Y = y;
        public readonly List<Point> Children = [];
        public readonly Point? Parent = parent;

        public override bool Equals(object? obj)
        {
            return obj is Point point &&
                   Value == point.Value &&
                   X == point.X &&
                   Y == point.Y;
        }

        public IEnumerable<Point> GetAncestorsAndCurrent()
        {
            List<Point> result = [this];
            var current = Parent;
            while (current != null)
            {
                result.Add(current);
                current = current.Parent;
            }

            return result;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, X, Y);
        }
    };
}
