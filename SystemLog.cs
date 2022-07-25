using System.Diagnostics;


namespace FoenixCore
{
    public static class SystemLog
    {
        public enum SeverityCodes
        {
            Fatal = 0,
            Recoverable = 1,
            Minor = 2
        }

        public static void WriteLine(SeverityCodes Severity, string Message)
        {
            Debug.WriteLine("LOG: " + Message);
        }
    }
}
