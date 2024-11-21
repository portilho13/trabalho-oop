namespace trabalho_oop.Tests;
public class TestLogger : ILogger
{
    public List<string> LoggedMessages { get; } = new List<string>();

    public void Info(string message)
    {
        LoggedMessages.Add($"INFO: {message}");
    }

    public void Warn(string message)
    {
        LoggedMessages.Add($"WARN: {message}");
    }

    public void Error(string message)
    {
        LoggedMessages.Add($"ERROR: {message}");
    }
}

