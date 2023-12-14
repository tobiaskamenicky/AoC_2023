using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay14
{
    [Benchmark]
    public long Part1()
    {
        return Day14.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day14.Part2();
    }
}
