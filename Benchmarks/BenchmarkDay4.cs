using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay4
{
    [Benchmark]
    public int Part1()
    {
        return Day4.Part1();
    }

    [Benchmark]
    public int Part2()
    {
        return Day4.Part2();
    }
}
