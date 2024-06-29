namespace FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;

public class Page
{
    public int Id { get; set; }
    public int Namespace { get; set; }
    public KeyValuePair<string, string> Redirect { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    
    // public bool IsRedirect
    // {
    //     get { return Redirect != null; }
    // }
}