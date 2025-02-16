using Serilog;

namespace SharedLibrary.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }

        public static void LogToDebugger(string message) => Log.Debug(message);

        public static void LogToConsole(string message) => Log.Warning(message);
    }
}
