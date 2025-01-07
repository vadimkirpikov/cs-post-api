using CsPostApi.Models.Domain;
using CsPostApi.Models.Dto;

namespace CsPostApi.Services.Interfaces;

public interface IPostService
{
    Task PublishPostAsync(int userId, PostDto postDto);
    Task UpdatePostAsync(int postId, int userId, PostDto postDto);
    Task DeletePostAsync(int postId, int userId);
    Task<IEnumerable<Post>> GetPostsByUsersAsync(IEnumerable<int> userIds, int page, int pageSize);
    Task<Post> GetPostAsync(int id);
}