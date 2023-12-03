using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay3
{
    [Benchmark]
    public int Part1()
    {
        return Day3.Part1();
    }

    [Benchmark]
    public int Part2()
    {
        return Day3.Part2();
    }
}
