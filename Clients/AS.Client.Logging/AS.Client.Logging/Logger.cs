using System;

namespace AS.Client.Logging
{
    public enum LogLevel
    {
        Debug,
        Warning,
        Error
    }

    public static class Logger
    {
        public static void LogDebug(string text, params string[] parameters)
        {
            if (LogDebugMethod == null)
                return;
            if (parameters.Length > 0)
                text = String.Format(text, parameters);
            LogDebugMethod(text);
        }
        public static void LogWarning(string text, params string[] parameters)
        {
            if (LogWarningMethod == null)
                return;
            if (parameters.Length > 0)
                text = String.Format(text, parameters);
            LogWarningMethod(text);
        }
        public static void LogError(string text, params string[] parameters)
        {
            if (LogErrorMethod == null)
                return;
            if (parameters.Length > 0)
                text = String.Format(text, parameters);
            LogErrorMethod(text);
        }

        public static void SetLogger(LogLevel logLevel, Action<string> logMethod)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    LogDebugMethod = logMethod;
                    break;
                case LogLevel.Warning:
                    LogWarningMethod = logMethod;
                    break;
                case LogLevel.Error:
                    LogErrorMethod = logMethod;
                    break;
            }
        }

        private static Action<string> LogDebugMethod { get; set; }
        private static Action<string> LogWarningMethod { get; set; }
        private static Action<string> LogErrorMethod { get; set; }
    }
}
