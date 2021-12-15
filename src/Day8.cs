using System;
using System.Text;
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
            _ = int.TryParse(numStr, out var num);
            total += num;
            Console.WriteLine(RenderDigits(num, 4));
        }

        Console.WriteLine(RenderCharacters($"ANSWER IS: {total}"));

        return Task.CompletedTask;
    }

    private static readonly Dictionary<char, string[]> SegmentCharacters = new Dictionary<char, string[]>()
    {
        { '!',  new[] {"   ", "  |", "   "} },
        { ' ',  new[] {"   ", "   ", "   "} },
        { '"',  new[] {"   ", "| |", "   "} },
        { '#',  new[] {"   ", "| |", "| |"} },
        { '$',  new[] {" _ ", "|_ ", " _ "} },
        { '%',  new[] {" _ ", "|  ", " _|"} },
        { '&',  new[] {" _ ", "|_|", "|_ "} },
        { '\'', new[] {"   ", "|  ", "   "} },
        { '(',  new[] {" _ ", "|  ", "|_ "} },
        { ')',  new[] {" _ ", "  |", " _|"} },
        { '*',  new[] {" _ ", "|_|", "   "} },
        { '+',  new[] {"   ", "|_ ", "|  "} },
        { ',',  new[] {"   ", "   ", " _|"} },
        { '-',  new[] {"   ", " _ ", "   "} },
        { '[',  new[] {" _ ", "|  ", "|_ "} },
        { '\\', new[] {"   ", "|_ ", "   "} },
        { ']',  new[] {" _ ", "  |", " _|"} },
        { '^',  new[] {" _ ", "| |", "   "} },
        { '_',  new[] {"   ", "   ", " _ "} },
        { '/',  new[] {"   ", " _|", "   "} },
        { '0',  new[] {" _ ", "| |", "|_|"} },
        { '1',  new[] {"   ", "  |", "  |"} },
        { '2',  new[] {" _ ", " _|", "|_ "} },
        { '3',  new[] {" _ ", " _|", " _|"} },
        { '4',  new[] {"   ", "|_|", "  |"} },
        { '5',  new[] {" _ ", "|_ ", " _|"} },
        { '6',  new[] {" _ ", "|_ ", "|_|"} },
        { '7',  new[] {" _ ", "  |", "  |"} },
        { '8',  new[] {" _ ", "|_|", "|_|"} },
        { '9',  new[] {" _ ", "|_|", " _|"} },
        { ':',  new[] {"   ", " _ ", " _ "} },
        { ';',  new[] {"   ", " _ ", " _|"} },
        { '<',  new[] {" _ ", "|_ ", "   "} },
        { '=',  new[] {" _ ", " _ ", "   "} },
        { '>',  new[] {" _ ", " _|", "   "} },
        { '?',  new[] {" _ ", " _|", "|  "} },
        { '@',  new[] {" _ ", " _|", "|_|"} },
        { 'A',  new[] {" _ ", "|_|", "| |"} },
        { 'a',  new[] {" _ ", "|_|", "| |"} },
        { 'B',  new[] {" _ ", "|_|", "|_|"} },
        { 'b',  new[] {"   ", "|_ ", "|_|"} },
        { 'C',  new[] {" _ ", "|  ", "|_ "} },
        { 'c',  new[] {"   ", " _ ", "|_ "} },
        { 'D',  new[] {" _ ", "| |", "|_|"} },
        { 'd',  new[] {"   ", " _|", "|_|"} },
        { 'E',  new[] {" _ ", "|_ ", "|_ "} },
        { 'e',  new[] {" _ ", "|_ ", "|_ "} },
        { 'F',  new[] {" _ ", "|_ ", "|  "} },
        { 'f',  new[] {" _ ", "|_ ", "|  "} },
        { 'G',  new[] {" _ ", "|  ", "|_|"} },
        { 'g',  new[] {" _ ", "|  ", "|_|"} },
        { 'H',  new[] {"   ", "|_|", "| |"} },
        { 'h',  new[] {"   ", "|_ ", "| |"} },
        { 'I',  new[] {"   ", "|  ", "|  "} },
        { 'i',  new[] {"   ", "   ", "|  "} },
        { 'J',  new[] {"   ", "  |", "|_|"} },
        { 'j',  new[] {"   ", "  |", "|_|"} },
        { 'K',  new[] {" _ ", "|_ ", "| |"} },
        { 'k',  new[] {" _ ", "|_ ", "| |"} },
        { 'L',  new[] {"   ", "|  ", "|_ "} },
        { 'l',  new[] {"   ", "|  ", "|  "} },
        { 'M',  new[] {" _ ", "   ", "| |"} },
        { 'm',  new[] {" _ ", "   ", "| |"} },
        { 'N',  new[] {" _ ", "| |", "| |"} },
        { 'n',  new[] {"   ", " _ ", "| |"} },
        { 'O',  new[] {" _ ", "| |", "|_|"} },
        { 'o',  new[] {"   ", " _ ", "|_|"} },
        { 'P',  new[] {" _ ", "|_|", "|  "} },
        { 'p',  new[] {" _ ", "|_|", "|  "} },
        { 'Q',  new[] {" _ ", "|_|", "  |"} },
        { 'q',  new[] {" _ ", "|_|", "  |"} },
        { 'R',  new[] {" _ ", "| |", "|  "} },
        { 'r',  new[] {"   ", " _ ", "|  "} },
        { 'S',  new[] {" _ ", "|_ ", " _|"} },
        { 's',  new[] {" _ ", "|_ ", " _|"} },
        { 'T',  new[] {"   ", "|_ ", "|_ "} },
        { 't',  new[] {"   ", "|_ ", "|_ "} },
        { 'U',  new[] {"   ", "| |", "|_|"} },
        { 'u',  new[] {"   ", "   ", "|_|"} },
        { 'V',  new[] {"   ", "| |", " _|"} },
        { 'v',  new[] {"   ", "| |", " _|"} },
        { 'W',  new[] {"   ", "| |", " _ "} },
        { 'w',  new[] {"   ", "| |", " _ "} },
        { 'X',  new[] {"   ", "|_|", "| |"} },
        { 'x',  new[] {"   ", "|_|", "| |"} },
        { 'Y',  new[] {"   ", "|_|", " _|"} },
        { 'y',  new[] {"   ", "|_|", " _|"} },
        { 'Z',  new[] {" _ ", " _|", " _ "} },
        { 'z',  new[] {" _ ", " _|", " _ "} },
    };

    private static string RenderCharacters(string characters)
    {
        return RenderCharacters(characters.Select(c => c).ToArray());
    } 

    private static string RenderCharacters(char[] characters)
    {
        var sb = new StringBuilder();
        for (var r = 0; r < 3; r++)
        {
            for (var i = 0; i < characters.Length; i++) 
            { 
                var c = characters[i];
                if (c == '.') continue;
                sb.Append(SegmentCharacters.ContainsKey(c) ? SegmentCharacters[c][r] : SegmentCharacters['-'][r]);
                if ((c == '!' || (i +1 < characters.Length && characters[i+1]=='.')) && r == 2) sb.Append('.');
                else sb.Append(' ');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static string RenderDigits(int digit, int padding = 4)
    {
        var digitString = $"{digit}";
        if (padding > 0 && padding > digitString.Length)
        {
            var p = new string(Enumerable.Range(0, padding - digitString.Length).Select(e => '0').ToArray());
            digitString = $"{p}{digitString}";
        }
        var characters = digitString.Select(c => c).ToArray();
        return RenderCharacters(characters);
    }

    public static async Task Run()
    {
        Console.WriteLine(RenderCharacters("Advent of Code 2021, Day 8!"));
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
        Console.WriteLine(RenderCharacters("ALL DONE!"));
        Console.WriteLine("");
    }
}
