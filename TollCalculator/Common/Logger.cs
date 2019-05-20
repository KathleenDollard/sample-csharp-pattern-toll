using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum Severity
    {
        Info,
        Error,
        VeryBigProblem
    }
    public class Logger
    {
        public static void Log(string message, Severity severity)
            => Console.WriteLine($"{severity.ToString()}: {message}");

        public static void LogInfo(string message)
          => Log(message, Severity.Info);
        public static void LogError(string message)
          => Log(message, Severity.Error);
        public static void LogVeryBigProblem(string message)
          => Log(message, Severity.VeryBigProblem);

    }
}
