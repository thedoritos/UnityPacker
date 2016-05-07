namespace UnityPacker
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Packfile
    {
        public string FileName { get; private set; }

        public string Content { get; private set; }

        public string Name { get; private set; }

        public string PackageName { get { return Name + ".unitypackage"; } }

        public Packfile(string filePath)
        {
            Debug.Log(filePath);

            FileName = Path.GetFileName(filePath);
            Content = File.ReadAllText(filePath);
            Name = Path.GetFileNameWithoutExtension(filePath);
        }

        public static List<Packfile> GetFiles(string root)
        {
            return Directory.GetFiles(root, "*.pack", SearchOption.AllDirectories)

                // Remove git repositoy
                .Where(x => !x.Contains("/.git/"))

                // Remove Packages directory
                .Where(x => !x.StartsWith(Path.Combine(root, "Packages")))
                
                // Create instance
                .Select(x => new Packfile(x))
                
                .ToList();
        }

        public List<string> GetPaths()
        {
            return Content.Split('\n')

                // Remove comments
                .Select(x => Regex.Replace(x, @"#.*$", ""))

                // Remove whtie spaces
                .Select(x => x.Trim())

                // Remove empty lines
                .Where(x => !string.IsNullOrEmpty(x.Trim()))

                .ToList();
        }

        public List<string> GetSources(string root)
        {
            return GetPaths()
                
                // To absolute paths
                .Select(x => Path.Combine(root, x))
                
                // To files
                .SelectMany(x =>
                {
                    if (File.Exists(x))
                    {
                        return new string[] { x };
                    }

                    if (Directory.Exists(x))
                    {
                        return Directory.GetFiles(x, "*", SearchOption.AllDirectories);
                    }
                     
                    return new string[] { };
                })
                
                // Distinct
                .GroupBy(x => x)
                .Select(x => x.First())
                
                // Remove metas
                .Where(x => !x.EndsWith(".meta"))
                
                .ToList();
        }
    }
}