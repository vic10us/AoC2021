namespace AoC2021;

public static class Day6
{
    public class LanternFish
    {
        public ulong Age { get; set; } = 0;
        public ulong BirthTimer { get; set; } = 8;
        public ulong ChildenPerBirth { get; set; } = 1;
    }

    private static ulong[] lanternFishTracker = new ulong[9];

    // 3,4,3,1,2
    // 8 = 0
    // 7 = 0
    // 6 = 0
    // 5 = 0
    // 4 = 1
    // 3 = 2
    // 2 = 1
    // 1 = 1
    // 0 = 0

    // 8 = 0
    // 7 = 0
    // 6 = 0
    // 5 = 0
    // 4 = 0
    // 3 = 1
    // 2 = 2
    // 1 = 1
    // 0 = 1

    // 8 = 1
    // 7 = 0
    // 6 = 1
    // 5 = 0
    // 4 = 0
    // 3 = 0
    // 2 = 1
    // 1 = 2
    // 0 = 1

    // 8 = 1
    // 7 = 1
    // 6 = 1
    // 5 = 1
    // 4 = 0
    // 3 = 0
    // 2 = 0
    // 1 = 1
    // 0 = 2

    // 8 = 2
    // 7 = 1
    // 6 = 3
    // 5 = 1
    // 4 = 1
    // 3 = 0
    // 2 = 0
    // 1 = 0
    // 0 = 1

    // 2 = 3
    // 1 = 4
    // 1 = 1
    // 1 = 2

    private static List<LanternFish>? lanternFish;

    public static void RunForXDays(int days)
    {
        for (int i = 0; i < days; i++)
        {
            var iterationMax = lanternFish?.Count ?? 0;
            for (int j = 0; j < iterationMax; j++)
            {
                var l = lanternFish![j]!;
                if (l.BirthTimer == 0)
                {
                    l.BirthTimer = 6;
                    lanternFish.Add(new LanternFish());
                    continue;
                }
                l.Age++;
                l.BirthTimer--;
            }
        }
    }

    public static void RunForALotOfDays(int days)
    {
        for (int i = 0;i < days;i++)
        {
            var buffer = new ulong[9];
            var newBirths = lanternFishTracker[0];
            for (int j = 8; j > 0; j--)
            {
                buffer[j - 1] = lanternFishTracker[j];
            }
            buffer[8] = newBirths;
            buffer[6] += newBirths;
            lanternFishTracker = buffer;
        }
    }

    private static Task PartOne()
    {
        Console.WriteLine("-- Part 1 --");
        // Iterate for 80 days!
        RunForXDays(80);
        Console.WriteLine($"Total Number of Latern Fish After 80 days: {lanternFish?.Count ?? 0}");
        return Task.CompletedTask;
    }

    private static Task PartTwo()
    {
        Console.WriteLine("-- Part 2 --");
        RunForALotOfDays(256);
        ulong totalFish = 0;
        foreach (ulong fishItem in lanternFishTracker) totalFish += fishItem;
        Console.WriteLine($"Total Number of Latern Fish After 256 days: {totalFish}");
        return Task.CompletedTask;
    }

    public static async Task Run()
    {
        Console.WriteLine("Advent of Code 2021, Day 6!");
        lanternFish = File.ReadLines("Resources/Day6.txt").First().Split(",").Select(e => {
            var value = ulong.Parse(e);
            var result = new LanternFish { BirthTimer = value, Age = 7 - value, ChildenPerBirth = 1 };
            return result;
        }).ToList();
        foreach (var item in lanternFish)
        {
            int v = (int)item.BirthTimer;
            lanternFishTracker[v]++;
        }

        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
