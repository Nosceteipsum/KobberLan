using System.Collections.ObjectModel;
using Avalonia.Threading;
using KobberLan.Models;

namespace KobberLan.Utilities;

public class LogStore
{
    public ObservableCollection<LogEntry> Entries { get; } = new();

    public int MaxEntries { get; set; } = 500;

    public void Add(LogEntry entry)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Entries.Add(entry);
            while (Entries.Count > MaxEntries)
                Entries.RemoveAt(0);
        });
    }
}