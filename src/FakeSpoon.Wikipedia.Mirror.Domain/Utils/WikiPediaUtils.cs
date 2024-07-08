namespace FakeSpoon.Wikipedia.Mirror.Domain.Utils;

public static class WikiUtils
{
    public static string AsTopicName(string input) {
        return input.ToLower()
            .Trim()
            .Replace(",", "")
            .Replace("(", "")
            .Replace(")", "")
            .Replace(" ", "-");
    }
    
    public static string UrlFromTitle(string title)
    {
        return $"https://en.wikipedia.org/wiki/{title.Replace(" ", "_")}";
    }
}