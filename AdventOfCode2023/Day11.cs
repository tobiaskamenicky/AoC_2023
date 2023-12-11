namespace AdventOfCode2023;

public static class Day11
{
    public static void Solve()
    {
        Console.WriteLine("Day 11");
        Console.WriteLine($"Part 1: {Part1()}"); //9795148
        Console.WriteLine($"Part 2: {Part2()}"); //650672493820
    }

    public static long Part1()
    {
        return GetDistances(_input.AsSpan(), 2);
    }

    public static long Part2()
    {
        return GetDistances(_input.AsSpan(), 1_000_000);
    }

    private static long GetDistances(ReadOnlySpan<char> input, int expansion)
    {
        var y = 0;
        var galaxies = new List<(int X, int Y)> ();
        Span<bool> emptyColumns = stackalloc bool[input.IndexOf(Environment.NewLine[0])];
        emptyColumns.Fill(true);
        foreach (var line in input.EnumerateLines())
        {
            var allEmpty = true;
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] != '#') continue;

                galaxies.Add((x, y));
                emptyColumns[x] = false;
                allEmpty = false;
            }

            y += allEmpty ? expansion : 1;
        }

        for (var i = 0; i < galaxies.Count; i++)
        {
            var g = galaxies[i];
            galaxies[i] = g with {X = g.X + emptyColumns[..g.X].Count(true) * (expansion-1)};
        }

        long distance = 0;
        for (var i = 0; i < galaxies.Count; i++)
        {
            for (var j = i; j < galaxies.Count; j++)
            {
                distance += Math.Abs(galaxies[i].X - galaxies[j].X) + Math.Abs(galaxies[i].Y - galaxies[j].Y);
            }
        }

        return distance;
    }

    private const string _testInput = """
                                      ...#......
                                      .......#..
                                      #.........
                                      ..........
                                      ......#...
                                      .#........
                                      .........#
                                      ..........
                                      .......#..
                                      #...#.....
                                      """;

    private const string _input = """
                                  ...#...................#.......................#.......................................................#....................................
                                  ........................................................#...................................................................................
                                  ...............................#..........#..........................#.....#......................#.........................................
                                  ......#.................................................................................#.............................#..............#......
                                  ...........................................................................................................................................#
                                  .................#........#.........................#...........#................#....................#......#..............................
                                  ........................................#................#....................................#..............................#..............
                                  ......................#.....................................................................................................................
                                  ..#.........#.......................#..............................#...............................................................#........
                                  .............................................................#..................................................#...........................
                                  ...................#...........#...................#....................#...............................................#...................
                                  ...........................................#...................................#.....................#......#...............................
                                  ....#.........................................................................................................................#.............
                                  ........................................................................................#..........................#........................
                                  ...........#........................#..........................#............#...............................................................
                                  .#...........................#..............................................................................................................
                                  ...............#..........................#................#.......................#.......................#................................
                                  .....................#...........................................................................................#..........................
                                  .........#.......................................#..................#..........#............#..........#..................#.................
                                  .......................................................................................................................................#....
                                  ....................................#........................#..........................#...................................................
                                  .....#.....................................#................................................................................................
                                  ...............#......................................#......................................................#....................#.........
                                  ....................#................................................................#.......#...............................#..............
                                  ...........#................................................................................................................................
                                  .........................#......................#...........................................................................................
                                  ...#..............................#....................................#...........................#........................................
                                  ..................................................................#..................................................................#......
                                  ..........................................#..........#......................#............................................#..................
                                  ...............#......................................................................#..........................#........................#.
                                  .....................#......................................................................................................................
                                  ..................................................#.............#.................................#.........................................
                                  #..........................#.........................................................................................#......................
                                  .....................................#......#................................#..............#...............#..................#............
                                  ........#............................................................................................#................................#.....
                                  ................................................................................................................#...........................
                                  ...............................#.......................#..................#................................................#................
                                  ..................................................................................................#...................#....................#
                                  ............................................................................................................................................
                                  ................#..........#....................#..................#..........................................#.............................
                                  ........#.................................#........................................#.....#.....................................#............
                                  .......................#............................................................................................#.......................
                                  .........................................................................................................................#.........#........
                                  ........................................................................................................#...................................
                                  ....#.......#...............#........................................#......................................................................
                                  ...........................................#.....#.............................................................#..............#.........#...
                                  ............................................................................................................................................
                                  .....................#................#..................#....................#..............#..............................................
                                  ......#.....................................................................................................................................
                                  ...............#......................................................................................#.........................#...........
                                  ........................#...........................#..........#......................................................#.....................
                                  .........................................#...........................#.................#......................#.............................
                                  ............................................................................................................................................
                                  ............................#.............................#.................................#..............................#...........#....
                                  .#...........#......................#.........#...........................#.................................................................
                                  ............................................................................................................................................
                                  .................#..................................................................#...........#.........#......................#..........
                                  ............................................................................................................................................
                                  ............................................................................................................................................
                                  .........#...................#.................................#.............#..............................................................
                                  ...............#....................................................#...............................................#.....#.................
                                  ..........................................#..............................................#...........#.......#..............................
                                  ..#..................................................................................................................................#......
                                  ..........................................................................#.........#.......................................................
                                  ....................................#.........................#.........................................................#...................
                                  ...............................#................................................................#...........................................
                                  ...........#.............................................#............................................#............#........................
                                  ...........................................................................................................................#............#...
                                  ................#...........................#......................#..........#.......#.....................................................
                                  #......................................................................................................................#............#.......
                                  ........................................................................#..................................#................................
                                  ..............................................................#...........................#.....#.........................................#.
                                  ......#.......#.....#.............#.........................................................................................................
                                  .............................#.........#..................#.................................................................................
                                  ..........#.........................................................................................................#.......................
                                  ......................................................#................................................#...................#................
                                  ....#....................#.........................................#.........................................#..............................
                                  ....................................#.........#...............#...............#..............#...................................#..........
                                  ............................................................................................................................................
                                  .............................#..................................................................................#.......#...................
                                  ........................................#................#...............#..................................................................
                                  ............................................................................................................................................
                                  .............................................................#.........................#................#...................................
                                  ..........#......#............................#..................................#...............#..............................#.........#.
                                  ..................................#.................................................................................#.......................
                                  ....................................................#.......................#...............#.................#.......................#.....
                                  .#...........#........................#..............................................#......................................................
                                  ................................................................#.......#..............................#....................................
                                  ..................................................................................................#...........................#.............
                                  ............................................................#............................................................#..............#...
                                  ......#.....................#......................................#...........#............................................................
                                  .....................................................#.............................................................#.............#..........
                                  .......................#.............#.....................................................................................................#
                                  ...........#.................................#............................#...........................#.....................................
                                  .#...............................#....................................................................................#.....................
                                  ...................#...................................#..........................................#..............#..........................
                                  .........................#.......................................#.............#............................................................
                                  ..........................................#.......#.....................#................#..................................#...............
                                  ...#.......................................................................................................#............................#...
                                  ............#.............................................#..................................#...................................#..........
                                  ............................#..........#.............................................................#.............#........................
                                  ......................#.....................................................................................................................
                                  .............................................................................................................#..............................
                                  ....#........................................#.....................................#.......................................#................
                                  .....................................................#..............#...................................................................#...
                                  #....................................#....................#.......................................................................#.........
                                  ................................................................................................#...........................................
                                  ...........#.........#....................#...............................#.............#................................#..................
                                  ............................................................................................................................................
                                  ......#..........................#...........................#...........................................#...........#......................
                                  .#.............#.........#.........................................#...........#.....#..................................................#...
                                  .....................................#...................#.....................................................................#............
                                  ...........................................................................................#................................................
                                  ...................#................................................................................#..............#........................
                                  ...........................................#......#........................................................................................#
                                  ..#........................#..................................#.........................#...................................................
                                  ........#.............................#...............................................................................#.....................
                                  ...............................#........................#...............#..............................#....................................
                                  .................................................................#..........................................#...............................
                                  ....................................................#........................#........#....................................#.........#......
                                  ....#.........#....................................................................................#.......................................#
                                  ......................#.....#.............................................................#.................................................
                                  #......................................#......#....................................#...............................#.............#..........
                                  .......................................................#..................................................#.................................
                                  .........................................................................#......................#...........................................
                                  ...............................................................................#............................................#...............
                                  ....#........#................#...............................#.......................................................................#.....
                                  .......................#.................................................................#..................#...............................
                                  .....................................#.............#........................................................................................
                                  ...................................................................................................#......................................#.
                                  ..#.........................................................................................................................................
                                  ........#.................................#.............#...........#................#......................................................
                                  ...............................................................................................#............................#...............
                                  .................#....................#.........................#..................................................................#........
                                  ....#.............................................#........................................................#........#.......................
                                  .........................#.............................................#............................#.......................................
                                  ......................................................#.................................#................................................#..
                                  .............................#...............#.................................#............................................................
                                  .........#....................................................................................#........................#........#...........
                                  ..................#...........................................#.................................................#...........................
                                  """;
}
