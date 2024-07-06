# FakeSpoon's Nostr - Wikipedia Mirror

Publishes Legacy Wikipedia articles as WikiArticle Events on Nostr.

## The goal
The goal of this application is to copy ALL Wikipedia articles to Nostr relays as WikiArticles and to then keep them up-to-date with all changes made to the articles on [wikipedia.org](https://wikipedia.org).

This will provide a knowledge baseline for Nostr upon which everyone can extend and improve as they please.

## Requests for help
Any help is greatly appreciated, particularly on setting up the process of picking apart the compressed wikipedia dumps and converting them from the `wikitext` format into proper `markdown` format. Wikipedia uses several extensions, which make it hard to convert it.

## Conversion of wiki pages to markdown.
Currently using [pandoc](https://pandoc.org/) CLI tool and [PandocNet package](https://github.com/SimonCropp/PandocNet) to do the first round of conversion into `markdown` format. More processing is needed to interpret the wikipedia templates `{{ ...data... }}` format.

### Wikipedia extensions
Full list of extension tags: https://www.mediawiki.org/wiki/Parser_extension_tags

**Currently supported exstension tags;**
- `<ref>` tags


