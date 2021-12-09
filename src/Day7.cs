using System;
using System.Threading.Tasks;

namespace AoC2021;

public static class Day7
{
    private static int[] horizontalPositions;

    private static Task PartOne()
    {
        Console.WriteLine("-- Part 1 --");
        var min = horizontalPositions.Min();
        var max = horizontalPositions.Max();
        var cheapeastCost = int.MaxValue;
        var cheapestPosition = int.MaxValue;
        for (var i = 0; i < max; i++)
        {
            // e = 0
            // i = 2
            // 0 - 2 = -2 (2)
            var cost = horizontalPositions.Sum(e => Math.Abs(e-i));
            if (cost < cheapeastCost)
            {
                cheapeastCost = cost;
                cheapestPosition = i;
            }
        }
        Console.WriteLine($"Cheapest Position is {cheapestPosition} and would cost {cheapeastCost} fuel!");
        return Task.CompletedTask;
    }

    private static Task PartTwo()
    {
        // Example expectations:
        // 168 fuel position of 5
        // Each move cost an additional 1 fuel...
        Console.WriteLine("-- Part 2 --");
        var min = horizontalPositions.Min();
        var max = horizontalPositions.Max();
        var cheapeastCost = int.MaxValue;
        var cheapestPosition = int.MaxValue;
        for (var i = 0; i < max; i++)
        {
            // e = 0
            // i = 2
            // 0 - 2 = (2*3)
            var cost = horizontalPositions.Where(e => Math.Abs(e - i) > 0).Sum(e => Enumerable.Range(1, Math.Abs(e - i)).Sum());
            if (cost < cheapeastCost)
            {
                cheapeastCost = cost;
                cheapestPosition = i;
            }
        }
        Console.WriteLine($"Cheapest Position is {cheapestPosition} and would cost {cheapeastCost} fuel!");
        return Task.CompletedTask;
    }

    public static async Task Run()
    {
        Console.WriteLine("Advent of Code 2021, Day 7!");
        horizontalPositions = File.ReadLines("Resources/Day7.txt").First().Split(",").Select(e => int.Parse(e)).ToArray();
        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
