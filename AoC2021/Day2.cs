using System.Text.RegularExpressions;

namespace AoC2021;

public static class Day2
{
    private static IEnumerable<string> input = new List<string>();

    private static readonly Regex regexp = new("^(?<direction>[a-z]+)\\s(?<amount>[0-9]+)$");

    private static (string direction, int amount) ParseCommand(string command)
    {
        var x = regexp.Matches(command).FirstOrDefault();
        if (x == null) return (string.Empty, 0);
        var direction = x.Groups["direction"].Value;
        _ = int.TryParse(x.Groups["amount"].Value, out var amount);
        return (direction, amount);
    }

    private static Task PartOne()
    {
        var (x, y) = (0, 0);
        var commands = input.Select(i => ParseCommand(i));
        x = commands.Where(c => c.direction.Equals("forward")).Select(c => c.amount).Sum();
        y = commands.Where(c => !c.direction.Equals("forward")).Select(c => c.direction == "up" ? c.amount * -1 : c.amount).Sum();
        Console.WriteLine("-- Part 1 --");
        Console.WriteLine($"X: {x} Y: {y}");
        Console.WriteLine($"Answer 1: {x * y}");
        return Task.CompletedTask;
    }

    private static Task PartTwo()
    {
        Console.WriteLine("-- Part 2 --");
        var commands = input.Select(i => ParseCommand(i));
        var (x, y, aim) = (0, 0, 0);
        foreach (var (direction, amount) in commands)
        {
            switch (direction)
            {
                case "down":
                    aim += amount;
                    break;
                case "up":
                    aim -= amount;
                    break;
                case "forward":
                    x += amount;
                    y += amount * aim;
                    break;
            }
        }
        Console.WriteLine($"X: {x} Y: {y} Aim: {aim}");
        Console.WriteLine($"Answer 2: {x * y}");
        return Task.CompletedTask;
    }

    public static async Task Run()
    {
        input = File.ReadLines("Resources/Day2.txt");
        Console.WriteLine("Advent of Code 2021, Day 2!");
        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
