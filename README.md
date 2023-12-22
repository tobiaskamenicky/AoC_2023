# Advent of Code 2023

<https://adventofcode.com/2023>

This time boring in C# :)

## Overview

Measured times\memory allocations do not include loading input nor displaying output.

 |                                        Day | Part | Mean Time | Allocated memory |
 |-------------------------------------------:| ---: |----------:|-----------------:|
 |   [1](https://adventofcode.com/2023/day/1) | Part 1 |   9.02 µs |              0 B |
 |                                            | Part 2 |  70.70 µs |              0 B |
 |   [2](https://adventofcode.com/2023/day/2) | Part 1 |  11.77 µs |              0 B |
 |                                            | Part 2 |  12.60 µs |              0 B |
 |   [3](https://adventofcode.com/2023/day/3) | Part 1 |  23.53 µs |              0 B |
 |                                            | Part 2 |  14.74 µs |              0 B |
 |   [4](https://adventofcode.com/2023/day/4) | Part 1 |  57.07 µs |              0 B |
 |                                            | Part 2 |  60.48 µs |              0 B |
 |   [5](https://adventofcode.com/2023/day/5) | Part 1 |  13.30 µs |              0 B |
 |                                            | Part 2 |  48.88 µs |              0 B |
 |   [6](https://adventofcode.com/2023/day/6) | Part 1 |  84.20 ns |              0 B |
 |                                            | Part 2 |  98.61 ns |              0 B |
 |   [7](https://adventofcode.com/2023/day/7) | Part 1 | 116.60 µs |         15.68 KB |
 |                                            | Part 2 | 116.70 µs |         15.68 KB |
 |   [8](https://adventofcode.com/2023/day/8) | Part 1 |   5.86 ms |              3 B |
 |                                            | Part 2 | 974.60 µs |         48.10 KB |
 |   [9](https://adventofcode.com/2023/day/9) | Part 1 |  51.64 µs |         21.88 KB |
 |                                            | Part 2 |  54.33 µs |         21.88 KB |
 | [10](https://adventofcode.com/2023/day/10) | Part 1 |  59.12 µs |              0 B |
 |                                            | Part 2 | 293.43 µs |        538.66 KB |
 | [11](https://adventofcode.com/2023/day/11) | Part 1 | 233.60 µs |          8.19 KB |
 |                                            | Part 2 | 233.60 µs |          8.19 KB |
 | [12](https://adventofcode.com/2023/day/12) | Part 1 | 345.70 µs |              0 B |
 |                                            | Part 2 | 161.15 ms |        279.83 KB |
 | [13](https://adventofcode.com/2023/day/13) | Part 1 |  15.17 µs |              0 B |
 |                                            | Part 2 |  18.26 µs |              0 B |
 | [14](https://adventofcode.com/2023/day/14) | Part 1 |   6.90 µs |              0 B |
 |                                            | Part 2 | 121.32 ms |        358.72 KB |
 | [15](https://adventofcode.com/2023/day/15) | Part 1 |  21.43 µs |              0 B |
 |                                            | Part 2 |  80.41 µs |        374.17 KB |
 | [16](https://adventofcode.com/2023/day/16) | Part 1 | 511.10 µs |          1.21 MB |
 |                                            | Part 2 | 100.61 ms |        239.08 MB |
 | [17](https://adventofcode.com/2023/day/17) | Part 1 |   2.26  s |          1.48 GB |
 |                                            | Part 2 |   9.31  s |          5.00 GB |
 | [18](https://adventofcode.com/2023/day/18) | Part 1 | 904.90 µs |        222.01 KB |
 |                                            | Part 2 | 315.90 µs |        526.16 KB |
 | [19](https://adventofcode.com/2023/day/19) | Part 1 | 198.70 µs |        615.17 KB |
 |                                            | Part 2 | 185.90 µs |       1166.61 KB |
 | [20](https://adventofcode.com/2023/day/20) | Part 1 |   2.14 ms |        801.93 KB |
 |                                            | Part 2 |  28.55 ms |          6.10 MB |
 | [21](https://adventofcode.com/2023/day/21) | Part 1 |   1.99 ms |          3.24 MB |
 |                                            | Part 2 | 830.40 ms |        549.26 MB |
 | [22](https://adventofcode.com/2023/day/22) | Part 1 |  17.86 ms |           706 KB |
 |                                            | Part 2 | 217.20 ms |        220.10 MB |


Using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)
```
 Measured on
   OS: Windows 11 Pro x64
   CPU: Intel(R) Core(TM) i9-13900HXH @ 2.20GHz
   RAM: 32GB
 ```
