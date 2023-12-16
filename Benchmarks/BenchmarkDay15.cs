using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay15
{
    [Benchmark]
    public long Part1()
    {
        return Day15.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day15.Part2();
    }
}
