namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System;

    public record PostMetadata
    {
        public string Title { get; init; }
        public DateTime Timestamp { get; init; }
        public string? Description { get; init; }
    }
}