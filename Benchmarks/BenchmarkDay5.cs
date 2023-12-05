using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay5
{
    [Benchmark]
    public long Part1()
    {
        return Day5.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day5.Part2();
    }
}
