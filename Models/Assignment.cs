using System.ComponentModel.DataAnnotations;

public class Assignment
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    public string? FilePath { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}