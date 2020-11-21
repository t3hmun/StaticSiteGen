namespace t3hmun.StaticSiteGenerator
{
    using System;
    using System.IO;
    using t3hmun.StaticSiteGenerator.Metadata;

    public class Post
    {
        private Post(string title, in DateTime timestamp, string? description, string html)
        {
            Title = title;
            Timestamp = timestamp;
            Description = description;
            Html = html;
        }

        public string Title { get; }
        public DateTime Timestamp { get; }
        public string? Description { get; }
        public string Html { get; }

        public static Post LoadPost(string filepath, MarkdownParser? markdownParser = null)
        {
            markdownParser ??= new MarkdownParser();

            FileInfo info = new(filepath);
            string filename = info.Name;
            string wholeFile = File.ReadAllText(filename);

            FileNameParser.Metadata filenameMetadata = FileNameParser.Parse(filename);
            int finalFrontMatterChar = JsonFrontMatterParser.IndexOfFrontMatterFinalChar(wholeFile);
            string jsonFrontMatter = wholeFile.Substring(0, finalFrontMatterChar + 1);
            JsonFrontMatterParser.Metadata frontMatterMetadata = JsonFrontMatterParser.Parse(jsonFrontMatter);
            string markdown = wholeFile.Substring(finalFrontMatterChar + 1);
            ContentMetadataParser.Metadata contentMetadata = ContentMetadataParser.Parse(markdown);

            string title = frontMatterMetadata.Title ?? contentMetadata.Title ?? filenameMetadata.Title;
            string? description = frontMatterMetadata.Description ?? contentMetadata.Description;
            DateTime timestamp = frontMatterMetadata.Timestamp ?? filenameMetadata.Timestamp;

            // Insert H1 if it does not exist.
            string markdownWithTitle = contentMetadata.Title == null ? $"# {title}\n\n{markdown}" : markdown;

            string html = markdownParser.ParseToHtml(markdownWithTitle);

            return new Post(title, timestamp, description, html);
        }
    }
}