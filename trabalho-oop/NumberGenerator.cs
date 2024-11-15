namespace trabalho_oop;

public static class NumberGenerator
{
    public static string GenerateRandomNumber()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var randomId = new char[6];

        for (int i = 0; i < randomId.Length; i++)
        {
            randomId[i] = chars[random.Next(chars.Length)];
        }

        return new string(randomId);
    }
}