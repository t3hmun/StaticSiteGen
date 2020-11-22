namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System;

    public record ArticleMetadata
    {
        /// <summary>
        ///     The title of the article. This should be the same in the index page, H1 and head title of the article unless both
        ///     the H1 and the front matter set a value.
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        ///     When the article was written or uploaded - it is up to the user to correct this.
        ///     This should only be set via the filename, can be set in the front matter but that could be confusing.
        /// </summary>
        public DateTime Timestamp { get; init; }

        /// <summary>RAW MARKDOWN Short description of the article for the index.</summary>
        public string? MarkdownDescription { get; init; }

        /// <summary>The name to use for the statically hosted file.</summary>
        public string ShortUrl { get; init; }
    }
}