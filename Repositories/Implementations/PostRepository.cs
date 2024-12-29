using CsPostApi.Data;
using CsPostApi.Models.Domain;
using CsPostApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CsPostApi.Repositories.Implementations;

public class PostRepository(ApplicationDbContext context): IPostRepository
{
    public async Task CreatePostAsync(Post post)
    {
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Post>> GetAllPostsAsync(int page, int pageSize)
    {
        return await context.Posts.OrderBy(post => post.PublishDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        return await context.Posts.SingleOrDefaultAsync(post => post.Id == id);
    }

    public async Task<IEnumerable<Post>> GetPostsByUsersAsync(IEnumerable<int> userIds, int page, int pageSize)
    {
        return await context.Posts.Where(post => userIds.Contains(post.UserId))
            .OrderBy(post => post.PublishDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task UpdatePostAsync(Post post)
    {
        context.Posts.Update(post);
        await context.SaveChangesAsync();
    }

    public async Task DeletePostAsync(Post post)
    {
        context.Posts.Remove(post);
        await context.SaveChangesAsync();
    }
}