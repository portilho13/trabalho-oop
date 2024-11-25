namespace trabalho_oop.Tests
{
    [TestFixture]
    public class NumberGeneratorTests
    {
        [Test]
        public void GenerateRandomNumber_ReturnsStringOfLength6()
        {
            string randomNumber = NumberGenerator.GenerateRandomNumber();
            Assert.That(randomNumber.Length, Is.EqualTo(6));
        }

        [Test]
        public void GenerateRandomNumber_ContainsOnlyUppercaseLettersAndDigits()
        {
            string randomNumber = NumberGenerator.GenerateRandomNumber();
            foreach (char c in randomNumber)
            {
                Assert.That(char.IsLetterOrDigit(c), Is.True);
                Assert.That(char.IsUpper(c), Is.True);
            }
        }

        [Test]
        public void GenerateRandomNumber_GeneratesUniqueValues() // Only tested with 2 values to not kill performance
        {
            string firstNumber = NumberGenerator.GenerateRandomNumber();
            string secondNumber = NumberGenerator.GenerateRandomNumber();
            Assert.That(firstNumber, Is.Not.EqualTo(secondNumber));
        }
    }
}