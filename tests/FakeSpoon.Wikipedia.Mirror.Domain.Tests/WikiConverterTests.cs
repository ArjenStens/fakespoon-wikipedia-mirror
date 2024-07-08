using FakeSpoon.Wikipedia.Mirror.Domain.Utils;
using FluentAssertions;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Tests;

public class WikiConverterTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [TestCase("TestFiles/wikitext1.txt", "TestFiles/wikitext1_expectedMd.txt")]
    public async Task ToWikiArticle_Should_ProduceValidArticle(string wikiTextInputFilePath, string expectedFilePath)
    {
        var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        
        // arrange
        var wikiText = await ReadTextFile(Path.Combine(directory, wikiTextInputFilePath));
        var expected = await ReadTextFile(Path.Combine(directory, expectedFilePath));
        
        // act
        var actual = await WikiConverter.ToWikiArticle(wikiText);
        
        // assert
        actual.Should().Be(expected);
    }
    
    [TestCase("TestFiles/textWithBrokenFootnotes.md", "TestFiles/textWithCorrectFootnotes.md")]
    public async Task RenderWikiTemplates_Should_ProduceValidArticle(string wikiTextInputFilePath, string expectedFilePath)
    {
        var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        
        // arrange
        var wikiTextInMarkdown = await ReadTextFile(Path.Combine(directory, wikiTextInputFilePath));
        var expected = await ReadTextFile(Path.Combine(directory, expectedFilePath));
        
        // act
        var actual = await WikiConverter.RenderWikiTemplates(wikiTextInMarkdown);
        
        // assert
        actual.Should().Be(expected);
    }
    //
    // [Test]
    // public async Task GetXmlElements_Should_ProvideListOfAllElements()
    // {
    //     var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    //     
    //     // arrange
    //     var text = await ReadTextFile(Path.Combine(directory, "TestFiles/textWithXmlTags1.txt"));
    //     
    //     // act
    //     var actual = WikiConverter.GetXmlElements(new []{"ref"}, text);
    //     
    //     var x = actual.Reverse();
    //     foreach (var element in x)
    //     {
    //         text = text.Replace(element.Value, "[REMOVED]");
    //     }
    //     
    //     // assert
    //     // actual.Should();
    // }

    public async Task<string> ReadTextFile(string fullPath)
    {
        try
        {
            using StreamReader reader = new(fullPath);
            return await reader.ReadToEndAsync();
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
            Assert.Fail("Error loading test input files, are you including it in the build?");
        }
        
        return "";
    }
    
    [OneTimeTearDown]
    public static void SomeTearDown()
    {
        
    }
}