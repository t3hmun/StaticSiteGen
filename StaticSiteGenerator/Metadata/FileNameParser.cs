namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System;
    using System.Globalization;

    public static class FileNameParser
    {
        public static FileNameMetadata Parse(string fileName)
        {
            if (fileName.Length < 15)
                throw new MetadataParseException(
                    $"{nameof(fileName)}: `{fileName}` is too short to match yyyy-mm-dd-title.md format.");
            if (!fileName.EndsWith(".md"))
                throw new MetadataParseException($"{nameof(fileName)}: `{fileName}` does not end with .md as expected");

            string dateText = fileName.Substring(0, 10);
            string titleText = fileName.Substring(11, fileName.Length - 14);
            DateTime timestamp = DateTime.ParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return new FileNameMetadata(fileName, timestamp, titleText);
        }

        public interface IFileMetadata
        {
            string FileName { get; }
            DateTime Timestamp { get; }
            string FileNameTitle { get; }
        }

        public sealed record FileNameMetadata
            (string FileName, DateTime Timestamp, string FileNameTitle) : IFileMetadata;
    }
}