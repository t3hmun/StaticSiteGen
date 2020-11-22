namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System;

    public record ArticleMetadata
    {
        public string Title { get; init; }
        public DateTime Timestamp { get; init; }
        public string? Description { get; init; }
        public string ShortUrl { get; init; }
    }
}