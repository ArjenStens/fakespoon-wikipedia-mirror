namespace FakeSpoon.Wikipedia.Mirror.Domain.WikiFreedia.Utils;

public static class WikiFreediaUtils
{
    public static string AsTopicName(string input) {
        return input.ToLower()
            .Trim()
            .Replace(",", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace(" ", "-");
    }
}