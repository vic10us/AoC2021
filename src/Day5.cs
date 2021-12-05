using System.Text.RegularExpressions;

namespace AoC2021;

public static class Day5
{
    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
                return false;

            Point p = (Point)obj;
            return (x == p.x) && (y == p.y);
        }

        public override int GetHashCode()
        {
            return (x << 2) ^ y;
        }

        public override string ToString()
        {
            return String.Format("Point({0}, {1})", x, y);
        }
    }

    public class Line
    {
        public Point Start;
        public Point End;

        public Line(int x1, int y1, int x2, int y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }

        public Line(string input)
        {
            var matches = LineParser.Matches(input).FirstOrDefault();
            if (matches != null)
            {
                _ = int.TryParse(matches.Groups["x1"].Value, out var x1);
                _ = int.TryParse(matches.Groups["y1"].Value, out var y1);
                _ = int.TryParse(matches.Groups["x2"].Value, out var x2);
                _ = int.TryParse(matches.Groups["y2"].Value, out var y2);
                Start = new Point(x1, y1);
                End= new Point(x2, y2);
            } else
            {
                Start=new Point(0,0);
                End=new Point(0,0);
            }
        }

        public IEnumerable<Point> GetAllPoints()
        {
            // 0,8 -> 8,0
            // 0,8 1,7 2,6 3,5 4,4 3,5 2,6 1,7 0,8
            var lenX = Math.Abs(Start.x - End.x);
            var lenY = Math.Abs(Start.y - End.y);
            var lineLength = Math.Max(lenX, lenY) + 1;
            var xInc = Start.x == End.x ? 0 : Start.x > End.x ? -1 : 1;
            var yInc = Start.y == End.y ? 0 : Start.y > End.y ? -1 : 1;

            for (int p = 0; p < lineLength; p++)
            {
                var x = Start.x + xInc * p;
                var y = Start.y + yInc * p;
                yield return new Point(x, y);
            }
        }
    }

    // example input: 973,543 -> 601,915
    private static readonly Regex LineParser = new("^(?<x1>[0-9]+),(?<y1>[0-9]+)\\s->\\s(?<x2>[0-9]+),(?<y2>[0-9]+)$");
    private static IEnumerable<Line> lines = new List<Line>();

    private static IEnumerable<Line> ParseInput(string filename)
    {
        var input = File.ReadLines(filename);
        var result = input.Select(x => new Line(x)).ToList();
        return result;
    }

    private static void Print(IEnumerable<Point> points)
    {
        var maxX = points.Max(p => p.x);
        var maxY = points.Max(p => p.y);
        int[,] g = new int[maxX + 1, maxY + 1];
        foreach (var point in points) g[point.x, point.y]++;
        for (int y = 0; y <= maxY; y++)
        {
            for (int x = 0; x <= maxX; x++) Console.Write(g[x, y] > 0 ? $"{g[x, y]}" : ".");
            Console.WriteLine();
        }
    }

    private static Task PartOne()
    {
        Console.WriteLine("-- Part 1 --");
        var allPoints = lines.Where(l => l.Start.x == l.End.x || l.Start.y == l.End.y).SelectMany(l => l.GetAllPoints());
        var query = allPoints
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();
        Console.WriteLine($"Total Number of Points in all lines: {allPoints.Count()}");
        Console.WriteLine($"Total Number of Overlapping Points in all lines: {query.Count()}");
        // Print(allPoints);
        return Task.CompletedTask;
    }

    private static Task PartTwo()
    {
        Console.WriteLine("-- Part 2 --");
        var allPoints = lines.SelectMany(l => l.GetAllPoints());
        var query = allPoints
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();
        Console.WriteLine($"Total Number of Points in all lines: {allPoints.Count()}");
        Console.WriteLine($"Total Number of Overlapping Points in all lines: {query.Count()}");
        // Print(allPoints);
        return Task.CompletedTask;
    }

    public static async Task Run()
    {
        Console.WriteLine("Advent of Code 2021, Day 4!");
        lines = ParseInput($"Resources/{nameof(Day5)}.txt");
        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
