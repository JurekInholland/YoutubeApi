namespace Models;

public abstract class Enums
{
    public enum DownloadStatus
    {
        Queued,
        Downloading,
        Finished,
        Error
    }

    public enum ApplicationTask
    {
        ProcessDownloadQueue,
        ProcessDownloadQueueItem,
        CleanUpDownloadQueue,
    }

    public enum TaskStatus
    {
        NeverRan,
        Started,
        Finished,
        Error
    }
}
