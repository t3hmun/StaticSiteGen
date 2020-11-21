namespace t3hmun.StaticSiteGenerator.Tests
{
    using System;
    using t3hmun.StaticSiteGenerator.Metadata;
    using Xunit;

    public class FileNameParserTests
    {
        [Theory]
        [InlineData("2016-09-26-Test Post.md", 2016, 09, 26, "Test Post")]
        [InlineData("2016-09-26-X.md", 2016, 09, 26, "X")]
        public void FileNameShouldParse(string fileName, int year, int month, int day, string title)
        {
            var expected = new FileNameParser.FileNameMetadata(fileName, new DateTime(year, month, day), title);
            FileNameParser.FileNameMetadata actual = FileNameParser.Parse(fileName);
            Assert.Equal(expected, actual);
        }
    }
}