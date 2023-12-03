using FluentAssertions;

namespace Day1Tests
{
    public class Day1
    {
        [Theory]
        [InlineData("example1.txt", 142)]
        [InlineData("input.txt", 54940)]
        public void Part1(string fileName, int expectedResult)
        {
            var result = Part1Execute(fileName);

            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData("example2.txt", 281)]
        [InlineData("input.txt", 54208)]
        public void Part2(string fileName, int expectedResult)
        {
            var result = Part2Execute(fileName);

            result.Should().Be(expectedResult);
        }



        private int Part1Execute(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            var numbers = new List<int>();

            foreach (var line in lines)
            {
                var first = line.ToCharArray().FirstOrDefault(char.IsNumber);
                var last = line.ToCharArray().LastOrDefault(char.IsNumber);

                char[] chars = { first, last };

                numbers.Add(int.Parse(new string(chars)));
            }

            return numbers.Sum();
        }

        private int Part2Execute(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            List<int> numbers = new();

            foreach (var line in lines)
            {
                var first = line.ToCharArray().FirstOrDefault(char.IsNumber);
                var firstIndex = -1;
                if (first != default)
                {
                    firstIndex = line.IndexOf(first);
                }

                var firstWord = FindFirstWord(line);

                var last = line.ToCharArray().LastOrDefault(char.IsNumber);
                var lastIndex = -1;
                if (last != default)
                {
                    lastIndex = line.LastIndexOf(last);
                }

                var lastWord = FindLastWord(line);

                if (firstIndex == -1 || firstIndex > firstWord.Item2 && firstWord != default)
                {
                    first = ToCharNumber(firstWord.Item1);
                }

                if (lastIndex == -1 || lastIndex < lastWord.Item2 && lastWord != default)
                {
                    last = ToCharNumber(lastWord.Item1);
                }

                char[] chars = { first, last };


                numbers.Add(int.Parse(new string(chars)));
            }

            return numbers.Sum();
        }

        private (string, int) FindFirstWord(string line)
        {
            var list = new List<(string, int)>();
            foreach (var wordNumber in WordNumbers)
            {
                var indexOf = line.IndexOf(wordNumber, StringComparison.Ordinal);
                if (indexOf != -1)
                {
                    list.Add((wordNumber, indexOf));
                }
            }

            list = list.OrderBy(x => x.Item2).ToList();
            return list.FirstOrDefault(x => x.Item2 != -1);
        }

        private (string, int) FindLastWord(string line)
        {
            var list = new List<(string, int)>();
            foreach (var wordNumber in WordNumbers)
            {
                var lastIndexOf = line.LastIndexOf(wordNumber, StringComparison.Ordinal);
                if (lastIndexOf != -1)
                {
                    list.Add((wordNumber, lastIndexOf));
                }
            }
            list = list.OrderByDescending(x => x.Item2).ToList();
            return list.FirstOrDefault(x => x.Item2 != -1);
        }

        private IReadOnlyList<string> WordNumbers => new List<string>()
        {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine"
        };

        private char ToCharNumber(string number) => number switch
        {
            "one" => '1',
            "two" => '2',
            "three" => '3',
            "four" => '4',
            "five" => '5',
            "six" => '6',
            "seven" => '7',
            "eight" => '8',
            "nine" => '9',
            _ => throw new ArgumentOutOfRangeException(nameof(number), $"Not expected Number: {number}"),
        };
    }
}