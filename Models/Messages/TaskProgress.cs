namespace Models;

public class TaskProgress
{
    public DateTime Time { get; set; }
    public Enums.ApplicationTask Task { get; set; }
    public Enums.TaskStatus Status { get; set; }
}
