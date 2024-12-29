namespace CsPostApi.Models.Dto;

public class PostDto
{
    public required int UserId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
}