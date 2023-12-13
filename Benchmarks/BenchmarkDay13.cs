using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay13
{
    [Benchmark]
    public long Part1()
    {
        return Day13.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day13.Part2();
    }
}
