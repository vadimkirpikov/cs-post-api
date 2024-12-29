using CsPostApi.Models.Domain;

namespace CsPostApi.Repositories.Interfaces;

public interface IPostRepository
{
    Task CreatePostAsync(Post post);
    Task<IEnumerable<Post>> GetAllPostsAsync(int page, int pageSize);
    Task<Post?> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> GetPostsByUsersAsync(IEnumerable<int> userIds, int page, int pageSize);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(Post post);
}