using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace AoC2023.Day2;

public class Solution
{
    private readonly int[] Limits = { 12, 13, 14 };
    private readonly string[] Lines;

    public Solution()
    {
        using var file = new FileInfo("Input/day2.txt").OpenText();
        Lines = file.ReadToEnd().Split("\r\n");
    }

    [Benchmark]
    public (int Part1, int Part2) Solve()
    {

        var validGames = 0;
        var powerOfAll = 0;
        foreach (var line in Lines)
        {
            var split = line.Split(":");
            var game = int.Parse(split[0].Split(" ")[1]);

            var contains = new List<int>(3) { 0, 0, 0 };
            foreach (var set in split[1].Split(";"))
            {
                foreach (var content in set.Split(", "))
                {
                    var c = content.TrimStart().Split(" ");
                    var num = int.Parse(c[0]);

                    var idx = c[1] switch
                    {
                        "red" => 0,
                        "green" => 1,
                        _ => 2
                    };

                    if (contains[idx] < num)
                        contains[idx] = num;
                }
            }

            validGames += IsValidGame(contains, game);
            powerOfAll += contains[0] * contains[1] * contains[2];
        }

        return (validGames, powerOfAll);
    }

    private int IsValidGame(List<int> contains, int game)
    {
        for (var i = 0; i <= 2; i++)
            if (contains[i] > Limits[i])
                return 0;

        return game;
    }
}

public class Day2
{
    public static void Run()
    {
        var solved = new Solution().Solve();
        Console.WriteLine($"Part1: {solved.Part1} Part2 {solved.Part2}");

        BenchmarkRunner.Run<Solution>();
    }
}
