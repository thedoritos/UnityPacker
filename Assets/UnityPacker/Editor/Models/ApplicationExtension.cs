namespace UnityPacker
{
    using UnityEngine;
    using System.Text.RegularExpressions;

    public static class ApplicationExtension
    {
        public static string GetProjectPath(this Application app)
        {
            return Regex.Replace(Application.dataPath, "/Assets", "");
        }
    }
}