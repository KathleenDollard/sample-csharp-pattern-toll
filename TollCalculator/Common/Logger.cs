using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{

    public enum Severity
    {
        Info,
        Error,
        Exception,
        VeryBigProblem
    }
    public class Logger
    {
        public static void Log(string message, Severity severity)
            => Console.WriteLine($"{severity}: {message}");

        public static void LogInfo(string message)
            => Log(message, Severity.Info);

        public static void LogError(string message)
            => Log(message, Severity.Error);


        public static void LogException(Exception e)
            => Log(e.Message, Severity.Exception);


        public static void LogVeryBigProblem(string message)
            => Log(message, Severity.VeryBigProblem);
    }
}
