using FluentAssertions;

namespace Day3
{
    public class Day3
    {
        [Theory]
        [InlineData("example.txt", 4361)]
        [InlineData("input.txt", 533784)]
        public void Part1(string fileName, int expectedResult)
        {
            var result = Part1Execute(fileName);

            result.Should().Be(expectedResult);
        }

        private int Part1Execute(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var symbols = LocateSymbols(lines);

            var partNumbers = LocatePartNumbers(lines);

            var actualParts = new List<PartNumber>();

            foreach (var symbol in symbols)
            {
                actualParts.AddRange(partNumbers.Where(part => IsAdjacent(symbol, part)));
            }


            return actualParts.Sum(x => x.Number);
        }


        [Theory]
        [InlineData("example.txt", 467835)]
        [InlineData("input.txt", 78826761)]
        public void Part2(string fileName, int expectedResult)
        {
            var result = Part2Execute(fileName);

            result.Should().Be(expectedResult);
        }

        private int Part2Execute(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var symbols = LocateSymbols(lines).Where(x => x.Data == '*');

            var partNumbers = LocatePartNumbers(lines);

            var symbolToPartNumber = new Dictionary<Symbol, List<PartNumber>>();

            foreach (var symbol in symbols)
            {
                symbolToPartNumber.Add(symbol, new());

                foreach (var part in partNumbers.Where(part => IsAdjacent(symbol, part)))
                {
                    symbolToPartNumber[symbol].Add(part);
                }
            }

            return symbolToPartNumber.Where(x => x.Value.Count == 2)
                .Select(x => (x.Value[0].Number * x.Value[1].Number))
                .Sum();
        }


        private bool IsAdjacent(Symbol symbol, PartNumber partNumber)
        {
            var result = false;
            for (var i = 0; i < partNumber.Length; i++)
            {
                result = Math.Abs(symbol.X - (partNumber.X + i)) <= 1 && Math.Abs(symbol.Y - partNumber.Y) <= 1;

                if (result) break;
            }

            return result;
        }

        private List<Symbol> LocateSymbols(string[] lines)
        {
            List<Symbol> symbols = new();
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                    if (!char.IsNumber(lines[y][x]) && lines[y][x] != '.')
                    {
                        symbols.Add(new Symbol(lines[y][x], x, y));
                    }
            }
            return symbols;
        }

        private List<PartNumber> LocatePartNumbers(string[] lines)
        {
            List<PartNumber> parts = new();
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                    if (char.IsNumber(lines[y][x]))
                    {
                        var lengthOfNumber = LengthOfNumber(lines[y], x);

                        var number = new char[lengthOfNumber];

                        for (var length = 0; length < lengthOfNumber; length++)
                        {
                            number[length] = lines[y][x + length];
                        }
                        parts.Add(new PartNumber(int.Parse(new string(number)), x, y, lengthOfNumber));

                        x += lengthOfNumber;
                    }
            }
            return parts;
        }

        private int LengthOfNumber(string line, int x)
        {
            var length = 1;
            while (x + length < line.Length && char.IsNumber(line[x + length]))
            {
                length++;
            }
            return length;
        }


    }

    public record Symbol(char Data, int X, int Y);

    public record PartNumber(int Number, int X, int Y, int Length);
}