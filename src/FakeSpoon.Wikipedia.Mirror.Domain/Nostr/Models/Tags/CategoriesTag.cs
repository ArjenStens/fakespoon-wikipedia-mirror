using FakeSpoon.Wikipedia.Mirror.Infrastructure.Nostr.Models.Tags;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Nostr.Models.Tags;

public class CategoriesTag : INostrTag
{
    public CategoriesTag(IEnumerable<string> categories)
    {
        Categories = categories;
    }

    public string Name { get; } = "c";

    private IEnumerable<string> Categories { get; set; }

    public string[] Rendered => new List<string> { Name }.Concat(Categories).ToArray();
}