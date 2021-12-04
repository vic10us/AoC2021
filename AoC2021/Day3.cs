using System.Text;

namespace AoC2021;

public static class Day3
{
    private static IEnumerable<string> input = new List<string>();

    private static (string mostCommon, string leastCommon) GetMostAndLeastCommonBit(IEnumerable<string> items, int position)
    {
        var columnValues = items.Select(x => x[position]);
        var c0 = columnValues.Count(c => c.Equals('0'));
        var c1 = columnValues.Count(x => x.Equals('1'));
        return c0 > c1 ? ("0", "1") : ("1", "0");
    }

    private static (int zeroCount, int oneCount) GetCountInColumn(IEnumerable<string> items, int pos)
    {
        var columnValues = items.Select(x => x[pos]);
        var c0 = columnValues.Count(c => c.Equals('0'));
        var c1 = columnValues.Count(x => x.Equals('1'));
        return (c0, c1);
    }

    private static string GetGammaBinaryString(IEnumerable<string> items)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < items.First().Length; i++)
        {
            sb.Append(GetMostAndLeastCommonBit(items, i).mostCommon);
        }
        return sb.ToString();
    }

    private static string GetEpsilonBinaryString(IEnumerable<string> items)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < items.First().Length; i++)
        {
            sb.Append(GetMostAndLeastCommonBit(items, i).leastCommon);
        }
        return sb.ToString();
    }

    private static Task PartOne()
    {
        Console.WriteLine("-- Part 1 --");
        var gamma = Convert.ToInt64(GetGammaBinaryString(input), 2);
        var epsilon = Convert.ToInt64(GetEpsilonBinaryString(input), 2);
        Console.WriteLine($"Gamma: {GetGammaBinaryString(input)} [{gamma}]");
        Console.WriteLine($"Epsilon: {GetEpsilonBinaryString(input)} [{epsilon}]");
        Console.WriteLine($"Answer 1: {gamma * epsilon}");
        return Task.CompletedTask;
    }

    private static IEnumerable<string> FilterByPosition(IEnumerable<string> items, int pos, string defaultValue)
    {
        if (items.Count() <= 1) return items;
        var (zeroes, ones) = GetCountInColumn(items, pos);
        if (zeroes > ones)
            return items.Where(x => x[pos].Equals('0'));
        else 
            return items.Where(x => x[pos].Equals('1'));
    }

    private static IEnumerable<string> FilterByPositionInv(IEnumerable<string> items, int pos, string defaultValue)
    {
        if (items.Count() <= 1) return items;
        var (zeroes, ones) = GetCountInColumn(items, pos);
        if (zeroes > ones)
            return items.Where(x => x[pos].Equals('1'));
        else 
            return items.Where(x => x[pos].Equals('0'));
    }

    private static Task PartTwo()
    {
        Console.WriteLine("-- Part 2 --");
        var oxygenList = new List<string>(input);
        var co2List = new List<string>(input);
        for (int i = 0; i < input.Count(); i++)
        {
            oxygenList = FilterByPosition(oxygenList, i, "1").ToList();
            co2List = FilterByPositionInv(co2List, i, "0").ToList();
        }
        var o2Rating = Convert.ToInt64(oxygenList.First(), 2);
        var co2Rating = Convert.ToInt64(co2List.First(), 2);
        Console.WriteLine($"Oxygen Rating: {oxygenList.First()} [{o2Rating}]");
        Console.WriteLine($"CO2 Rating: {co2List.First()} [{co2Rating}]");
        Console.WriteLine($"Answer 2: {co2Rating * o2Rating}");
        return Task.CompletedTask;
    }

    public static async Task Run()
    {
        input = File.ReadLines("Resources/Day3.txt");
        Console.WriteLine("Advent of Code 2021, Day 3!");
        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
