namespace CsPostApi.Models.Domain;

public class Post
{
    public required int Id { get; set; }
    public required int UserId { get; set; }
    public required string UserFullName { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}