namespace t3hmun.StaticSiteGenerator
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using t3hmun.StaticSiteGenerator.Metadata;

    /// <summary>
    ///     The html output of the article metadata - only the html of the article, not responsible for the website layout
    ///     of the page.
    /// </summary>
    public class Article
    {
        private static readonly Regex UrlCleanRegex = new(@"[^\w\.@-]", RegexOptions.Compiled);

        private Article(string title, in DateTime timestamp, string? descriptionHtml, string shortUrl,
            string articleHtml)
        {
            Title = title;
            Timestamp = timestamp;
            DescriptionHtml = descriptionHtml;
            ShortUrl = shortUrl;
            ArticleHtml = articleHtml;
        }

        public string ArticleHtml { get; }

        public string Title { get; }
        public DateTime Timestamp { get; }
        public string? DescriptionHtml { get; }
        public string ShortUrl { get; }

        public static Article CreateFromFile(string filepath, MarkdownParser? markdownParser = null)
        {
            markdownParser ??= new MarkdownParser();

            FileInfo info = new(filepath);
            string filename = info.Name;
            string wholeFile = File.ReadAllText(filepath);

            FileNameParser.Metadata filenameMetadata = FileNameParser.Parse(filename);
            int finalFrontMatterChar = JsonFrontMatterParser.IndexOfFrontMatterFinalChar(wholeFile);
            string jsonFrontMatter = wholeFile.Substring(0, finalFrontMatterChar + 1);
            JsonFrontMatterParser.Metadata frontMatterMetadata = JsonFrontMatterParser.Parse(jsonFrontMatter);
            string markdown = wholeFile.Substring(finalFrontMatterChar + 1);
            ContentMetadataParser.Metadata contentMetadata = ContentMetadataParser.Parse(markdown);

            string title = frontMatterMetadata.Title ?? contentMetadata.Title ?? filenameMetadata.Title;
            string? mdDescription = frontMatterMetadata.MarkdownDescription ?? contentMetadata.MarkdownDescription;
            DateTime timestamp = frontMatterMetadata.Timestamp ?? filenameMetadata.Timestamp;

            string shortUrl = frontMatterMetadata.ShortUrl ?? UrlCleanRegex.Replace(title, "");

            // Insert H1 if it does not exist.
            string markdownWithTitle = contentMetadata.Title == null ? $"# {title}\n\n{markdown}" : markdown;

            string html = markdownParser.ParseToHtml(markdownWithTitle);
            string description = mdDescription == null ? "" : markdownParser.ParseToHtml(mdDescription);

            return new Article(title, timestamp, description, shortUrl, html);
        }
    }
}