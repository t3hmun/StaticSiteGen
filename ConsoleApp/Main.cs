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

DirectoryInfo postsFolder = new(inputDirPath);
FileInfo[] postFiles = postsFolder.GetFiles("*.md");
MarkdownParser parser = new();
Post[] posts = postFiles.Select(file => Post.LoadPost(file.FullName, parser)).ToArray();

foreach (Post post in posts)
{
    string outputPath = Path.Combine(outputDirPath, post.ShortUrl + ".html");
    File.WriteAllText(outputPath, post.Html, Encoding.UTF8);
}