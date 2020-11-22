namespace t3hmun.StaticSiteGenerator
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using t3hmun.StaticSiteGenerator.Metadata;

    public class Article
    {
        private static readonly Regex UrlCleanRegex = new(@"[^\w\.@-]", RegexOptions.Compiled);

        private Article(string title, in DateTime timestamp, string? description, string shortUrl, string html)
        {
            Title = title;
            Timestamp = timestamp;
            Description = description;
            ShortUrl = shortUrl;
            Html = html;
        }

        public string Title { get; }
        public DateTime Timestamp { get; }
        public string? Description { get; }
        public string ShortUrl { get; }
        public string Html { get; }

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
            string? description = frontMatterMetadata.Description ?? contentMetadata.Description;
            DateTime timestamp = frontMatterMetadata.Timestamp ?? filenameMetadata.Timestamp;

            string shortUrl = frontMatterMetadata.ShortUrl ?? UrlCleanRegex.Replace(title, "");

            // Insert H1 if it does not exist.
            string markdownWithTitle = contentMetadata.Title == null ? $"# {title}\n\n{markdown}" : markdown;

            string html = markdownParser.ParseToHtml(markdownWithTitle);

            return new Article(title, timestamp, description, shortUrl, html);
        }
    }
}