namespace DocVault.API.Models;

public class Document
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string FilePath { get; set; } = "";
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;


}