using CsPostApi.Models.Domain;
using CsPostApi.Models.Dto;

namespace CsPostApi.Services.Interfaces;

public interface IPostService
{
    Task PublishPostAsync(PostDto postDto);
    Task UpdatePostAsync(int id, PostDto postDto);
    Task DeletePostAsync(int postId);
    Task<IEnumerable<Post>> GetPostsByUsersAsync(IEnumerable<int> userIds, int page, int pageSize);
    Task<Post> GetPostAsync(int id);
}