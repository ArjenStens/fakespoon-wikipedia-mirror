using Newtonsoft.Json;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;

public class WikiPage
{
    public int Id { get; set; }
    public int Namespace { get; set; }
    public Object Redirect { get; set; }
    public string Title { get; set; }
    
    public WikiRevision Revision { get; set; }
    
    
    public bool IsRedirect
    {
        get { return Redirect != null; }
    }
}

public class WikiRevision
{
    public WikiText Text { get; set; }
}
public class WikiText
{
    [JsonProperty("#text")]
    public string Value { get; set; }
    
    [JsonProperty("@xml:space")]
    public string Space { get; set; }
}