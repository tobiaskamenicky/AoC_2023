using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay2
{
    [Benchmark]
    public int Part1()
    {
        return Day2.Part1();
    }

    [Benchmark]
    public int Part2()
    {
        return Day2.Part2();
    }
}
