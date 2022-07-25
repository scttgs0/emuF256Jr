using System.Diagnostics;


namespace FoenixCore
{
    class AboutWindowProcess
    {
        public static string AppVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fvi.FileVersion;
        }
    }
}
