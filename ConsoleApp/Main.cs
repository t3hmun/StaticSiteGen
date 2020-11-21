using System;
using System.IO;
using System.Linq;
using t3hmun.StaticSiteGenerator;

if (args.Length != 2)
{
    Console.WriteLine("Need 2 arguments, space separated, input dir, output dir.");
}

string postsFolderPath = args[0];
string siteFolderPath = args[0];
    
DirectoryInfo postsFolder = new DirectoryInfo(postsFolderPath);
FileInfo[] postFiles = postsFolder.GetFiles("*.md");    
MarkdownParser parser = new MarkdownParser();

Post[] posts = postFiles.Select(file=> Post.LoadPost(file.FullName, parser)).ToArray();
foreach (Post post in posts)
{
    string outputPath = Path.Combine(siteFolderPath, post.Filename)
}
