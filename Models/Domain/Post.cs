using System.ComponentModel.DataAnnotations;

namespace CsPostApi.Models.Domain;

public class Post
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    [MaxLength(30)]
    public required string Title { get; set; }
    [MaxLength(1000)]
    public required string Content { get; set; }
    public DateTime PublishDate { get; set; } = DateTime.UtcNow;
}