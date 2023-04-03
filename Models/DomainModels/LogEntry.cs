namespace Models.DomainModels;

public class LogEntry
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; } = string.Empty;
}
