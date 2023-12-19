using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay17
{
    [Benchmark]
    public long Part1()
    {
        return Day17.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day17.Part2();
    }
}
