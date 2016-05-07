namespace UnityPacker
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class UnityPacker
    {
        static string ProjectPath { get { return Regex.Replace(Application.dataPath, "/Assets", ""); } }

        static string PackagesPath { get { return Path.Combine(ProjectPath, "Packages"); } }

        [MenuItem("Tools/Packer/Export Packages %e")]
        static void Pack()
        {
            Packfile.GetFiles(ProjectPath).ForEach(Pack);
        }

        static void Pack(Packfile packfile)
        {
            var paths = packfile.GetPaths();
            if (paths.Count == 0)
            {
                var message = "Skip exporting {0}. Packfile {1} does not contain any files.";
                Debug.Log(string.Format(message, packfile.PackageName, packfile.FileName));
                return;
            }

            var package = Path.Combine(PackagesPath, Path.Combine(packfile.Name, packfile.PackageName));
            CreateDirectoryForFile(package);

            AssetDatabase.ExportPackage(paths.ToArray(), package, ExportPackageOptions.Recurse);
            
            Debug.Log("Exported package at " + ProjectPath + "/" + packfile.PackageName);
        }

        [MenuItem("Tools/Packer/Export Sources %#e")]
        static void Source()
        {
            Packfile.GetFiles(ProjectPath).ForEach(Source);
        }

        static void Source(Packfile packfile)
        {
            var sources = packfile.GetSources(ProjectPath);
            if (sources.Count == 0)
            {
                var message = "Skip exporting sources. Packfile {0} does not contain any files.";
                Debug.Log(string.Format(message, packfile.FileName));
                return;
            }

            sources.ForEach(x => CopySource(x, packfile.Name));
            
            Debug.Log("Exported source at " + ProjectPath + "/" + packfile.FileName);
        }

        static void CreateDirectoryForFile(string filePath)
        {
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        static void CopySource(string source, string package)
        {
            var copyRoot = Path.Combine(PackagesPath, Path.Combine(package, "Source"));
            var copy = Regex.Replace(source, "^" + Application.dataPath, copyRoot);

            CreateDirectoryForFile(copy);

            File.Copy(source, copy, true);
        }
    }
}