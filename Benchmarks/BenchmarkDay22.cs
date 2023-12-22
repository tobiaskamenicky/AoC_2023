using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay22
{
    [Benchmark]
    public long Part1()
    {
        return Day22.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day22.Part2();
    }
}
