namespace AdventOfCode2024;

internal class AdventDay12 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day12.txt");
        List<Fence> fences = GetFences(input);

        return fences.Sum(d => d.Area * d.Perimeters.Count);
    }

    public async Task<object> ExecuteTask2()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day12.txt");
        List<Fence> fences = GetFences(input);
        Dictionary<Fence, int> newFences = [];
        foreach (var fence in fences)
        {
            newFences[fence] = 0;
            List<List<Perimeter>> lines = [];
            var ordered = fence.Perimeters.OrderBy(p => p.Point.Y).ThenBy(p => p.Point.X).ThenBy(p => p.Side).ToList();
            for (int i = 0; i < ordered.Count; i++)
            {
                var current = ordered[i];
                var matchingLine = lines
                    .FirstOrDefault(l => l
                        .Any(p => p.Side == current.Side
                            && (Math.Abs(current.Point.X - p.Point.X) == 1 && current.Point.Y - p.Point.Y == 0
                                || Math.Abs(current.Point.Y - p.Point.Y) == 1 && current.Point.X - p.Point.X == 0)));
                if (matchingLine != null)
                {
                    matchingLine.Add(current);
                }
                else
                {
                    lines.Add([current]);
                }
            }

            newFences[fence] = lines.Count;
        }

        return newFences.Sum(f => f.Key.Area * f.Value);
    }

    private static List<Fence> GetFences(string[] input)
    {
        Dictionary<char, List<Point>> dict = [];
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                if (!dict.TryGetValue(input[i][j], out List<Point>? coordinates))
                {
                    coordinates = [];
                    dict[input[i][j]] = coordinates;
                }

                coordinates.Add(new Point(j, i));
            }
        }

        List<Fence> fences = [];
        foreach (var item in dict)
        {
            List<Fence> vegetableFences = [];
            foreach (var coordinate in item.Value)
            {
                if (vegetableFences.Any(f => f.Coordinates.Contains(coordinate)))
                {
                    continue;
                }
                var field = GetAllNeighbours(item.Value, coordinate, [coordinate]);
                field.Add(coordinate);
                field = field.Distinct().ToList();

                Fence fence = new();
                foreach (var point in field)
                {
                    fence.Coordinates.Add(point);
                    fence.Area += 1;
                    if (point.Y == 0 || input[point.Y - 1][point.X] != item.Key)
                    {
                        fence.Perimeters.Add(new Perimeter(point, Side.Top));
                    }

                    if (point.Y == input.Length - 1 || input[point.Y + 1][point.X] != item.Key)
                    {
                        fence.Perimeters.Add(new Perimeter(point, Side.Down));
                    }

                    if (point.X == 0 || input[point.Y][point.X - 1] != item.Key)
                    {
                        fence.Perimeters.Add(new Perimeter(point, Side.Left));
                    }

                    if (point.X == input[point.Y].Length - 1 || input[point.Y][point.X + 1] != item.Key)
                    {
                        fence.Perimeters.Add(new Perimeter(point, Side.Right));
                    }
                }

                vegetableFences.Add(fence);
            }
            fences.AddRange(vegetableFences);
        }

        return fences;
    }

    private static List<Point> GetAllNeighbours(List<Point> matrix, Point pointToCheck, HashSet<Point> except)
    {
        var neighbours = matrix
            .Except(except)
            .Where(pointToCheck.IsNeighbour)
            .ToList();

        List<Point> result = [];
        result.AddRange(neighbours);
        foreach (var neighbour in neighbours)
            except.Add(neighbour);
        foreach (var neighbour in neighbours)
        {

            result.AddRange(GetAllNeighbours(matrix, neighbour, except));
        }

        return result;
    }

    private class Fence
    {
        public List<Perimeter> Perimeters { get; set; } = [];
        public int Area { get; set; } = 0;
        public List<Point> Coordinates { get; } = [];
    }

    private readonly struct Perimeter(AdventDay12.Point point, AdventDay12.Side side)
    {
        public Point Point { get; } = point;
        public Side Side { get; } = side;
    }

    private enum Side
    {
        Top, Down, Left, Right
    }

    public readonly struct Point(int x, int y)
    {
        public int X { get; } = x; public int Y { get; } = y;

        public readonly bool IsNeighbour(Point other)
        {
            return X == other.X + 1 && Y == other.Y
                || X == other.X - 1 && Y == other.Y
                || X == other.X && Y == other.Y - 1
                || X == other.X && Y == other.Y + 1;
        }
    }
}
