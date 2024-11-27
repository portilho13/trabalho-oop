using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using trabalho_oop;

namespace trabalho_oop.Tests
{
    [TestFixture]
    public class LoggerTests
    {
        private string _testLogFilePath;
        private Logger _logger;
        private StringWriter _consoleOutput;
        private TextWriter _originalConsoleOutput;

        [SetUp]
        public void Setup()
        {
            // Create a unique log file path for each test
            _testLogFilePath = Path.Combine(Path.GetTempPath(), $"test_log_{Guid.NewGuid()}.txt");
            _logger = new Logger(_testLogFilePath);

            // Redirect console output
            _consoleOutput = new StringWriter();
            _originalConsoleOutput = Console.Out;
            Console.SetOut(_consoleOutput);
        }

        [TearDown]
        public void Teardown()
        {
            // Restore console output
            Console.SetOut(_originalConsoleOutput);

            // Dispose of the StringWriter
            _consoleOutput.Dispose();

            // Clean up the test log file
            if (File.Exists(_testLogFilePath))
            {
                File.Delete(_testLogFilePath);
            }
        }

        [Test]
        public void Constructor_ValidPath_CreatesLogFile()
        {
            Assert.That(File.Exists(_testLogFilePath), Is.True);
        }

        [Test]
        public void Constructor_NullOrWhitespacePath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Logger(null));
            Assert.Throws<ArgumentException>(() => new Logger(""));
            Assert.Throws<ArgumentException>(() => new Logger("   "));
        }

        [Test]
        public void Info_LogsMessageCorrectly()
        {
            string testMessage = "Test info message";
            _logger.Info(testMessage);

            // Check console output
            string consoleOutput = _consoleOutput.ToString().Trim();
            Assert.That(consoleOutput, Is.EqualTo($"INFO: {testMessage}"));

            // Check file content
            string fileContent = File.ReadAllText(_testLogFilePath);
            Assert.That(fileContent, Does.Contain($"INFO: {testMessage}"));
        }

        [Test]
        public void Warn_LogsMessageCorrectly()
        {
            string testMessage = "Test warning message";
            _logger.Warn(testMessage);

            // Check console output
            string consoleOutput = _consoleOutput.ToString().Trim();
            Assert.That(consoleOutput, Is.EqualTo($"WARN: {testMessage}"));

            // Check file content
            string fileContent = File.ReadAllText(_testLogFilePath);
            Assert.That(fileContent, Does.Contain($"WARN: {testMessage}"));
        }

        [Test]
        public void Error_LogsMessageCorrectly()
        {
            string testMessage = "Test error message";
            _logger.Error(testMessage);

            // Check console output
            string consoleOutput = _consoleOutput.ToString().Trim();
            Assert.That(consoleOutput, Is.EqualTo($"ERROR: {testMessage}"));

            // Check file content
            string fileContent = File.ReadAllText(_testLogFilePath);
            Assert.That(fileContent, Does.Contain($"ERROR: {testMessage}"));
        }

        [Test]
        public void LogFile_ContainsTimestamp()
        {
            _logger.Info("Test message");

            string fileContent = File.ReadAllText(_testLogFilePath);
            Assert.That(fileContent, Does.Match(@"\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2} INFO: Test message"));
        }
    }
}