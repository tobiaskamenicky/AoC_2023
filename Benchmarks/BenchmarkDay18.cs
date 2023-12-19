using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay18
{
    [Benchmark]
    public long Part1()
    {
        return Day18.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day18.Part2();
    }
}
