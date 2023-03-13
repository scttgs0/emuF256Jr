
using System.Diagnostics;
using System.Reflection;


namespace FoenixCore
{
    class AboutWindowProcess
    {
        public static string AppVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }
    }
}
