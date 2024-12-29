using CsPostApi.Models.Domain;
using CsPostApi.Models.Dto;
using CsPostApi.Repositories.Interfaces;
using CsPostApi.Services.Interfaces;

namespace CsPostApi.Services.Implementations;

public class PostService(IPostRepository postRepository) : IPostService
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
    public async Task PublishPostAsync(PostDto postDto)
    {
        var post = new Post
        {
            Title = postDto.Title,
            Content = postDto.Content,
            UserId = postDto.UserId
        };
        await postRepository.CreatePostAsync(post);
    }

    public async Task UpdatePostAsync(int id, PostDto postDto)
    {
        var existingPost = await TryGetPostByIdAsync(id);
        existingPost.Title = postDto.Title;
        existingPost.Content = postDto.Content;
        await postRepository.UpdatePostAsync(existingPost);
    }

    public async Task DeletePostAsync(int id)
    {
        var existingPost = await TryGetPostByIdAsync(id);
        await postRepository.DeletePostAsync(existingPost);
    }

    public async Task<IEnumerable<Post>> GetPostsByUsersAsync(IEnumerable<int> userIds, int page, int pageSize)
    {
        return await postRepository.GetPostsByUsersAsync(userIds, page, pageSize);
    }

    public async Task<Post> GetPostAsync(int id)
    {
        return await TryGetPostByIdAsync(id);
    }
}