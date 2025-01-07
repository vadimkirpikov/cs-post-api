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
    public async Task PublishPostAsync_Should_Create_Post_And_Notify()
    {
        var userId = 1;
        var postDto = new PostDto { Title = "Test Title", Content = "Test Content" };
        _postRepositoryMock
            .Setup(repo => repo.CreatePostAsync(It.IsAny<Post>()))
            .Returns(Task.CompletedTask);
        _notifierMock
            .Setup(notifier => notifier.NotifyAsync(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        await _postService.PublishPostAsync(userId, postDto);

        _postRepositoryMock.Verify(repo => repo.CreatePostAsync(It.Is<Post>(p =>
            p.UserId == userId &&
            p.Title == postDto.Title &&
            p.Content == postDto.Content
        )), Times.Once);

        _notifierMock.Verify(notifier => notifier.NotifyAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_Should_Update_Post_When_User_Is_Owner()
    {
        var postId = 1;
        var userId = 1;
        var postDto = new PostDto { Title = "Updated Title", Content = "Updated Content" };
        var existingPost = new Post { Id = postId, UserId = userId, Title = "Old Title", Content = "Old Content" };

        _postRepositoryMock.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync(existingPost);
        _postRepositoryMock.Setup(repo => repo.UpdatePostAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

        
        await _postService.UpdatePostAsync(postId, userId, postDto);

        
        _postRepositoryMock.Verify(repo => repo.UpdatePostAsync(It.Is<Post>(p =>
            p.Id == postId &&
            p.Title == postDto.Title &&
            p.Content == postDto.Content
        )), Times.Once);
    }

    [Fact]
    public async Task UpdatePostAsync_Should_Throw_When_User_Is_Not_Owner()
    {
        var postId = 1;
        var userId = 2; 
        var postDto = new PostDto { Title = "Updated Title", Content = "Updated Content" };
        var existingPost = new Post { Id = postId, UserId = 1, Title = "Test title", Content = "Test content"};

        _postRepositoryMock.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync(existingPost);
        
        await Assert.ThrowsAsync<ArgumentException>(() => _postService.UpdatePostAsync(postId, userId, postDto));
    }

    [Fact]
    public async Task DeletePostAsync_Should_Delete_Post_When_User_Is_Owner()
    {
        var postId = 1;
        var userId = 1;
        var existingPost = new Post { Id = postId, UserId = userId, Title = "Test title", Content = "Test content"};

        _postRepositoryMock.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync(existingPost);
        _postRepositoryMock.Setup(repo => repo.DeletePostAsync(existingPost)).Returns(Task.CompletedTask);

        await _postService.DeletePostAsync(postId, userId);

        _postRepositoryMock.Verify(repo => repo.DeletePostAsync(existingPost), Times.Once);
    }

    [Fact]
    public async Task GetPostsByUsersAsync_Should_Throw_For_Invalid_Page_Size()
    {
        var userIds = new List<int> { 1, 2, 3 };
        var invalidPageSize = 0;
        
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _postService.GetPostsByUsersAsync(userIds, page: 1, pageSize: invalidPageSize));
    }

    [Fact]
    public async Task GetPostsByUsersAsync_Should_Return_Posts()
    {
        var userIds = new List<int> { 1, 2 };
        var page = 1;
        var pageSize = 10;
        var posts = new List<Post> { new Post { Id = 1, UserId = 1, Title = "Test title", Content = "Test content"}, new Post { Id = 2, UserId = 2, Title = "Test title", Content = "Test content"} };

        _postRepositoryMock.Setup(repo => repo.GetPostsByUsersAsync(userIds, page, pageSize)).ReturnsAsync(posts);

        var result = await _postService.GetPostsByUsersAsync(userIds, page, pageSize);

        Assert.Equal(posts, result);
    }

    [Fact]
    public async Task GetPostAsync_Should_Return_Post_If_Exists()
    {
        var postId = 1;
        var post = new Post { Id = postId, UserId = 1, Title = "Test title", Content = "Test content" };

        _postRepositoryMock.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync(post);

        var result = await _postService.GetPostAsync(postId);

        Assert.Equal(post, result);
    }

    [Fact]
    public async Task GetPostAsync_Should_Throw_If_Post_Not_Found()
    {
        var postId = 1;

        _postRepositoryMock.Setup(repo => repo.GetPostByIdAsync(postId)).ReturnsAsync((Post)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _postService.GetPostAsync(postId));
    }
}