namespace FolderSyncApp;

public class FileComparer
{
    public static bool CompareFiles(string sourceFile, string replicaFile)
    {
        using var fsSource = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
        using var fsReplica = new FileStream(replicaFile, FileMode.Open, FileAccess.Read);
        
        if(fsSource.Length != fsReplica.Length)
            return false;
        
        var bufferSource = new byte[1024 * 1024];
        var bufferReplica = new byte[1024 * 1024];
        var s = fsSource.ReadAtLeast(bufferSource, bufferSource.Length, throwOnEndOfStream: false);
        while (s > 0)
        {
            var r = fsReplica.ReadAtLeast(bufferReplica, bufferReplica.Length, throwOnEndOfStream: false);
            if (!bufferSource.AsSpan(0, s).SequenceEqual(bufferReplica.AsSpan(0, r)))
            {
                return false;
            }
            s = fsSource.ReadAtLeast(bufferSource, bufferSource.Length, throwOnEndOfStream: false);
        }
        return true;
    }
}