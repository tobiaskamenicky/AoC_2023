using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay9
{
    [Benchmark]
    public long Part1()
    {
        return Day9.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day9.Part2();
    }
}
