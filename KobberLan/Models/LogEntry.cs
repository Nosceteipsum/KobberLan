using System;

namespace KobberLan.Models;

public record LogEntry(
    DateTime Time, 
    LogLevel Level, 
    string Message, 
    Exception? Exception = null);