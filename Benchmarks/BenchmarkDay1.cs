using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay1
{
    [Benchmark]
    public int Part1()
    {
        return Day1.Part1();
    }

    [Benchmark]
    public int Part2()
    {
        return Day1.Part2();
    }
}
