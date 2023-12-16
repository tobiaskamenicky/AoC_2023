using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay16
{
    [Benchmark]
    public long Part1()
    {
        return Day16.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day16.Part2();
    }
}
