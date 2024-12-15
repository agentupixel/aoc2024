using System.Collections.Concurrent;

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

        Dictionary<Prize, List<ButtonPress>> winnablePrizes = [];
        foreach (var prize in prizes)
        {
            for (int i = 1; i <= 100; i++)
            {
                if (IsWinnable(prize.PrizePoint, prize.A, prize.B, i))
                {
                    if (!winnablePrizes.ContainsKey(prize))
                    {
                        winnablePrizes[prize] = [];
                    }
                    ButtonPress buttonPress = new(i, (prize.PrizePoint.X - prize.A.XChange * i) / prize.B.XChange);
                    winnablePrizes[prize].Add(buttonPress);
                }

                if (IsWinnable(prize.PrizePoint, prize.B, prize.A, i))
                {
                    if (!winnablePrizes.ContainsKey(prize))
                    {
                        winnablePrizes[prize] = [];
                    }
                    ButtonPress buttonPress = new((prize.PrizePoint.X - prize.B.XChange * i) / prize.A.XChange, i);
                    winnablePrizes[prize].Add(buttonPress);
                }
            }
        }
        foreach (var prize in winnablePrizes)
        {
            var cheapest = prize.Value.OrderBy(p => p.GetCost()).First();

        }

        return winnablePrizes.Sum(prize => prize.Value.OrderBy(p => p.GetCost()).First().GetCost());
    }

    private static bool IsWinnable(Point prizePoint, Button button1, Button button2, long counter)
    {
        var aXDiff = prizePoint.X - button1.XChange * counter;
        var aYDiff = prizePoint.Y - button1.YChange * counter;

        var isWinnable = aXDiff > 0
            && aYDiff > 0
            && aXDiff % button2.XChange == 0
            && aXDiff / button2.XChange > 0
            && aYDiff % button2.YChange == 0
            && aYDiff / button2.YChange > 0;

        if (isWinnable)
        {
            var button2Counter = aXDiff / button2.XChange;
            if (button1.XChange * counter + button2.XChange * button2Counter != prizePoint.X
                || button1.YChange * counter + button2.YChange * button2Counter != prizePoint.Y)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        return false;
    }
    public async Task<object> ExecuteTask2()
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

        List<Prize> updatedPrizes = new(prizes.Count);

        foreach (var prize in prizes)
        {
            updatedPrizes.Add(new Prize(prize.A, prize.B, new Point(prize.PrizePoint.X + 10000000000000, prize.PrizePoint.Y + 10000000000000)));
        }

        Dictionary<Prize, ConcurrentBag<ButtonPress>> winnablePrizes = [];
        foreach (var prize in prizes)
        {
            winnablePrizes[prize] = [];
            CalculatePrize(winnablePrizes, prize);
        }
        return winnablePrizes.Where(p => p.Value.Any()).Sum(prize => prize.Value.OrderBy(p => p.GetCost()).First().GetCost());
    }

    private static void CalculatePrize(Dictionary<Prize, ConcurrentBag<ButtonPress>> winnablePrizes, Prize prize)
    {

        Parallel.For(1, 10000000000, i =>
        {
            if (IsWinnable(prize.PrizePoint, prize.A, prize.B, i))
            {
                ButtonPress buttonPress = new(i, (prize.PrizePoint.X - prize.A.XChange * i) / prize.B.XChange);
                winnablePrizes[prize].Add(buttonPress);
            }

            if (IsWinnable(prize.PrizePoint, prize.B, prize.A, i))
            {
                ButtonPress buttonPress = new((prize.PrizePoint.X - prize.B.XChange * i) / prize.A.XChange, i);
                winnablePrizes[prize].Add(buttonPress);
            }

        });

        Console.WriteLine("done");
    }

    private readonly struct Button(long xChange, long yChange)
    {
        public long XChange { get; } = xChange;
        public long YChange { get; } = yChange;

        public override string ToString()
        {
            return $"X+{XChange}, Y+{YChange}";
        }
    }

    private readonly struct Point(long x, long y)
    {
        public long X { get; } = x;
        public long Y { get; } = y;

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }

    private readonly struct Prize(AdventDay13.Button a, AdventDay13.Button b, AdventDay13.Point prizePoint)
    {
        public Button A { get; } = a;
        public Button B { get; } = b;
        public Point PrizePoint { get; } = prizePoint;

        public override string ToString()
        {
            return A.ToString() + Environment.NewLine + B.ToString() + Environment.NewLine + PrizePoint.ToString() + Environment.NewLine;
        }
    }

    private readonly struct ButtonPress(long aPress, long bPress)
    {
        public long APress { get; } = aPress;
        public long BPress { get; } = bPress;

        public readonly long GetCost()
        {
            return APress * 3 + BPress * 1;
        }

        public override string ToString()
        {
            return $"A: {APress}, B: {BPress}";
        }
    }
}
