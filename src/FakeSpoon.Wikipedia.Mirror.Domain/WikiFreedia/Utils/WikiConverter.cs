using System.Text.RegularExpressions;
using Pandoc;

namespace FakeSpoon.Wikipedia.Mirror.Domain.WikiFreedia.Utils;

public static class WikiConverter
{

    public static async Task<string> ToWikiArticle(string wikiText)
    {
        var pandoc = new PandocEngine("/opt/homebrew/bin/pandoc"); // TODO: include in project
        var markdown = await pandoc.ConvertToText<MediaWikiIn, PandocMdOut>(wikiText);


        return markdown;
    }
    
    public static async Task<string> RenderWikiTemplates(string wikiTextInMarkdown)
    {
        // Force remove some of the templates already
        wikiTextInMarkdown = wikiTextInMarkdown.Replace("{=mediawiki}", "");
        wikiTextInMarkdown = wikiTextInMarkdown.Replace("{{reflist}}", "");
        
        //
        var templates = await GetAllTemplates(wikiTextInMarkdown);
        
        foreach (var result in templates)
        {

            if (result.Template.GetType() == typeof(CiteWebTemplate))
            {
                var template = (CiteWebTemplate)result.Template;
                wikiTextInMarkdown = wikiTextInMarkdown.Replace(result.Raw, template.ToString());
                continue;
            }
        }
        

        return wikiTextInMarkdown;
    }

    public static async Task<IEnumerable<WikiTemplateResult>> GetAllTemplates(string wikiTextInMarkdown)
    {
        string pattern = "{{(?<template_value>).*}}";

        // Create dictionary to store key-value pairs
        var templates = new List<WikiTemplateResult>();

        // Match key-value pairs using regex
        MatchCollection matches = Regex.Matches(wikiTextInMarkdown, pattern);

        // Populate dictionary with matched key-value pairs
        foreach (Match match in matches)
        {
            var result = new WikiTemplateResult()
            {
                StartPosition = match.Index,
                Raw = match.Value,
                Template = await ParseTemplate(match.Value)
            };
            templates.Add(result);
        }

        return templates;
    }

    public static async Task<IWikiTemplate> ParseTemplate(string template)
    {
        string pattern = @"\|\s*(\w+)\s*=\s*(.*?)\s*(?=\||}})";
        

        var kv = new Dictionary<string, string>();
        
        MatchCollection matches = Regex.Matches(template, pattern);
        foreach (Match match in matches)
        {
            string key = match.Groups[1].Value.Trim();
            string value = match.Groups[2].Value.Trim();
            kv[key] = value;
        }

        if (template.ToLower().StartsWith("{{cite web"))
        {
            return new CiteWebTemplate()
            {
                Title = kv.GetValueOrDefault("title"),
                Url = kv.GetValueOrDefault("url"),
                Date = kv.GetValueOrDefault("date"),
                AccessDate = kv.GetValueOrDefault("access-date"),
                Website = kv.GetValueOrDefault("website"),
                Language = kv.GetValueOrDefault("language")
            };
        }
        
        return new UnknownTemplate();
    }
    
    public class WikiTemplateResult
    {
        public int StartPosition { get; init; }
        public string Raw { get; init; }
        public IWikiTemplate Template { get; init; } = null;
    }
    
    public interface IWikiTemplate
    {
        
    }
    
    public class CiteWebTemplate : IWikiTemplate
    {
        public string? Date { get; init; }
        public string? Title { get; init; }
        public string? Url { get; init; }
        public string? AccessDate { get; init; }
        public string? Website { get; init; }
        public string? Language { get; init; }

        public override string ToString()
        {
            return $"[{Title}, {Date}, {Url}, {Website}, {AccessDate}, {Language}]";
        }
    }
    
    public class UnknownTemplate : IWikiTemplate{ }

    // public static IEnumerable<XmlExlement> GetXmlElements(string[] elementNames, string text)
    // {
    //     // Xml element names
    //     var elementNamesCapturingGroup = $"({string.Join("|", elementNames)})";
    //     
    //     var nonEmptyXmlPattern = $@"\\<{elementNamesCapturingGroup}(?<attributes>[^>]*)\>(?<content>.*?)\\<\/{elementNamesCapturingGroup}\\>";
    //     var selfClosingXmlPattern = $@"\\<{elementNamesCapturingGroup}(?<attributes>[^>]*)\>";
    //     
    //     var xmlPattern = $"{nonEmptyXmlPattern}|{selfClosingXmlPattern}";
    //     
    //     Regex regex = new Regex(xmlPattern, RegexOptions.Multiline);
    //     MatchCollection matches = regex.Matches(text);
    //
    //     return matches.Select(match => new XmlExlement()
    //     {
    //         StartPosition = match.Index,
    //         Value = match.Value,
    //         Attributes = match.Groups["attributes"].Value.Trim(),
    //         Content = match.Groups["content"].Success ? Regex.Unescape(match.Groups["content"].Value) : null
    //     });
    // }
    //
    // public class XmlExlement
    // {
    //     public int StartPosition { get; init; }
    //     public string Value { get; init; }
    //     public string Attributes { get; init; }
    //     public string? Content { get; init; }
    // }
}