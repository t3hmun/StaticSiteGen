namespace t3hmun.StaticSiteGenerator.Metadata
{
    using System;
    using System.Text.Json;

    /// <summary>Extracts metadata from the Json front matter.</summary>
    public static class JsonFrontMatterParser
    {
        public static Metadata Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new Metadata(null, null, null, null);
            var options = new JsonSerializerOptions {AllowTrailingCommas = true, PropertyNameCaseInsensitive = true};
            var frontMatter = JsonSerializer.Deserialize<Metadata>(json, options);
            if (frontMatter == null)
                throw new MetadataParseException(
                    $"Front matter json deserialize returned null, {nameof(json)}: `{json}`");
            return frontMatter;
        }

        /// <summary>
        ///     Extracts the json
        /// </summary>
        /// <param name="markdown">The markdown file content.</param>
        /// <returns>The index of the final character of the json front matter, -1 if there is none.</returns>
        /// <exception cref="MetadataParseException">It thinks it found json but then ran into trouble.</exception>
        public static int IndexOfFrontMatterFinalChar(string markdown)
        {
            if (!markdown.TrimStart(' ', '\n').StartsWith("{")) return -1;
            int first = markdown.IndexOf('{');
            int openBraces = 0;
            int finalBraceIndex = 0;
            bool escaped = false;
            bool withinQuotes = false;
            for (int i = first; i < markdown.Length; i++)
            {
                char current = markdown[i];

                if (!escaped && current == '"') withinQuotes = !withinQuotes;

                if (!withinQuotes)
                {
                    if (current == '{') openBraces++;
                    else if (current == '}') openBraces--;
                }

                if (openBraces == 0)
                {
                    finalBraceIndex = i;
                    break;
                }

                if (current == '\\' && !escaped) escaped = true;
                else escaped = false;
            }

            if (finalBraceIndex == 0)
                throw new MetadataParseException(
                    $"Tried to extract invalid JsonBlock, ended with {openBraces} open braces.");
            return finalBraceIndex;
        }

        public record Metadata (string? Description, string? Title, DateTime? Timestamp, string? ShortUrl);
    }
}