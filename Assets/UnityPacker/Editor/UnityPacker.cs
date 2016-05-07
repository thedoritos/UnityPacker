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

        [MenuItem("Tools/Packer/Export Packages %#e")]
        static void Pack()
        {
            var packfilePaths = Directory.GetFiles(Application.dataPath, "*.pack", SearchOption.AllDirectories);
            foreach (var path in packfilePaths)
            {
                Pack(path);
            }
        }

        static void Pack(string packfilePath)
        {
            var name = Path.GetFileNameWithoutExtension(packfilePath) + ".unitypackage";

            var paths = File.ReadAllText(packfilePath)
                .Split('\n')

                // Remove comments
                .Select(x => Regex.Replace(x, @"#.*$", ""))

                // Remove whtie spaces
                .Select(x => x.Trim())

                // Remove empty lines
                .Where(x => !string.IsNullOrEmpty(x.Trim()))

                // To relative path
                .Select(x => Regex.Replace(x, "^" + ProjectPath, ""));

            if (paths.Count() == 0)
            {
                Debug.Log("Skip exporting package " + name + " does not contain any files.");
                return;
            }

            AssetDatabase.ExportPackage(paths.ToArray(), name, ExportPackageOptions.Recurse);

            Debug.Log("Exported package at " + ProjectPath + "/" + name);
        }
    }
}