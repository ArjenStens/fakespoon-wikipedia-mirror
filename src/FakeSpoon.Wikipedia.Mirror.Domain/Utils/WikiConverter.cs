using System.Text.RegularExpressions;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Normalize;
using Markdig.Renderers.Roundtrip;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Pandoc;

namespace FakeSpoon.Wikipedia.Mirror.Domain.Utils;

public static class WikiConverter
{

    public static async Task<string> ToWikiArticle(string wikiText)
    {
        if (wikiText.StartsWith("{{Short description|Ratio of how much light is reflected back from a body}}"))
        {
            Console.WriteLine(wikiText);
        }
        
        var pandoc = new PandocEngine("/opt/homebrew/bin/pandoc"); // TODO: include in project
        var markdownPlain = await pandoc.ConvertToText<MediaWikiIn, PandocMdOut>(wikiText, outOptions: new ()
        {
            Wrap = Wrap.Preserve,
        });

        var parsedMd = Markdown.Parse(markdownPlain);
        var codeBlocks = GetCodeBlocks(parsedMd);
        
        foreach (var block in codeBlocks)
        {
            // var lsw = new StringWriter();
            // new NormalizeRenderer(lsw).Write(block);
            // var text = lsw.ToString();
            if(block.GetType() == typeof(FencedCodeBlock) || block.GetType() == typeof(CodeBlock) )
            {
                var b = (CodeBlock)block;
                b.Parent.Remove(b);
                
            } else if(block.GetType() == typeof(CodeInline))
            {
                var inline = (CodeInline)block;
                inline.Remove();
            }
        }
            
        var sw = new StringWriter();
        new NormalizeRenderer(sw).Write(parsedMd);
        var outputMarkdown = sw.ToString();

        outputMarkdown = outputMarkdown.Replace("{=mediawiki}", "");
        
        return outputMarkdown;
    }

    private static List<MarkdownObject> GetCodeBlocks(ContainerBlock containerBlock)
    {
        var result = new List<MarkdownObject>();
        foreach (var block in containerBlock)
        {
            if (block.GetType() == typeof(FencedCodeBlock) || block.GetType() == typeof(CodeBlock) || block.GetType() == typeof(CodeInline))
            {
                result.Add(block);
            }
            else if (block.GetType() == typeof(LeafBlock) || block.GetType().IsSubclassOf(typeof(LeafBlock)))
            {
                var leafBlock = (LeafBlock)block;
                var inlineCodeBlocks = GetInlineCodeBlocks(leafBlock.Inline);
                result.AddRange(inlineCodeBlocks);
            }
            else if (block.GetType() == typeof(ContainerBlock) || block.GetType().IsSubclassOf(typeof(ContainerBlock)))
            {
                var innerCodeBlocks = GetCodeBlocks((ContainerBlock)block);
                result.AddRange(innerCodeBlocks);
            }
            else
            {
                Console.WriteLine(block.GetType());
            }
        }

        return result;
    }

    private static IEnumerable<MarkdownObject> GetInlineCodeBlocks(ContainerInline? leafBlockInline)
    {
        var result = new List<Inline>();

        if (leafBlockInline == null)
        {
            return result;
        }
        
        foreach (var inline in leafBlockInline)
        {
            if (inline.GetType() == typeof(CodeInline) || inline.GetType() == typeof(CodeInline))
            {
                result.Add(inline);
            }
        }

        return result;
    }

    public static async Task<string> RenderWikiTemplates(string wikiTextInMarkdown)
    {
        // Force remove some of the templates already
        wikiTextInMarkdown = wikiTextInMarkdown.Replace("{=mediawiki}", "");
        wikiTextInMarkdown = wikiTextInMarkdown.Replace("{{reflist}}", "");
        wikiTextInMarkdown = wikiTextInMarkdown.Replace("```\n\n```", "");
        
        //
        var templates = await GetAllTemplates(wikiTextInMarkdown);
        
        foreach (var result in templates)
        {
            wikiTextInMarkdown = wikiTextInMarkdown.Replace(result.Match, result.Raw);
            
            if (result.Template.GetType() == typeof(CiteWebTemplate))
            {
                var template = (CiteWebTemplate)result.Template;
                wikiTextInMarkdown = wikiTextInMarkdown.Replace(result.Raw, $" {template}");
                continue;
            }
        }
        
        return wikiTextInMarkdown;
    }

    public static async Task<IEnumerable<WikiTemplateResult>> GetAllTemplates(string wikiTextInMarkdown)
    {
        string pattern = "(\\n *```\\n *)(?<template_value>{{.*}})( *\\n *```)";

        // Create dictionary to store key-value pairs
        var templates = new List<WikiTemplateResult>();

        // Match key-value pairs using regex
        MatchCollection matches = Regex.Matches(wikiTextInMarkdown, pattern);

        // Populate dictionary with matched key-value pairs
        foreach (Match match in matches)
        {
            var value = match.Groups[3].Value;
            var result = new WikiTemplateResult
            {
                StartPosition = match.Index,
                Raw = value,
                Template = await ParseTemplate(value),
                Match = match.Value
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
        public string Match { get; init; } = null;
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
}