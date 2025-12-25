using System;
using KobberLan.Models;

namespace KobberLan.Utilities;

public static class AppLog
{
    public static LogStore Store { get; } = new();

    public static void Info(string msg)  => Write(LogLevel.Info, msg);
    public static void Warn(string msg)  => Write(LogLevel.Warning, msg);
    public static void Error(string msg, Exception? ex = null) => Write(LogLevel.Error, msg, ex);

    private static void Write(LogLevel level, string msg, Exception? ex = null)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {level}: {msg}" + (ex != null ? $" | {ex}" : ""));
        Store.Add(new LogEntry(DateTime.Now, level, msg, ex));
    }
}