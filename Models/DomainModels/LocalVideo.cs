namespace Models.DomainModels;

public class LocalVideo : BaseEntity
{
    public string Path { get; set; } = "";
    public string Extension { get; set; } = "";
    public int Width { get; set; }
    public int Height { get; set; }
    public int Fps { get; set; }
    public int Size { get; set; }
    public float Vbr { get; set; }
    public float Abr { get; set; }
}
