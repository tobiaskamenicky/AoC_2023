namespace AdventOfCode2023;

public static class Day16
{
    public static void Solve()
    {
        Console.WriteLine("Day 16");
        Console.WriteLine($"Part 1: {Part1()}"); //6883
        Console.WriteLine($"Part 2: {Part2()}"); //7228
    }

    public static long Part1()
    {
        var input = _input.AsSpan();
        var lineLength = input.IndexOf(Environment.NewLine[0]);
        var lineWidth = lineLength + Environment.NewLine.Length;
        var lineCount = input.Length / lineWidth + 1;

        var pending = new Stack<(int X, int Y, char D)>();
        pending.Push((0, 0, 'r'));
        var visited = new HashSet<(int X, int Y, char D)>();

        while (pending.Count != 0)
        {
            var (x, y, d) = pending.Pop();

            for (; x >= 0 && x < lineLength && y >= 0 && y < lineCount;)
            {
                if (!visited.Add((x, y, d)))
                {
                    break;
                }

                var c = input[y * lineWidth + x];
                d = (d, c) switch
                {
                    (_, '.') => d,

                    ('r', '/') => 'u',
                    ('r', '\\') => 'd',
                    ('l', '/') => 'd',
                    ('l', '\\') => 'u',
                    ('u', '/') => 'r',
                    ('u', '\\') => 'l',
                    ('d', '/') => 'l',
                    ('d', '\\') => 'r',

                    ('r', '-') => 'r',
                    ('l', '-') => 'l',
                    (_, '-') => 'h',
                    ('u', '|') => 'u',
                    ('d', '|') => 'd',
                    (_, '|') => 'v',

                    _ => throw new()
                };

                if (d == 'h')
                {
                    d = 'l';
                    pending.Push((x + 1, y, 'r'));
                }
                else if (d == 'v')
                {
                    d = 'u';
                    pending.Push((x, y + 1, 'd'));
                }

                (x, y) = d switch
                {
                    'r' => (x + 1, y),
                    'l' => (x - 1, y),
                    'u' => (x, y - 1),
                    'd' => (x, y + 1),
                    _ => throw new()
                };
            }
        }

        return visited.DistinctBy(x => (x.X, x.Y)).Count();
    }

    public static long Part2()
    {
        var input = _input.AsSpan();
        var lineLength = input.IndexOf(Environment.NewLine[0]);
        var lineWidth = lineLength + Environment.NewLine.Length;
        var lineCount = input.Length / lineWidth + 1;

        var result = 0;
        for (var i = 0; i < lineLength; i++)
        {
            result = Math.Max(result, FindCount(i, 0, 'd'));
            result = Math.Max(result, FindCount(i, lineCount - 1, 'u'));
        }

        for (var i = 0; i < lineCount; i++)
        {
            result = Math.Max(result, FindCount(0, i, 'r'));
            result = Math.Max(result, FindCount(lineCount - 1, i, 'l'));
        }

        return result;

        int FindCount(int x, int y, char d)
        {
            var pending = new Stack<(int X, int Y, char D)>();
            pending.Push((x, y, d));
            var visited = new HashSet<(int X, int Y, char D)>();

            while (pending.Count != 0)
            {
                (x, y, d) = pending.Pop();

                for (; x >= 0 && x < lineLength && y >= 0 && y < lineCount;)
                {
                    if (!visited.Add((x, y, d)))
                    {
                        break;
                    }

                    var c = _input.AsSpan()[y * lineWidth + x];
                    d = (d, c) switch
                    {
                        (_, '.') => d,

                        ('r', '/') => 'u',
                        ('r', '\\') => 'd',
                        ('l', '/') => 'd',
                        ('l', '\\') => 'u',
                        ('u', '/') => 'r',
                        ('u', '\\') => 'l',
                        ('d', '/') => 'l',
                        ('d', '\\') => 'r',

                        ('r', '-') => 'r',
                        ('l', '-') => 'l',
                        (_, '-') => 'h',
                        ('u', '|') => 'u',
                        ('d', '|') => 'd',
                        (_, '|') => 'v',

                        _ => throw new()
                    };

                    if (d == 'h')
                    {
                        d = 'l';
                        pending.Push((x + 1, y, 'r'));
                    }
                    else if (d == 'v')
                    {
                        d = 'u';
                        pending.Push((x, y + 1, 'd'));
                    }

                    (x, y) = d switch
                    {
                        'r' => (x + 1, y),
                        'l' => (x - 1, y),
                        'u' => (x, y - 1),
                        'd' => (x, y + 1),
                        _ => throw new()
                    };
                }
            }

            return visited.DistinctBy(v => (v.X, v.Y)).Count();
        }
    }

    private const string _testInput = """
                                      .|...\....
                                      |.-.\.....
                                      .....|-...
                                      ........|.
                                      ..........
                                      .........\
                                      ..../.\\..
                                      .-.-/..|..
                                      .|....-|.\
                                      ..//.|....
                                      """;

    private const string _input = """
                                  \.........../../..\.\.\................|..-...\........./.......-.....................-...\...--....../..\....
                                  ........|............|............-.........|....\.................\-.........../...../....-../....\..........
                                  .-....................\\..................|.\..........\.........../.............................-...-........
                                  ............-.....\/...........-........-......................-..-...../...............|................/....
                                  .......\....-...-...............|.............-...................../....-../............./\..........-..\....
                                  ..................-.............\.........\........-......./..../..|....|......-/............\.............\..
                                  -...............|.........\........../.\........../......./-..........-...-.....-............................|
                                  .\\...\..........-............./...-|...................../.......-.................-....../.........|........
                                  ..................../........................./..\...................\.................-/......./.............
                                  ../...................|...../.-..................-.....|....-.|....................../.../\....-....|..\..|.\.
                                  ..................|..................................-....|.........\................................./.......
                                  .........-...........-........\..|-...................--.............-.../......./....................\..-....
                                  -..........-.................\.....|......|.\................|../..........\.........\.|.............-....\...
                                  ......./....-....|....-..........|........-...-....-.....-............................................/../....
                                  .......\...........|.......\....\.//\.............\...../|....-...../..|....-................/................
                                  .........\........./............\.-.|......./..........................|....\........-.-.............../..|-.|
                                  .............................\......|....-../.....\.....-..../.........................\......|....../...\/...
                                  .../.\.........../................................./.\.\...............\..............\.....|........-........
                                  ..................\......-......|..................\................................/.../.|...................
                                  ...\\............................\./................../............................-.......-..|...............
                                  /.............\...-....\...//..-..\....-......-.\............../.....|../.........|..../........\......\......
                                  ./..........-.........\..-.................-.|...........\.......-.......|...................|../.............
                                  .....-./.\..../.|.............../...........|..../....\...\/..-.........|....-.........|......................
                                  ...../.|.............../.....................\\../...|........\........./|-...........\/./.\\.................
                                  ..............\...-..............\.|......./........-...............-.-...|..\......-./..........-.......|../.
                                  .....|...............\....|............../................./....../...-........-.....|........\.........||../|
                                  .../................................../...............-......................................../.......-......
                                  ......../.........|..../...............|...../..|................./............|.../.....|....|..\........-...
                                  .../................................//-.......\........./......../....-.....-....-............................
                                  ........../....................|................................./.-......|.....\........-..............|..|..
                                  ........|...-............|.\.../.......................\...\../...........|../\.\-..../|\..|-...\......\....-.
                                  ...-.\..../.....||.....\./.-.....\..............-.......\.../...-....|......\...........-.....-....../........
                                  ...|-...|..........\.........../.......................-....../...............\.......\.....|.................
                                  ....\..../\\..........-........\............................/...........|.........|......|.|...........|...|..
                                  .............\............................../....|..\.\..../....................\..|...|..-............\....-.
                                  .-/|./...\....-\....-...\...............-.../............-..../.--............................|||.............
                                  ....../../\\...|..-.................\.............../.|\/............................../.........\.....|......
                                  ..../....................\..................../......\............-//.........................................
                                  .............\..../........../........-..............................|...-/....../..|....|.......-......../...
                                  ................-.|..\../...|.\..|..-.....-..../...................\..............\........................./.
                                  ........................../......\...............................-..../......................-...........-....
                                  ........\.-......................./...............\..............................\...../...................\..
                                  ..............................\..|..............|\.-..-....-..|..../.........\.............../.-..............
                                  ..........................\\./|.........\|\..............\......./.......-.-...................\.......-......
                                  .............|.........-..\..................|......-..|.-|.................--..||............-.|.............
                                  ..............\..\.\.\.........-./....../\../|../.......................\-.....\....|.........................
                                  .............................|.........\.....|..............................|.......././-.../.........-./....|
                                  \.....................\.../..................................-......./........../..\.\.................|......
                                  ...............-......................../.....................................................................
                                  ..../........./........-.....|................/../....\.........../../.././........./......................-..
                                  .......\................/............................................\........../..\.\......|/|\..............
                                  ...........\.............................\\......\\..|..-\................../.-.....................-.........
                                  ........|../..\...............|......./.................................|......|./....../......-..............
                                  ........./................\.............|...............-.........-.....\.............../.\..........|........
                                  .........................|......./....................-......|........\.........................|/............
                                  .\..-.......................................-....|.................................|\./.\.....................
                                  \../................................-.-.....-..................../....../......../\...|..|......../.......\...
                                  ........................../..............\|............./..../.................../...\....\/..................
                                  ../-.........................\...\......\......................|.......\..\...\...\.................-..|....\.
                                  ....-...................\......-........../......../...............|................................/.........
                                  .................................../......|.......|......................................|..\........-........
                                  ......|..|....-..................\./.............../.|...............|.|/.......\.\...........................
                                  ...............|...........|.....\................................................|................\.....|....
                                  ......../...|......../.................../../...........|...|............./.............\..|.../........-.....
                                  .....................-............\...|..........|...-..........|..-..../...-...../......|....................
                                  ...\.......\.....-..................................|/.....\......|.....................|.......|......./.-...
                                  .....................|........./...............|.-...........\........|......|...-/............../.-..........
                                  ......./....../.......-....-..............\../\.......\................................................|......
                                  ................../.......\\.......................|...\........|...\........-....\..-..........\...../..-....
                                  ...............................................-.........................\................\............../..\.
                                  ....|......../.........\.........-../....-....................\..................|............................
                                  ................./.-../\..........-.....|.........\.........\..../.............................|........--/...
                                  ................|./...............|.......\....-\........-.....................-.....|..................../-..
                                  ...........\........................|../...-./......\...............\........-./......|.................|..\..
                                  .......|-....../...........|..........|............/.............../.|....../...................\.............
                                  ...../...../..........|.......................|....................|.........\|............................\..
                                  ........-........\......./....\.............//................................./......./...................|..
                                  ..-......../...................-...../.........-...................|..........................\....../.-......
                                  ./............................\...............................|............|...../.|................|.........
                                  ......\................|.............................-.........\.................................../-........\
                                  ./-/..........|.................................../........\....\.....\.....................\.\.........|.....
                                  .-............../../........./.........-......\.....................\..\..\..\/......-./.../....|.............
                                  .....|..................-...\||.......\./../........................-.........................-..........\....
                                  ...-................/.|........-....................................|..\....-...\......-..|............/......
                                  ..........\...............|.|...-.............-.././...................../.../................................
                                  .......\./............./........../..\............\............-\......|......|....|.........../|.............
                                  ............\....|...........|...............-........\..|\..-.....-...|....................\..../.......-....
                                  ........\.....\...................|......./......-........./....\-...................|................\....-..
                                  ../......................../................-...|..........\........../...................................|.\.
                                  .....................-..../......../........./........./.........\.......|/-...........|.........\...../...../
                                  ......//..................\.|....|..|..\...\-........./...........\............/..................|...-.....\.
                                  .....-..-....../..................\....\........................|......................../....|........\..|.\.
                                  .........-............\..............\..\.\../............................\..-...-................./-.........
                                  ....|./..........\..-.................\..\...../.\...................-................/............|...../....
                                  ...\.................../.....-...../.-.././....-/......|..................|................/.............-....
                                  .....\.||.................\................\..|.....|.............\.\-.............-................./........
                                  .......\..........\....................|................/..............|.../..................\...-.-.||......
                                  .....-./..\.\.........................................|......./.............../....|..|.......-........\...\..
                                  .....|............./.....-...................|.\..../-............\.........................-................\
                                  ........\/............./......\......|-...|................../.....|-......................./..............\-.
                                  ..............-..../.....-..........||.-.....-.../................././.../............\/................./../.
                                  -.....................................|.........-.....|...................|..................../.../..........
                                  .......................-..|................/...........\...........................|.--.....\..........|/..-..
                                  ...............\..................../..../.|..........................|.....................|..\...\....--....
                                  ....../.......-...............................|....../.........\.-........../.....................\.--...-....
                                  .-......\............/...-........|.....\........../|........................|..........\...|....../-.........
                                  /............................../.....................................\........../|........-...............\..-
                                  .........\........\.|.|\..\.....-..............-.........-...........|........|...|...........-.............||
                                  .-\................\.......|...................................../..................-.-.....|...\..\.|........
                                  \............-/../.......|............./......\\.\./.|......-/.\.\.............\........./|\.....-............
                                  """;
}
