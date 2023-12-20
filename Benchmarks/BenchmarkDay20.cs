using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay20
{
    [Benchmark]
    public long Part1()
    {
        return Day20.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day20.Part2();
    }
}
