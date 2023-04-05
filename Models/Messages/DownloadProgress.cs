namespace Models;

public class DownloadProgress
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public float Progress { get; set; }
    public string Speed { get; set; } = string.Empty;
    public TimeSpan Eta { get; set; }

    public string Fragment { get; set; } = string.Empty;

    public string ToString()
    {
        return $"{DateTime.Now.ToString("T")} {Id} [{Status}] {Progress}% {Speed} ETA: {Eta} Frag: {Fragment}";
    }
}
