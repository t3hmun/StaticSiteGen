namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class FileNameParser
    {
        private static readonly Regex FileNameRegex =
            new(@"^(\d\d\d\d-\d\d-\d\d)(-\d\d)?-(.*).md$", RegexOptions.Compiled);

        public static Metadata Parse(string fileName)
        {
            Match? match = FileNameRegex.Match(fileName);
            if (!match.Success)
                throw new MetadataParseException(
                    $"Filename did not match required format. Filename: {fileName}, Regex: {FileNameRegex}");

            string date = match.Groups[1].Value;
            string time = match.Groups[2].Value;
            string title = match.Groups[3].Value;

            string dateTimeFormat = "yyyy-MM-dd";
            DateTime timestamp = DateTime.ParseExact(date, dateTimeFormat, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(time)) timestamp = timestamp.AddHours(int.Parse(time));
            return new Metadata(fileName, timestamp, title);
        }

        public record Metadata (string FileName, DateTime Timestamp, string Title);
    }
}