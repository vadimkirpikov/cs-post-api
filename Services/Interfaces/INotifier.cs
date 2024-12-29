namespace CsPostApi.Services.Interfaces;

public interface INotifier
{
    Task NotifyAsync(int postId);
}