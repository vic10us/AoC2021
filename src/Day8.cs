using System;
using System.Threading.Tasks;

namespace AoC2021;

public static class Day8
{
    public class SegPair
    {
        public string[] Input { get; set; }
        public string[] Output { get; set; }
    }

    public static List<SegPair> SegPairs { get; set; }

    private static Task PartOne()
    {
        Console.WriteLine("-- Part 1 --");
        // Count the occurences of any Output element length is equal to 1, 4, 7 or 8...
        var easyCount = SegPairs.SelectMany(x => x.Output).Count(e => e.Length == 2 || e.Length == 3 || e.Length == 4 || e.Length == 7);
        Console.WriteLine($"The count of easily determined 7 segment numbers in the output is: {easyCount}");
        return Task.CompletedTask;
    }

    public static Dictionary<int, string> GetCodex(SegPair pair)
    {
        var codex = new Dictionary<int, string>();
        codex[1] = pair.Input.Single(e => e.Length == 2);
        codex[4] = pair.Input.Single(e => e.Length == 4);
        codex[7] = pair.Input.Single(e => e.Length == 3);
        codex[8] = pair.Input.Single(e => e.Length == 7);
        var p235 = pair.Input.Where(e => e.Length == 5);
        var p069 = pair.Input.Where(e => e.Length == 6);

        var segments = new Dictionary<char, char>();
        // a = the difference between 1 and 7
        foreach (var c in codex[7])
        {
            if (!codex[1].Any(a => a == c)) segments['a'] = c;
        }

        // b and d as being the difference between 4 and 1
        var bdp = codex[4].Select(p => p).Except(codex[1].Select(p => p));
        foreach (var p in bdp)
        {
            if (p069.Count(e => e.Contains(p)) == 2) segments['d'] = p;
            else segments['b'] = p;
        }

        // codex for 5 is the only codex that contains all three known positions of a,b,d
        codex[5] = p235.Single(p => segments.Values.All(s => p.Contains(s)));
        var p23 = p235.Where(p => p != codex[5]);

        // segment f should be the only common char between 1 and 5
        // knowing that, c is the other.
        // segments['f'] = codex[5].First(c => codex[1].Any(c1 => c1 == c));
        segments['f'] = codex[5].Intersect(codex[1]).First();
        segments['c'] = codex[1].First(c => c != segments['f']);

        codex[0] = p069.First(p => !p.Contains(segments['d']));
        codex[6] = p069.First(p => !p.Contains(segments['c']));
        codex[9] = p069.Except(new string[] { codex[0], codex[6] }).First();

        segments['g'] = codex[9].Except(segments.Values).First();
        segments['e'] = codex[8].Except(segments.Values).First();

        codex[2] = p23.First(p => p.Contains(segments['e']));
        codex[3] = p23.First(p => p.Contains(segments['f']));

        return codex;
    }

    private static Task PartTwo()
    {
        Console.WriteLine("-- Part 2 --");

        var total = 0;

        foreach (var pair in SegPairs) 
        {
            var codex = GetCodex(pair);

            string numStr = string.Empty;
            foreach (var outval in pair.Output)
            {
                var outsort = string.Concat(outval.OrderBy(c => c));
                var match = codex.FirstOrDefault(c => string.Concat(c.Value.OrderBy(v => v)) == outsort).Key;
                numStr += match.ToString();
            }
            int.TryParse(numStr, out var num);
            total += num;
            Console.WriteLine(num);
        }

        Console.WriteLine($"Total of the output is: {total}");

        return Task.CompletedTask;
    }

    public static async Task Run()
    {
        Console.WriteLine("Advent of Code 2021, Day 8!");
        SegPairs = File.ReadLines("Resources/Day8.txt").Select(e => {
            var lr = e.Split('|');
            return new SegPair
            {
                Input = lr[0].Trim().Split(' '),
                Output = lr[1].Trim().Split(' ')
            };
        }).ToList();
        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
