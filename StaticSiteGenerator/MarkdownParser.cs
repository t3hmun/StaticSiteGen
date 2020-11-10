namespace t3hmun.StaticSiteGenerator
{
    using Markdig;

    public class MarkdownParser
    {
        private readonly MarkdownPipeline _pipeline;

        public MarkdownParser()
        {
            var builder = new MarkdownPipelineBuilder();
            _pipeline = builder
                .UseEmphasisExtras() // subscript, superscript, strike through
                .UseDefinitionLists() // <dl> <dt> <dd>
                .UseAutoIdentifiers() // heading anchors
                .UseAutoLinks() // lazy hyperlinks
                .UseListExtras() // a. and i.
                .UseFigures() // neatly caption figures
                //.UseDiagrams() // Don't use this, use custom containers instead. Standard ``` code block is for text.
                .UseCustomContainers() // Create <div class="nomnoml"> blocks and similar.
                .UseGenericAttributes()
                .Build();
        }

        public string ParseToHtml(string markdown)
        {
            return Markdown.ToHtml(markdown, _pipeline);
        }
    }
}