using System;
using System.IO;
using System.Linq;
using System.Text;
using t3hmun.StaticSiteGenerator;

Console.WriteLine("Hello");
if (args.Length != 2) Console.WriteLine("Need 2 arguments, space separated, input dir, output dir.");
string inputDirPath = args[0];
string outputDirPath = args[1];

Console.WriteLine($"Input: {inputDirPath}");
Console.WriteLine($"Output: {outputDirPath}");

DirectoryInfo articlesDir = new(inputDirPath);
FileInfo[] articleFiles = articlesDir.GetFiles("*.md");
MarkdownParser parser = new();
Article[] articles = articleFiles.Select(file => Article.CreateFromFile(file.FullName, parser)).ToArray();

foreach (Article article in articles)
{
    string outputPath = Path.Combine(outputDirPath, article.ShortUrl + ".html");
    File.WriteAllText(outputPath, article.Html, Encoding.UTF8);
}