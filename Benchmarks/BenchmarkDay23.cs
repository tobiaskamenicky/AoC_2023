using AdventOfCode2023;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class BenchmarkDay23
{
    [Benchmark]
    public long Part1()
    {
        return Day23.Part1();
    }

    [Benchmark]
    public long Part2()
    {
        return Day23.Part2();
    }
}
