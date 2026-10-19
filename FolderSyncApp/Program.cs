namespace FolderSyncApp;

class Program
{
    static async Task<int> Main(string[] args)
    {
        if (CmdHandler.CheckHelp(args))
            return 0;
        
        CmdArgs cmdArgs;
        Logger logger;
        try
        {
            cmdArgs = CmdHandler.ParseArgs(args);
            logger = new Logger(cmdArgs.LogFilePath);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Parameters provided: {args[0]}, {args[1]}, {args[2]}, {args[3]}");
            CmdHandler.DisplayError(e.Message);
            return 1;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            CmdHandler.DisplayError(e.Message);
            return 2;
        }
        Console.WriteLine("Folder synchronization started - to stop synchronizing folders use Ctrl+C ");
        var synchronizer = new Synchronizer(cmdArgs.SourceFolder, cmdArgs.ReplicaFolder, logger );
        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cts.Cancel();
        };
        using var timer = new PeriodicTimer(cmdArgs.Interval);
        try
        {
            do
            {
                synchronizer.Synchronize();
            } while (await timer.WaitForNextTickAsync(cts.Token));
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Synchronization stopped");
        }
        return 0;
    }
}