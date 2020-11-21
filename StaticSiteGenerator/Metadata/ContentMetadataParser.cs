namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System.Text.RegularExpressions;

    public static class ContentMetadataParser
    {
        /// <summary>Regex is easy.</summary>
        private static readonly Regex MetadataRegex =
            new(@"^\s*(?:(?:#(?!#) ?(?<hashH1>.*?))|((?<eqH1>.+?)\n=+?))\n(?<description>.*?\n)?(?:##.+|(?:.+?)\n-+)",
                RegexOptions.Singleline);

        public static Metadata Parse(string content)
        {
            Match? match = MetadataRegex.Match(content);
            Group? hashH1Group = match.Groups["hashH1"];
            Group? eqH1Group = match.Groups["eqH1"];
            Group? descGroup = match.Groups["description"];
            string? title = hashH1Group.Success ? hashH1Group.Value : eqH1Group.Success ? eqH1Group.Value : null;
            string? description = descGroup.Success ? descGroup.Value.Trim() : null;
            return new Metadata(title, description);
        }

        public record Metadata (string? Title, string? Description);
    }
}