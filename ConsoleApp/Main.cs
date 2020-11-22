using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using t3hmun.StaticSiteGenerator;

if (args.Length != 1)
{
    throw new Exception("SSG FAILED: Needs one argument, path of article markdown files.");
}

string inputDirPath = args[0];
DirectoryInfo articlesDir = new(inputDirPath);
FileInfo[] articleFiles = articlesDir.GetFiles("*.md");
MarkdownParser parser = new();
Article[] articles = articleFiles.Select(file => Article.CreateFromFile(file.FullName, parser)).ToArray();
string articlesJson = JsonSerializer.Serialize(articles);
Console.Write(articlesJson);
return 0;