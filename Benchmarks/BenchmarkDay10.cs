using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay10
{
    [Benchmark]
    public long Part1()
    {
        return Day10.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day10.Part2();
    }
}
