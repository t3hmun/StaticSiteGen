namespace t3hmun.StaticSiteGenerator.Tests
{
    using FluentAssertions;
    using t3hmun.StaticSiteGenerator.Metadata;
    using Xunit;

    public class FrontMatterParserTests
    {
        [Theory]
        [InlineData("a word", "", "valid text value no content")]
        [InlineData("a word", "\n#Markdown\n\nContent", "valid text value, text content")]
        [InlineData("{", "", "valid open brace value no content")]
        [InlineData("{", "\n#Markdown\n\nContent", "valid open brace value, text content")]
        [InlineData("}", "", "valid close brace value no content")]
        [InlineData("}", "\n#Markdown\n\nContent", "valid close brace value, text content")]
        [InlineData("{}", "", "valid open-close brace value no content")]
        [InlineData("{}", "\n#Markdown\n\nContent", "valid open-close brace value, text content")]
        [InlineData("}{", "", "valid close-open brace value no content")]
        [InlineData("}{", "\n#Markdown\n\nContent", "valid close-open brace value, text content")]
        [InlineData("\\\"", "\n#Markdown\n\nContent", "valid escaped quote value, text content")]
        [InlineData("\\\"\\\"", "\n#Markdown\n\nContent", "valid escaped quote pair value, text content")]
        [InlineData("\\\"{", "\n#Markdown\n\nContent", "valid escaped quote open brace value, text content")]
        public void ShouldFindOneLineSinglePropJson(string value, string content, string because)
        {
            string expected = "{\"key\":\"" + value + "\"}";
            string markdown = expected + content;
            int finalCharIndex = JsonFrontMatterParser.IndexOfFrontMatterFinalChar(markdown);
            string actual = markdown.Substring(0, finalCharIndex + 1);
            actual.Should().Be(expected, because);
        }
    }
}