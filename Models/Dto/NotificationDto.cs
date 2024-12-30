using System.Text.Json;

namespace CsPostApi.Models.Dto;

public class NotificationDto
{
    public required int PostId { private get; set; }
    
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}