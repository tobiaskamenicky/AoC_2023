using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay8
{
    [Benchmark]
    public long Part1()
    {
        return Day8.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day8.Part2();
    }
}
