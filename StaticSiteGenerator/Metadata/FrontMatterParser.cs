namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System.Text.Json;

    public static class FrontMatterParser
    {
        public static IFrontMatter ParseJsonFrontMatter(string json)
        {
            var options = new JsonSerializerOptions {AllowTrailingCommas = true, PropertyNameCaseInsensitive = true};
            var frontMatter = JsonSerializer.Deserialize<JsonFrontMatter>(json, options);
            if (frontMatter == null)
                throw new MetadataParseException(
                    $"Front matter json deserialize returned null, {nameof(json)}: `{json}`");
            return frontMatter;
        }

        public static string? GetJsonFrontMatterOrNull(string markdown)
        {
            if (!markdown.TrimStart(' ', '\n').StartsWith("{")) return null;
            int first = markdown.IndexOf('{');
            int braces = 0;
            int final = 0;
            bool escaped = false;
            bool inQuotes = false;
            for (int i = first; i < markdown.Length; i++)
            {
                char current = markdown[i];

                if (!escaped && current == '"') inQuotes = !inQuotes;

                if (!inQuotes)
                {
                    if (current == '{') braces++;
                    else if (current == '}') braces--;
                }

                if (braces == 0)
                {
                    final = i;
                    break;
                }

                if (current == '\\' && !escaped) escaped = true;
                else escaped = false;
            }

            // If the json never terminated then it isn't json.
            if (final == 0) return null;
            string json = markdown.Substring(first, final - first);
            return json;
        }

        public interface IFrontMatter
        {
            string? FrontMatterDescription { get; }
            string? FrontMatterTitle { get; }
            string? FrontMatterTime { get; }
        }

        private record JsonFrontMatter (string? FrontMatterDescription, string? FrontMatterTitle,
            string? FrontMatterTime) : IFrontMatter;
    }
}