using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay12
{
    [Benchmark]
    public long Part1()
    {
        return Day12.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day12.Part2();
    }
}
