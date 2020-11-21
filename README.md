# StaticSiteGen

* Generate html from markdown using many markdig extensions
* Read custom json preamble
* Auto generate or parse heading and description metadata

As always the main point of this is to play around with the latest C# features, this time .Net5 and C#9.

This time I've avoided ASP/MVC/Razor because of the horribly slow cold starts on Azure. 
It is obviously the wrong tech for hosting a *static* site but the point was learning, not the site.

The templating, CSS, scripts and other pages will be separate from this generator.


## Metadata

Basic metadata for every article:

* Title
  * Used for index page
  * Maybe be inserted into page if H1 is missing from markdown
  * Source: JsonFrontmatter then H1 then aticle filename
* Timestamp
  * Used for index page and sorting
  * Time component defaults to 0, only hour may be specified
  * Source: JsonFrontmatter then aticle filename
* Description
  * Used for index page
  * Source: JsonFrontMatter then the text between the H1 and H2 then blank
* ShortUrl
  * Used as the page name - the staticly hosted html file
  * Source: JsonFrontMatter then the Title with the non-url friendly character removed


### Article Filename

Must be of the format `yyyy-MM-dd-Title.md` or `yyyy-MM-dd-hh-Title.md` if there is more than one article in a day. 
The fallback title and timestamp are extracted from it.


### JsonFrontMatter

This is a oneline Json block at the start of the file, can define any of the metadata properties.
Any metadata define here supersedes other sources of metadata.

I like json because it does not confuse simple markdown parsers / previews, just shows a line of raw json text.
In comparison YAML can totally confuse markdown parsers, makes a mess.


### Metadata extracted from content

The H1 in the markdown is interpreted as a more correct title than the filename.
However it can be omitted, in which case it is automatically inserted using Title metadata.
Originally this was only the filename, but valid file names are limiting and it is nicer to write an article including the H1.

The text between the H1 (article title) and H2 will automatically be taken as the description unless it is already defined in frontmatter.
This is a nice option to allow some formatting in the description.
