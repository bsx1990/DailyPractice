using NUnit.Framework;

namespace FizzPuzzWhizz.Test
{
    [TestFixture]
    public class FizzPuzzWhizzTest
    {
        [TestCase(1, ExpectedResult = "1")]
        [TestCase(2, ExpectedResult = "2")]
        [TestCase(3, ExpectedResult = "Fizz")]
        [TestCase(6, ExpectedResult = "Fizz")]
        [TestCase(5, ExpectedResult = "Buzz")]
        [TestCase(7, ExpectedResult = "Whizz")]
        [TestCase(13, ExpectedResult = "Fizz")]
        [TestCase(15, ExpectedResult = "FizzBuzz")]
        [TestCase(21, ExpectedResult = "FizzWhizz")]
        [TestCase(35, ExpectedResult = "Fizz")]
        [TestCase(51, ExpectedResult = "Fizz")]
        [TestCase(70, ExpectedResult = "BuzzWhizz")]
        [TestCase(105, ExpectedResult = "FizzBuzzWhizz")]
        public string ShouldGetExpectedResult(int input)
        {
            return new FizzPuzzWhizz().Translate(input);
        }
    }
}
