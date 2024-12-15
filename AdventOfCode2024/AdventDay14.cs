namespace AdventOfCode2024;

internal class AdventDay14 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        var robots = await GetInput();

        for (var i = 0; i < 100; i++)
        {
            for (int j = 0; j < robots.Count; j++)
            {
                Robot robot = robots[j];
                robots[j] = robot.Move();
            }
        }

        var validRobots = robots.Where(r => r.X != Robot.Width / 2 && r.Y != Robot.Height / 2);
        var quadrants = validRobots.GroupBy(GetQuadrant);
        var result = 1;
        foreach (var quadrant in quadrants)
        {
            result *= quadrant.Count();
        }

        return result;
    }


    private int GetQuadrant(Robot robot)
    {
        if (robot.X < Robot.Width / 2 && robot.Y < Robot.Height / 2)
            return 0;
        if (robot.X > Robot.Width / 2 && robot.Y < Robot.Height / 2)
            return 1;
        if (robot.X < Robot.Width / 2 && robot.Y > Robot.Height / 2)
            return 2;
        if (robot.X > Robot.Width / 2 && robot.Y > Robot.Height / 2)
            return 3;

        throw new Exception("error");
    }

    public async Task<object> ExecuteTask2()
    {
        throw new NotImplementedException();
    }

    private static async Task<List<Robot>> GetInput()
    {
        List<Robot> robots = [];

        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day14.txt");
        foreach (string line in input)
        {
            var split = line.Split(' ');
            var rawInitial = split[0].Replace("p=", string.Empty).Split(',');
            var rawVelocity = split[1].Replace("v=", string.Empty).Split(',');
            Robot robot = new(int.Parse(rawInitial[0]), int.Parse(rawInitial[1]), int.Parse(rawVelocity[0]), int.Parse(rawVelocity[1]));
            robots.Add(robot);
        }

        return robots;
    }

    private readonly struct Robot(int x, int y, int xMovement, int yMovement)
    {
        public const int Width = 11;
        public const int Height = 7;

        public int X { get; } = x;
        public int Y { get; } = y;
        public int XMovement { get; } = xMovement;
        public int YMovement { get; } = yMovement;

        public Robot Move()
        {
            var newX = X + XMovement;
            var newY = Y + YMovement;
            if (newX < 0)
            {
                newX = Width - 1 - Math.Abs(newX);
            }

            if (newY < 0)
            {
                newY = Height - 1 - Math.Abs(newY);
            }

            if (newX > Width - 1)
            {
                newX = Math.Abs(Width - 1 - newX);
            }

            if (newY > Width - 1)
            {
                newY = Math.Abs(Height - 1 - newY);
            }

            return new(newX, newY, XMovement, YMovement);
        }
    }
}