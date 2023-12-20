namespace AdventOfCode2023;

public static class Day20
{
    public static void Solve()
    {
        Console.WriteLine("Day 20");
        Console.WriteLine($"Part 1: {Part1()}"); //406849
        Console.WriteLine($"Part 2: {Part2()}"); //138625360533574
    }

    private interface IModule
    {
        public string Label { get; }
        public string[] Destinations { get; }

        public bool IsDefault();
    }

    private record Module(string Label, string[] Destinations) : IModule
    {
        public bool IsDefault() => true;
    }
    private record FlipModule(string Label, string[] Destinations) : IModule
    {
        public bool IsOn { get; private set; }
        public void Flip() => IsOn = !IsOn;

        public bool IsDefault() => !IsOn;
    }
    private record ConjunctionModule(string Label, string[] Destinations) : IModule
    {
        private readonly Dictionary<string, bool> _inputs = new();
        public void AddSource(string label)
        {
            _inputs.Add(label, false);
        }

        public void SetInput(string source, bool isHigh) => _inputs[source] = isHigh;
        public bool IsHigh => _inputs.Values.All(x => x);
        public bool IsDefault() => _inputs.Values.All(x => !x);
    }

    public static long Part1()
    {
        var input = _input.AsSpan();

        var graph = new Dictionary<string, IModule>();
        foreach (var line in input.EnumerateLines())
        {
            var connections = line[(line.IndexOf(">") + 2)..]
                .ToString().Split(",")
                .Select(x => x.Trim())
                .ToArray();
            var label = line[..line.IndexOf(" ")];
            IModule module = line[0] switch
            {
                '%' => new FlipModule(label[1..].ToString(), connections),
                '&' => new ConjunctionModule(label[1..].ToString(), connections),
                _ => new Module(label.ToString(), connections)
            };
            graph.Add(module.Label, module);
        }

        foreach (var module in graph)
        {
            foreach (var destination in module.Value.Destinations)
            {
                if (!graph.TryGetValue(destination, out var destModule)) continue;
                if (destModule is ConjunctionModule conjunctionModule)
                {
                    conjunctionModule.AddSource(module.Key);
                }
            }
        }

        var high = 0;
        var low = 0;

        var queue = new Queue<(string Label, bool IsHigh, string Source)>();

        for (var i = 0; i < 1000; i++)
        {
            queue.Enqueue(("broadcaster", false, "button"));
            while (queue.Count > 0)
            {
                var (label, isHigh, source) = queue.Dequeue();
                if (isHigh)
                {
                    high++;
                }
                else
                {
                    low++;
                }

                if (!graph.TryGetValue(label, out var module)) continue;
                if (module is FlipModule flipModule)
                {
                    if (isHigh) continue;

                    flipModule.Flip();
                    isHigh = flipModule.IsOn;
                }
                else if (module is ConjunctionModule conjunctionModule)
                {
                    conjunctionModule.SetInput(source, isHigh);
                    isHigh = !conjunctionModule.IsHigh;
                }

                foreach (var destination in module.Destinations)
                {
                    queue.Enqueue((destination, isHigh, label));
                }
            }

            if (graph.All(x => x.Value.IsDefault()))
            {
                var cycle = i + 1;
                var totalCycles = 1000 / cycle;

                high *= totalCycles;
                low *= totalCycles;
                i = totalCycles*cycle - 1;
            }
        }

        return high * low;
    }

    public static long Part2()
    {
        var input = _input.AsSpan();

        var graph = new Dictionary<string, IModule>();
        foreach (var line in input.EnumerateLines())
        {
            var connections = line[(line.IndexOf(">") + 2)..]
                .ToString().Split(",")
                .Select(x => x.Trim())
                .ToArray();
            var label = line[..line.IndexOf(" ")];
            IModule module = line[0] switch
            {
                '%' => new FlipModule(label[1..].ToString(), connections),
                '&' => new ConjunctionModule(label[1..].ToString(), connections),
                _ => new Module(label.ToString(), connections)
            };
            graph.Add(module.Label, module);
        }

        foreach (var module in graph)
        {
            foreach (var destination in module.Value.Destinations)
            {
                if (!graph.TryGetValue(destination, out var destModule)) continue;
                if (destModule is ConjunctionModule conjunctionModule)
                {
                    conjunctionModule.AddSource(module.Key);
                }
            }
        }

        var queue = new Queue<(string Label, bool IsHigh, string Source)>();

        var rxSource = graph.FirstOrDefault(x => x.Value.Destinations.Contains("rx")).Key;
        var sources = graph.Where(x => x.Value.Destinations.Contains(rxSource)).Select(x => x.Key).ToArray();
        var sourceIntervals = sources.Select(x => new List<long>()).ToArray();
        for (var i = 0;; i++)
        {
            queue.Enqueue(("broadcaster", false, "button"));
            while (queue.Count > 0)
            {
                var (label, isHigh, source) = queue.Dequeue();

                if (!graph.TryGetValue(label, out var module)) continue;
                if (module is FlipModule flipModule)
                {
                    if (isHigh) continue;

                    flipModule.Flip();
                    isHigh = flipModule.IsOn;
                }
                else if (module is ConjunctionModule conjunctionModule)
                {
                    conjunctionModule.SetInput(source, isHigh);
                    isHigh = !conjunctionModule.IsHigh;
                }

                foreach (var destination in module.Destinations)
                {
                    var sourceIndex = Array.IndexOf(sources, label);
                    if (sourceIndex >= 0 && isHigh)
                    {
                        sourceIntervals[sourceIndex].Add(i+1);
                    }

                    queue.Enqueue((destination, isHigh, label));
                }
            }

            if (sourceIntervals.All(x => x.Count >= 2)) break;
        }

        var intervals = sourceIntervals.Select(x => x[1] - x[0]).ToArray();
        var lcm = Lcm(intervals[0], intervals[1]);
        for (var i = 2; i < intervals.Length; i++)
        {
            lcm = Lcm(lcm, intervals[i]);
        }

        return lcm;
    }

    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            a %= b;
            (a, b) = (b, a);
        }

        return a;
    }

    private static long Lcm(long a, long b)
    {
        return a / Gcd(a, b) * b;
    }

    private const string _testInput = """
                                      broadcaster -> a
                                      %a -> inv, con
                                      &inv -> b
                                      %b -> con
                                      &con -> output
                                      """;

    private const string _input = """
                                  %tx -> dx
                                  %nx -> fn, rn
                                  %nr -> cj, mh
                                  %nk -> jt, vk
                                  %mv -> fk, rn
                                  &pz -> kt, pg, mb, vr, hp, jp, tx
                                  &jt -> fb, zb, jq, sv, lp
                                  %vp -> lp, jt
                                  &qs -> gf
                                  %lj -> jt, dt
                                  %jh -> mh
                                  %xc -> nx
                                  %hx -> xb
                                  %kd -> pz, pp
                                  %jq -> jt, qt
                                  %lp -> jm
                                  %ph -> mb, pz
                                  &sv -> gf
                                  %ff -> xc
                                  %th -> mh, hx
                                  %kt -> ct
                                  %ct -> kd, pz
                                  &mh -> bc, qs, hx, xb, nv
                                  &pg -> gf
                                  %fn -> kn
                                  %sk -> hr
                                  %nv -> mh, th
                                  %dx -> pz, ph
                                  broadcaster -> bx, jq, nv, jp
                                  %dt -> jt, zb
                                  %fx -> sk, rn
                                  %rv -> rn
                                  %gv -> mh, nr
                                  %fk -> rn, rv
                                  %cj -> mh, vh
                                  %xk -> jt, nk
                                  %vh -> mh, jh
                                  %zb -> fb
                                  %mb -> jc
                                  %kn -> rn, mv
                                  %jc -> pz, kt
                                  &sp -> gf
                                  %hp -> tx
                                  %jf -> bc, mh
                                  %fb -> vp
                                  %xm -> mh, gv
                                  %jm -> jt, xk
                                  %vr -> hp
                                  %hr -> ff
                                  %jp -> pz, vr
                                  &rn -> fn, hr, bx, ff, xc, sp, sk
                                  %pp -> pz
                                  &gf -> rx
                                  %xb -> jf
                                  %bx -> rn, fx
                                  %bc -> xm
                                  %qt -> lj, jt
                                  %vk -> jt
                                  """;
}
