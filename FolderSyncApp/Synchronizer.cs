namespace FolderSyncApp;

public class Synchronizer(string sourceFolder, string replicaFolder,  Logger logger)
{
    public void Synchronize()
    {
        var sourceFiles = GetFileList(sourceFolder);
        var replicaFiles = GetFileList(replicaFolder);
        var sourceFolderTree = GetDirectories(sourceFolder);
        var replicaFolderTree = GetDirectories(replicaFolder);
        foreach (var file in replicaFiles.Except(sourceFiles))
            ExecuteSafely(file, "Deleting file", () => DeleteFiles(file));
        foreach (var directory in replicaFolderTree.Except(sourceFolderTree).OrderDescending())
            ExecuteSafely(directory, "Deleting folder", () => DeleteFolder(directory));
        foreach (var directory in sourceFolderTree.Except(replicaFolderTree))
            ExecuteSafely(directory, "Creating folder", () => CreateFolder(directory));
        foreach (var file in sourceFiles)
            ExecuteSafely(file, "Writing File", () => UpdateFile(file, replicaFiles.Contains(file)));
    }

    private HashSet<string> GetFileList(string folderPath) => Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
        .Select(f => Path.GetRelativePath(folderPath, f))
        .ToHashSet(StringComparer.OrdinalIgnoreCase);
    
    private HashSet<string> GetDirectories(string path) => Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories)
        .Select(d => Path.GetRelativePath(path, d))
        .ToHashSet(StringComparer.OrdinalIgnoreCase);
    
    private void DeleteFiles(string fileToDelete)
    {
        var path = Path.Combine(replicaFolder, fileToDelete);
        File.Delete(path);
        logger.AppendLog($"Deleted file: {fileToDelete}");
    }

    private void DeleteFolder(string folderToDelete)
    {
        var path = Path.Combine(replicaFolder, folderToDelete);
        if (!Directory.Exists(path))
            return;
        Directory.Delete(path, false);
        logger.AppendLog($"Deleted folder: {folderToDelete}");
    }

    private void CreateFolder(string folderPath)
    {
        Directory.CreateDirectory(Path.Combine(replicaFolder, folderPath));
        logger.AppendLog($"Created folder: {folderPath}");
    }
    
    private void UpdateFile(string fileToUpdate, bool replicaExists)
    {
        var sourceFile = Path.Combine(sourceFolder, fileToUpdate);
        var replicaFile =  Path.Combine(replicaFolder, fileToUpdate);

        if (replicaExists)
            if (FileComparer.CompareFiles(sourceFile, replicaFile))
                return;
        File.Copy(sourceFile, replicaFile, true);
        logger.AppendLog($"{(replicaExists ? "Updated" : "Created")} file: {fileToUpdate}");
    }

    private void ExecuteSafely(string path, string operation, Action action)
    {
        try
        {
            action();
        }
        catch(Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            logger.AppendLog($"Error while {operation} {path}: {e.Message}");
        }
    }
}