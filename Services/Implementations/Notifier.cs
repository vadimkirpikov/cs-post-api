using Confluent.Kafka;
using CsPostApi.Models.Dto;
using CsPostApi.Services.Interfaces;

namespace CsPostApi.Services.Implementations;

public class Notifier(IProducer<Null, string> producer, ILogger<Notifier> logger): INotifier
{
    private const string KafkaTopic = "post.published";
    public async Task NotifyAsync(int postId)
    {
        try
        {
            var notification = new NotificationDto { PostId = postId };
            await producer.ProduceAsync(KafkaTopic, new Message<Null, string> { Value = notification.ToJson() });
            logger.LogInformation("Notification about post with id {id} was delivered successfully.", postId);
        }
        catch (KafkaException e)
        {
            logger.LogError("Failed to deliver notifications with {ex}", e.ToString());
        }
    }
}