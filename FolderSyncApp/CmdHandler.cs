namespace FolderSyncApp;

public class CmdHandler
{
    public static bool CheckHelp(string[] args)
    {
        if (args.Length != 1 || (args[0] != "-h" && args[0] != "--help")) return false;
        DisplayHelp();
        return true;
    }

    public static CmdArgs ParseArgs(string[] args)
    {
        if (args.Length != 4)
        {
            DisplayError("Invalid argument number");
            Environment.Exit(13);
        }
        var sourceFolderPath = Path.GetFullPath(args[0]);
        var replicaFolderPath = Path.GetFullPath(args[1]);
        var logFilePath = Path.GetFullPath(args[2]);
        
        if(!Directory.Exists(sourceFolderPath))
            throw new ArgumentException($"Source folder does not exist: {sourceFolderPath}");
        if (CheckIfSame(sourceFolderPath, replicaFolderPath))
            throw new ArgumentException($"Replica folder cannot be the same as Source folder:\nSource folder: {args[0]}\nReplica folder: {args[1]}");
        if (CheckIfSame(sourceFolderPath, logFilePath))
            throw new ArgumentException($"Log file cannot be the same path as Source folder:\nSource folder {args[0]}\nLog file path: {args[2]}");
        if (CheckIfSame(replicaFolderPath, logFilePath))
            throw new ArgumentException($"Log file cannot be the same path as Replica file:\nReplica folder {args[1]}\nLog file path: {args[2]}");
        if (CheckIfInside(sourceFolderPath, replicaFolderPath))
            throw new ArgumentException($"Source folder cannot be inside Replica Folder:\nSource folder: {args[0]}\nReplica folder: {args[1]}");
        if (CheckIfInside(replicaFolderPath, sourceFolderPath))
            throw new ArgumentException($"Replica folder cannot be inside Source Folder:\nSource folder: {args[0]}\nReplica folder: {args[1]}");
        if (CheckIfInside(logFilePath, sourceFolderPath))
            throw new ArgumentException($"Log file cannot be inside Source folder:\nSource folder: {args[0]}\nLog file path: {args[2]}");
        if (CheckIfInside(logFilePath, replicaFolderPath))
            throw new ArgumentException($"Log file cannot be inside Replica folder:\nReplica folder: {args[1]}\nLog file path: {args[2]}");
        if (!int.TryParse(args[3], out var interval) || interval <= 0)
            throw new  ArgumentException($"Interval must be a positive whole number: {args[3]}");
        
        return new CmdArgs(sourceFolderPath, replicaFolderPath, logFilePath, TimeSpan.FromSeconds(interval));
    }
    
    private static bool CheckIfSame(string path1, string path2) => path1.Equals(path2, StringComparison.OrdinalIgnoreCase);
    private static bool CheckIfInside(string path1, string path2) => Path.TrimEndingDirectorySeparator(path1)
        .StartsWith(path2 + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

    public static void DisplayHelp()
    {
        Console.WriteLine("\nUsage: FolderSyncApp [sourceFolderPath] [replicaFolderPath] [logFilePath] [interval]\n");
        Console.WriteLine("[sourceFolderPath] - Provide a path to the source folder. Path must point to an existing catalog");
        Console.WriteLine("[replicaFolderPath] - Provide a path to the replica folder. If catalog doesn't exist it will be created at specified location");
        Console.WriteLine("[logFilePath] - Provide a path to the log file. If log file does not exist it will be created at specified location");
        Console.WriteLine("[interval] - Provide a number of seconds between each synchronization. Interval must be passed as a whole number without its unit\n");
        Console.WriteLine("- CAUTION - \n- Paths to provided cannot point to the same location in any possible combination");
        Console.WriteLine("- Neither replica folder nor log file cannot point to catalog inside the source directory\n- Ctrl+C to stop program");
    }
    
    public static void DisplayError(string errorMessage)
    {
        Console.WriteLine($"\nError: {errorMessage}");
        DisplayShortHelp();
    }
    
    private static void DisplayShortHelp()
    {
        Console.WriteLine("\nUsage: FolderSyncApp [sourceFolderPath] [replicaFolderPath] [logFilePath] [interval]");
        Console.WriteLine("For more detail explanation type FolderSyncApp -h or --help\n");
    }
}