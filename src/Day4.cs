namespace AoC2021;

public static class Day4
{
    public class WinningBoard
    {
        public int BoardNumber;
        public int LastCalledNumber;
        public int SumOfRemainingNumbers;
    }

    private static IEnumerable<string> input = new List<string>();
    private static int[]? callNumbers;
    private static List<int[,]> Boards = new List<int[,]>();
    private static List<int[,]> BoardMatches = new List<int[,]>();
    private static List<WinningBoard> winningBoards = new List<WinningBoard>();

    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }

    private static IEnumerable<int> GetRow(int[,] array, int row)
    {
        for (int i = 0; i <= array.GetUpperBound(1); ++i)
            yield return array[row, i];
    }

    private static IEnumerable<int> GetColumn(int[,] array, int column)
    {
        for (int i = 0; i <= array.GetUpperBound(0); ++i)
            yield return array[i, column];
    }

    private static Task ComputeWinningBoards()
    {
        Console.WriteLine("-- Part 1 --");
        for (int i = 0; i < callNumbers!.Length; i++) {
            var calledNumber = callNumbers[i];
            foreach (var (board, index) in Boards.WithIndex())
            {
                if (winningBoards.Any(wb => wb.BoardNumber == index+1)) continue;
                for (var r = 0; r < 5; r++)
                {
                    for (var c = 0; c < 5; c++)
                    {
                        if (board[r, c] == calledNumber) BoardMatches[index][r, c] = 1;
                    }
                }
                for (var c = 0; c < 5; c++)
                {
                    if (GetRow(BoardMatches[index], c).Sum() != 5 && GetColumn(BoardMatches[index], c).Sum() != 5) continue;
                    var remainingTotal = 0;
                    for (var r = 0; r < 5; r++)
                        for (var cc = 0; cc < 5; cc++)
                        {
                            if (BoardMatches[index][r, cc] != 1) remainingTotal += board[r, cc];
                        }
                    winningBoards.Add(new WinningBoard
                    {
                        BoardNumber = index + 1,
                        LastCalledNumber = calledNumber,
                        SumOfRemainingNumbers = remainingTotal
                    });
                    Console.WriteLine($"Board #{index + 1} Is a winner [Answer: {remainingTotal * calledNumber}]");
                }
            }
        }
        return Task.CompletedTask;
    }

    private static Task PartOne()
    {
        Console.WriteLine("-- Part 1 --");
        var firstWinningBoard = winningBoards.First();
        Console.WriteLine($"Board #{firstWinningBoard.BoardNumber} was the first board to win! The answer is: {firstWinningBoard.LastCalledNumber * firstWinningBoard.SumOfRemainingNumbers}");
        return Task.CompletedTask;
    }

    private static Task PartTwo()
    {
        Console.WriteLine("-- Part 2 --");
        var lastWinningBoard = winningBoards.Last();
        Console.WriteLine($"Board #{lastWinningBoard.BoardNumber} was the last board to win! The answer is: {lastWinningBoard.LastCalledNumber * lastWinningBoard.SumOfRemainingNumbers}");
        return Task.CompletedTask;
    }

    private static int[,] GetBoard(IEnumerable<string> inputstr)
    {
        var board = new int[5,5];
        for (int c = 0; c < 5; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                board[r, c] = int.Parse(inputstr.ElementAt(r).Substring(c * 3, 2).Trim());
            }
        }
        return board;
    }

    private static int[,] NewBoard()
    {
        var result = new int[5,5];
        for (int c = 0;c < 5;c++)
        {
            for (int r = 0;r < 5;r++)
            {
                result[r, c] = 0;
            }
        }
        return result;
    }

    public static async Task Run()
    {
        input = File.ReadLines($"Resources/{nameof(Day4)}.txt");
        callNumbers = input.First().Split(",").Select(n => int.Parse(n.Trim())).ToArray();
        var numberOfBoards = (input.Count() - 1) / 6;
        for (int i = 0; i < numberOfBoards; i++)
        {
            var boardStrings = input.Skip(2+i*6).Take(5);
            var board1 = GetBoard(boardStrings);
            Boards.Add(board1);
            BoardMatches.Add(NewBoard());
        }
        Console.WriteLine("Advent of Code 2021, Day 4!");
        await ComputeWinningBoards();
        await PartOne();
        await PartTwo();
        Console.WriteLine("");
    }
}
