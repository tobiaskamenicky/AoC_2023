namespace AdventOfCode2023;

public static class Day6
{
    public static void Solve()
    {
        Console.WriteLine("Day 6");
        Console.WriteLine($"Part 1: {Part1()}");
        Console.WriteLine($"Part 2: {Part2()}");
    }

    public static long Part1()
    {
        var result = 1;

        var input = _input.AsSpan();
        var endL = input.IndexOf(Environment.NewLine[0]);
        var firstLine = input[..endL];
        var secondLine = input[(endL + Environment.NewLine.Length)..];

        while (true)
        {
            var timeS = firstLine.IndexOfAnyInRange('0', '9');
            if (timeS == -1) break;
            firstLine = firstLine[timeS..];
            var timeE = firstLine.IndexOf(' ');
            if (timeE == -1)
            {
                timeE = firstLine.Length;
            }

            var time = int.Parse(firstLine[..timeE]);

            var distanceS = secondLine.IndexOfAnyInRange('0', '9');
            secondLine = secondLine[distanceS..];
            var distanceE = secondLine.IndexOf(' ');
            if (distanceE == -1) distanceE = secondLine.Length;
            var distance = int.Parse(secondLine[..distanceE]);

            var dis = Math.Sqrt(time * time - 4 * distance);
            var h1 = (time - dis) / 2;
            var h2 = (time + dis) / 2;
            result *= (int) Math.Ceiling(h2) - (int) (h1 + 1);

            if (timeE == firstLine.Length || distanceE == secondLine.Length) break;
            firstLine = firstLine[(timeE + 1)..];
            secondLine = secondLine[(distanceE + 1)..];
        }

        return result;
    }

    public static long Part2()
    {
        var input = _input.AsSpan();
        var endL = input.IndexOf(Environment.NewLine[0]);
        var firstLine = input[..endL];
        var secondLine = input[(endL + Environment.NewLine.Length)..];
        var time = ParseNumber(firstLine);
        var distance = ParseNumber(secondLine);

        var dis = Math.Sqrt(time * time - 4 * distance);
        var h1 = (time - dis) / 2;
        var h2 = (time + dis) / 2;

        return (long) Math.Ceiling(h2) - (long) Math.Floor(h1 + 1);

        long ParseNumber(ReadOnlySpan<char> line)
        {
            long value = 0;
            for (var i = line.IndexOfAnyInRange('0', '9'); i < line.Length; i++)
            {
                if (!char.IsDigit(line[i]))
                {
                    continue;
                }

                value = value * 10 + long.Parse(line[i..(i + 1)]);
            }

            return value;
        }
    }

    private const string _testInput = """
                                      Time:      7  15   30
                                      Distance:  9  40  200
                                      """;

    private const string _input = """
                                  Time:        46     68     98     66
                                  Distance:   358   1054   1807   1080
                                  """;
}
