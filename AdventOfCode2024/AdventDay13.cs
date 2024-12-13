namespace AdventOfCode2024;

internal class AdventDay13 : IAdventDay
{
    public async Task<object> ExecuteTask1()
    {
        string[] input = await File.ReadAllLinesAsync("C:\\repos\\days\\day13.txt");
        List<Prize> prizes = [];
        for (int i = 0; i < input.Length; i += 4)
        {
            var buttonARaw = input[i].Replace("Button A: ", string.Empty).Replace("X+", string.Empty).Replace("Y+", string.Empty).Trim().Split(", ");
            Button buttonA = new(int.Parse(buttonARaw[0]), int.Parse(buttonARaw[1]));
            var buttonBRaw = input[i + 1].Replace("Button B: ", string.Empty).Replace("X+", string.Empty).Replace("Y+", string.Empty).Trim().Split(", ");
            Button buttonB = new(int.Parse(buttonBRaw[0]), int.Parse(buttonBRaw[1]));
            var prizeRaw = input[i + 2].Replace("Prize: ", string.Empty).Replace("X=", string.Empty).Replace("Y=", string.Empty).Trim().Split(", ");
            Point prizePoint = new(int.Parse(prizeRaw[0]), int.Parse(prizeRaw[1]));
            prizes.Add(new(buttonA, buttonB, prizePoint));
        }

        Dictionary<Prize, ButtonPress> winnablePrizes = [];
        foreach (var prize in prizes)
        {
            ButtonPress? cheapestPress = null;
            for (int i = 1; i <= 100; i++)
            {
                if (IsWinnable(prize.PrizePoint, prize.A.XChange * i, prize.A.YChange * i, prize.B))
                {
                    ButtonPress buttonPress = new(i, (prize.PrizePoint.X - prize.A.XChange * i) / prize.B.XChange);
                    if (cheapestPress == null)
                    {
                        cheapestPress = buttonPress;
                    }
                    else
                    {
                        if (cheapestPress.Value.GetCost() >= buttonPress.GetCost())
                        {
                            cheapestPress = buttonPress;
                        }
                    }

                    winnablePrizes[prize] = cheapestPress.Value;
                }

                if (IsWinnable(prize.PrizePoint, prize.B.XChange * i, prize.B.YChange * i, prize.A))
                {
                    ButtonPress buttonPress = new((prize.PrizePoint.X - prize.B.XChange * i) / prize.A.XChange, i);
                    if (cheapestPress == null)
                    {
                        cheapestPress = buttonPress;
                    }
                    else
                    {
                        if (cheapestPress.Value.GetCost() >= buttonPress.GetCost())
                        {
                            cheapestPress = buttonPress;
                        }
                    }

                    winnablePrizes[prize] = cheapestPress.Value;
                }
            }
        }

        return winnablePrizes.Sum(prize => prize.Value.GetCost());
    }

    private static bool IsWinnable(Point prizePoint, int aXChange, int aYChange, Button b)
    {
        var aXDiff = prizePoint.X - aXChange;
        var aYDiff = prizePoint.Y - aYChange;

        return aXDiff > 0 && aYDiff > 0 && aXDiff % b.XChange == 0 && aYDiff % b.YChange == 0;
    }

    public async Task<object> ExecuteTask2()
    {
        throw new NotImplementedException();
    }

    private readonly struct Button(int xChange, int yChange)
    {
        public int XChange { get; } = xChange;
        public int YChange { get; } = yChange;
    }

    private readonly struct Point(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private readonly struct Prize(AdventDay13.Button a, AdventDay13.Button b, AdventDay13.Point prizePoint)
    {
        public Button A { get; } = a;
        public Button B { get; } = b;
        public Point PrizePoint { get; } = prizePoint;
    }

    private readonly struct ButtonPress(int aPress, int bPress)
    {
        public int APress { get; } = aPress;
        public int BPress { get; } = bPress;

        public readonly int GetCost()
        {
            return APress * 3 + BPress * 1;
        }
    }
}
