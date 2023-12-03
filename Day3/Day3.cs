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
                foreach (var part in partNumbers)
                {
                    if (IsAdjacent(symbol, part))
                    {
                        actualParts.Add(part);
                    }
                }
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

            var actualParts = new List<PartNumber>();

            var symbolToPartNumber = new Dictionary<Symbol, List<PartNumber>>();

            foreach (var symbol in symbols)
            {
                symbolToPartNumber.Add(symbol, new());
                foreach (var part in partNumbers)
                {
                    if (IsAdjacent(symbol, part))
                    {
                        symbolToPartNumber[symbol].Add(part);
                    }
                }
            }

            var ratios = symbolToPartNumber.Where(x => x.Value.Count == 2);

            var values = ratios.Select(x => (x.Value[0].Number * x.Value[1].Number));

            return values.Sum();
        }


        private bool IsAdjacent(Symbol symbol, PartNumber partNumber)
        {
            bool result = false;
            for (int i = 0; i < partNumber.Length; i++)
            {
                result = Math.Abs(symbol.X - (partNumber.X + i)) <= 1 && Math.Abs(symbol.Y - partNumber.Y) <= 1;

                if (result) break;
            }

            return result;
        }

        private List<Symbol> LocateSymbols(string[] lines)
        {
            List<Symbol> symbols = new();
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                    if (!char.IsNumber(lines[i][j]) && lines[i][j] != '.')
                    {
                        symbols.Add(new Symbol(lines[i][j], j, i));
                    }
            }
            return symbols;
        }

        private List<PartNumber> LocatePartNumbers(string[] lines)
        {
            List<PartNumber> parts = new();
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[i].Length; j++)
                    if (char.IsNumber(lines[i][j]))
                    {
                        var lengthOfNumber = LengthOfNumber(lines[i], j);

                        char[] number = new char[lengthOfNumber];

                        for (var length = 0; length < lengthOfNumber; length++)
                        {
                            number[length] = lines[i][j + length];
                        }
                        parts.Add(new PartNumber(int.Parse(new string(number)), j, i, lengthOfNumber));

                        j = j + lengthOfNumber;
                    }
            }
            return parts;
        }

        private int LengthOfNumber(string line, int j)
        {
            int length = 1;

            try
            {
                while (char.IsNumber(line[j + length]) && j + length <= line.Length - 1)
                {
                    length++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }


            return length;
        }


    }

    public record Symbol(char Data, int X, int Y);

    public record PartNumber(int Number, int X, int Y, int Length);
}