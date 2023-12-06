using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay6
{
    [Benchmark]
    public long Part1()
    {
        return Day6.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day6.Part2();
    }
}
