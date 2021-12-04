namespace AoC2021;

public static class Day1
{
    public static Task Run()
    {
        var input = File.ReadLines("Resources/Day1.txt").Select(l => int.Parse(l)).ToArray();
        Console.WriteLine("Advent of Code 2021, Day 1!");
        Console.WriteLine("-- Part 1 --");
        Console.WriteLine($"Total Entries: {input.Length}");
        Console.WriteLine($"Answer 1: {CountUpInArr(input)}");
        Console.WriteLine("-- Part 2 --");
        Console.WriteLine($"Total Entries 2: {SumByThree(input).Count()}");
        Console.WriteLine($"Answer 2: {CountUpInArr(SumByThree(input))}");
        Console.WriteLine("");
        return Task.CompletedTask;
    }

    private static IEnumerable<int> SumByThree(IEnumerable<int> input)
    {
        var buffer = new int[input.Count() - 2];
        for (int i = 0; i < input.Count() - 2; i++)
        {
            buffer[i] = input.Skip(i).Take(3).Sum();
        }
        return buffer;
    }

    private static int CountUpInArr(IEnumerable<int> input)
    {
        var countUp = 0;
        for (int i = 1; i < input.Count(); i++)
        {
            if (input.ElementAt(i) > input.ElementAt(i - 1)) { countUp++; }
        }
        return countUp;
    }
}
