using System.IO;
using System.Windows.Forms;

namespace VRCGPUTool.Util
{
    internal static class PathUtil
    {
        public static string LogDirectory { get; } = GetLogDirectory();

        private static string GetLogDirectory()
        {
            string tempPath = "D:\\Program Files (x86)\\TEMP";
            if (Directory.Exists(tempPath))
            {
                return Path.Combine(tempPath, "powerlog");
            }
            else
            {
                return Path.Combine(Application.StartupPath, "powerlog");
            }
        }
    }
}
