namespace FolderSyncApp;

public record CmdArgs(string SourceFolder, string ReplicaFolder, string LogFilePath, TimeSpan Interval);