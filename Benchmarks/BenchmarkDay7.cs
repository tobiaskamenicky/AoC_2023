using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay7
{
    [Benchmark]
    public long Part1()
    {
        return Day7.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day7.Part2();
    }
}
