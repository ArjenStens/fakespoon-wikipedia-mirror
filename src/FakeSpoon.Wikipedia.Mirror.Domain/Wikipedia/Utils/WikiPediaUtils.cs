namespace FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Utils;

public static class WikiPediaUtils
{
    public static string UrlFromTitle(string title)
    {
        return $"https://en.wikipedia.org/wiki/{title.Replace(" ", "_")}";
    }
}