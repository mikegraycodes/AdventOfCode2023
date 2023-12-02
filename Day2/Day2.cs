using FluentAssertions;
using Microsoft.VisualBasic;

namespace Day2
{
    public class Day2
    {
        [Theory]
        [InlineData("example.txt", 8)]
        [InlineData("input.txt", 2476)]
        public void Part1(string fileName, int expectedResult)
        {
            var result = Part1Execute(fileName);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("example.txt", 2286)]
        [InlineData("input.txt", 54911)]
        public void Part2(string fileName, int expectedResult)
        {
            var result = Part2Execute(fileName);

            result.Should().Be(expectedResult);
        }
        

        private int Part1Execute(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            List<int> ints = new();


            foreach (var line in lines)
            {
                bool valid = true;
                var game = new Game(line);

                foreach (var set in game.Sets)
                {
                    if (set.Entries.Where(x => x.Colour.Contains("red"))
                            .Sum(x => x.Number) > 12
                        || set.Entries.Where(x => x.Colour.Contains("green"))
                            .Sum(x => x.Number) > 13
                        || set.Entries.Where(x => x.Colour.Contains("blue"))
                            .Sum(x => x.Number) > 14)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                {
                    ints.Add(game.Id);
                }
            }

            return ints.Sum();
        }

        private int Part2Execute(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            List<int> ints = new();

            foreach (var line in lines)
            {
                var game = new Game(line);

                var minimumReds = new List<int>();
                var minimumGreen = new List<int>();
                var minimumBlue = new List<int>();

                foreach (var set in game.Sets)
                {
                    minimumReds.Add(set.Entries.Where(x => x.Colour.Contains("red")).Sum(x => x.Number));
                    minimumGreen.Add(set.Entries.Where(x => x.Colour.Contains("green")).Sum(x => x.Number));
                    minimumBlue.Add(set.Entries.Where(x => x.Colour.Contains("blue")).Sum(x => x.Number));
                }

                var minRed = minimumReds.Max();
                var minGreen = minimumGreen.Max();
                var minBlue = minimumBlue.Max();

                ints.Add(minRed * minGreen * minBlue);
            }

            return ints.Sum();
        }
    }

    public class Game(string line)
    {
        public int Id => int.Parse(line.Split(":").First().Split(" ").Last());
        public IReadOnlyList<Set> Sets => line.Split(":").Last().Split(";")
            .Select(x => new Set(x.Split(",")
                .Select(entry => new Entry(int.Parse(entry.Trim().Split(" ").First()), entry.Trim().Split(" ").Last())).ToList())).ToList();
    }

    public record Entry(int Number, string Colour);

    public record Set(List<Entry> Entries);
}