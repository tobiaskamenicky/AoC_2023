using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay19
{
    [Benchmark]
    public long Part1()
    {
        return Day19.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day19.Part2();
    }
}
