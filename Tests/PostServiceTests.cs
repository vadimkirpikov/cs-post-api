using CsPostApi.Models.Domain;
using CsPostApi.Models.Dto;
using CsPostApi.Repositories.Interfaces;
using CsPostApi.Services.Implementations;
using CsPostApi.Services.Interfaces;
using Moq;
using Xunit;

namespace CsPostApi.Tests;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly Mock<INotifier> _notifierMock;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _notifierMock = new Mock<INotifier>();
        _postService = new PostService(_postRepositoryMock.Object, _notifierMock.Object);
    }

    [Fact]
    public async Task PublishPostAsync_CreatesPostAndNotifies()
    {
        var postDto = new PostDto { Title = "Test", Content = "Test content", UserId = 1 };
        _postRepositoryMock.Setup(r => r.CreatePostAsync(It.IsAny<Post>()))
            .Returns(Task.CompletedTask);

        
        await _postService.PublishPostAsync(postDto);
        
        _postRepositoryMock.Verify(r => r.CreatePostAsync(It.Is<Post>(p => p.Title == "Test" && p.Content == "Test content" && p.UserId == 1)), Times.Once);
        _notifierMock.Verify(n => n.NotifyAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_UpdatesExistingPost()
    {
        
        var postDto = new PostDto { Title = "Updated", Content = "Updated content", UserId = 1 };
        var existingPost = new Post { Id = 1, Title = "Old", Content = "Old content", UserId = 1 };

        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(1))
            .ReturnsAsync(existingPost);

        
        await _postService.UpdatePostAsync(1, postDto);

        
        _postRepositoryMock.Verify(r => r.UpdatePostAsync(It.Is<Post>(p => p.Title == "Updated" && p.Content == "Updated content")), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_ThrowsWhenPostDoesNotExist()
    {
        
        var postDto = new PostDto { Title = "Updated", Content = "Updated content", UserId = 1 };

        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(1))
            .ReturnsAsync((Post)null);

        
        await Assert.ThrowsAsync<ArgumentException>(() => _postService.UpdatePostAsync(1, postDto));
    }

    [Fact]
    public async Task DeletePostAsync_DeletesExistingPost()
    {
        
        var existingPost = new Post { Id = 1, Title = "Test", Content = "Test content", UserId = 1 };

        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(1))
            .ReturnsAsync(existingPost);

        
        await _postService.DeletePostAsync(1);

        
        _postRepositoryMock.Verify(r => r.DeletePostAsync(existingPost), Times.Once);
    }

    [Fact]
    public async Task DeletePostAsync_ThrowsWhenPostDoesNotExist()
    {
        
        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(1))
            .ReturnsAsync((Post)null);

        
        await Assert.ThrowsAsync<ArgumentException>(() => _postService.DeletePostAsync(1));
    }

    [Fact]
    public async Task GetPostsByUsersAsync_ReturnsPosts()
    {
        
        var userIds = new List<int> { 1, 2, 3 };
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Test1", Content = "Content1", UserId = 1 },
            new Post { Id = 2, Title = "Test2", Content = "Content2", UserId = 2 }
        };

        _postRepositoryMock.Setup(r => r.GetPostsByUsersAsync(userIds, 1, 10))
            .ReturnsAsync(posts);

        
        var result = await _postService.GetPostsByUsersAsync(userIds, 1, 10);

        
        Assert.Equal(2, result.Count());
        _postRepositoryMock.Verify(r => r.GetPostsByUsersAsync(userIds, 1, 10), Times.Once);
    }

    [Fact]
    public async Task GetPostAsync_ReturnsExistingPost()
    {
        
        var post = new Post { Id = 1, Title = "Test", Content = "Test content", UserId = 1 };

        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(1))
            .ReturnsAsync(post);

        
        var result = await _postService.GetPostAsync(1);

        
        Assert.Equal(post, result);
    }

    [Fact]
    public async Task GetPostAsync_ThrowsWhenPostDoesNotExist()
    {
        
        _postRepositoryMock.Setup(r => r.GetPostByIdAsync(1))
            .ReturnsAsync((Post)null);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _postService.GetPostAsync(1));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 1001)]
    [InlineData(1, 0)]
    public async Task GetPostsAsync_ThrowsOutOfRangeException(int page, int pageSize)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _postService.GetPostsByUsersAsync(new List<int> { 1, 2, 3 }, page, pageSize));
    }
}