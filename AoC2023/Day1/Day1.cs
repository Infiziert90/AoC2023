﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AoC2023.Day1;

public class Solution
{
    private readonly (string, string)[] Words = {
        ("one", "1"), ("two", "2"), ("three", "3"), ("four", "4"), ("five", "5"),
        ("six", "6"), ("seven", "7"), ("eight", "8"), ("nine", "9")
    };

    private readonly string[] Lines;

    public Solution()
    {
        using var file = new FileInfo("Input/day1.txt").OpenText();
        Lines = file.ReadToEnd().Split("\r\n");
    }

    [Benchmark]
    public long Solve()
    {
        return Lines.AsParallel().Select(line =>
        {
            var idxNum = new List<(int, string)>();
            foreach (var (word, num) in Words)
            {
                var pos = -1;
                while ((pos = line.IndexOf(word, pos + 1, StringComparison.InvariantCulture)) != -1)
                    idxNum.Add((pos, num));
            }

            var lineEdited = line;
            foreach (var ((pos, num), iteration) in idxNum.OrderBy(tuple => tuple.Item1).Select((var, n) => (var, n)))
                lineEdited = lineEdited.Insert(pos + iteration, num);

            var numbers = new string(lineEdited.Where(char.IsDigit).ToArray());
            return (numbers[0] - '0') * 10 + (numbers[^1] - '0');
        }).Sum();
    }
}

public class Day1
{
    public static void Run()
    {
        Console.WriteLine(new Solution().Solve());

        BenchmarkRunner.Run<Solution>();
    }
}
