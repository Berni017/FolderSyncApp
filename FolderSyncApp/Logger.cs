namespace FolderSyncApp;

public class Logger
{
    private readonly string _logPath;

    public Logger(string logPath)
    { 
        _logPath = Path.GetFullPath(logPath);
            Directory.CreateDirectory(Path.GetDirectoryName(_logPath) ?? string.Empty);
            File.AppendAllText(_logPath, string.Empty);
    }

    public void AppendLog(string log)
    {
        var logToPrint = $"{GetTimestamp()}: {log}";
        using(var file = File.AppendText(_logPath))
            file.WriteLine(logToPrint);
        Console.WriteLine(logToPrint);
    }

    private static string GetTimestamp()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}