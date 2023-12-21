using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay21
{
    [Benchmark]
    public long Part1()
    {
        return Day21.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day21.Part2();
    }
}
