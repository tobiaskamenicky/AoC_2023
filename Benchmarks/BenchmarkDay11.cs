using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay11
{
    [Benchmark]
    public long Part1()
    {
        return Day11.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day11.Part2();
    }
}
