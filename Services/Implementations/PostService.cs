using CsPostApi.Models.Domain;
using CsPostApi.Models.Dto;
using CsPostApi.Repositories.Interfaces;
using CsPostApi.Services.Interfaces;

namespace CsPostApi.Services.Implementations;

public class PostService(IPostRepository postRepository, INotifier notifier) : IPostService
{
    private async Task<Post> TryGetPostByIdAsync(int id)
    {
        var existingPost = await postRepository.GetPostByIdAsync(id);
        if (existingPost == null)
        {
            throw new ArgumentException($"Post with id: {id} does not exist");
        }
        return existingPost;
    }

    private static void CheckOwnership(Post post, int userId)
    {
        if (post.UserId != userId)
        {
            throw new ArgumentException($"User with id={userId} is not owner of the post with id={post.Id}");
        }
    }
    public async Task PublishPostAsync(int userId, PostDto postDto)
    {
        var post = new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            UserId = userId,
        };
        await postRepository.CreatePostAsync(post);
        await notifier.NotifyAsync(post.Id);
    }

    public async Task UpdatePostAsync(int postId, int userId, PostDto postDto)
    {
        var existingPost = await TryGetPostByIdAsync(postId);
        CheckOwnership(existingPost, userId);
        existingPost.Title = postDto.Title;
        existingPost.Content = postDto.Content;
        await postRepository.UpdatePostAsync(existingPost);
    }

    public async Task DeletePostAsync(int postId, int userId)
    {
        var existingPost = await TryGetPostByIdAsync(postId);
        CheckOwnership(existingPost, userId);
        await postRepository.DeletePostAsync(existingPost);
    }

    public async Task<IEnumerable<Post>> GetPostsByUsersAsync(IEnumerable<int> userIds, int page, int pageSize)
    {
        if (pageSize <= 0 || pageSize > 1000 || page <= 0)
        {
            throw new ArgumentOutOfRangeException($"Invalid page or page size: {pageSize}");
        }
        return await postRepository.GetPostsByUsersAsync(userIds, page, pageSize);
    }

    public async Task<Post> GetPostAsync(int id)
    {
        return await TryGetPostByIdAsync(id);
    }
}