namespace t3hmun.StaticSiteGenerator.Tests
{
    using FluentAssertions;
    using t3hmun.StaticSiteGenerator.Metadata;
    using Xunit;

    public class ContentMetadataParserTests
    {
        private const string HeaderText = "This is a Title";
        private const string HashHeader = "# " + HeaderText;
        private const string EqHeader = HeaderText + "\n====";
        private const string ContentHashH2 = "\n## Header for rest of content\nSome content\nstuff.";
        private const string ContentDashH2 = "\nHeader for rest of content\n----\nSome content\nstuff.";


        [Theory]
        [InlineData(HashHeader + ContentHashH2, "valid no space after #")]
        [InlineData("#" + HeaderText + ContentHashH2, "valid space after #")]
        [InlineData("\n" + HashHeader + ContentHashH2, "valid preceded by newline")]
        [InlineData("\n\n\n" + HashHeader + ContentHashH2, "valid preceded by many newlines")]
        public void FindsHashH1WithHashH2(string content, string because)
        {
            ContentMetadataParser.ContentMetadata result = ContentMetadataParser.ParseContentMetadata(content);
            var expected = new ContentMetadataParser.ContentMetadata(HeaderText, null);
            result.Should().Be(expected, because);
        }

        [Theory]
        [InlineData(EqHeader + ContentHashH2, "valid")]
        [InlineData("\n" + EqHeader + ContentHashH2, "valid preceded by newline")]
        [InlineData("\n\n\n" + EqHeader + ContentHashH2, "valid preceded by many newlines")]
        public void FindsEqH1WithHashH2(string content, string because)
        {
            ContentMetadataParser.ContentMetadata result = ContentMetadataParser.ParseContentMetadata(content);
            var expected = new ContentMetadataParser.ContentMetadata(HeaderText, null);
            result.Should().Be(expected, because);
        }

        [Theory]
        [InlineData(EqHeader + ContentDashH2, "valid")]
        [InlineData("\n" + EqHeader + ContentDashH2, "valid preceded by newline")]
        [InlineData("\n\n\n" + EqHeader + ContentDashH2, "valid preceded by many newlines")]
        public void FindsEqH1WithDashH2(string content, string because)
        {
            ContentMetadataParser.ContentMetadata result = ContentMetadataParser.ParseContentMetadata(content);
            var expected = new ContentMetadataParser.ContentMetadata(HeaderText, null);
            result.Should().Be(expected, because);
        }

        [Theory]
        [InlineData(HashHeader + ContentDashH2, "valid no space after #")]
        [InlineData("#" + HeaderText + ContentDashH2, "valid space after #")]
        [InlineData("\n" + HashHeader + ContentDashH2, "valid preceded by newline")]
        [InlineData("\n\n\n" + HashHeader + ContentDashH2, "valid preceded by many newlines")]
        public void FindsHashH1WithDashH2(string content, string because)
        {
            ContentMetadataParser.ContentMetadata result = ContentMetadataParser.ParseContentMetadata(content);
            var expected = new ContentMetadataParser.ContentMetadata(HeaderText, null);
            result.Should().Be(expected, because);
        }

        [Theory]
        [InlineData("# The Title\n## Title before context\n rest of content", null, "no description")]
        [InlineData("# The Title\ndesc here\n## Title before context\n rest of content", "desc here",
            "immediate single line description")]
        [InlineData("# The Title\ndesc here\nlalala\n## Title before context\n rest of content", "desc here\nlalala",
            "immediate multiline description")]
        public void FindsHasH1AndDescriptionHashH2(string content, string description, string because)
        {
            ContentMetadataParser.ContentMetadata result = ContentMetadataParser.ParseContentMetadata(content);
            var expected = new ContentMetadataParser.ContentMetadata("The Title", description);
            result.Should().Be(expected, because);
        }
    }
}