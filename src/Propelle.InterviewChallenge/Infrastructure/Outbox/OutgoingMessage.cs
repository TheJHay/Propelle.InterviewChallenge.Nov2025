using System.Text.Json;

namespace Propelle.InterviewChallenge.Infrastructure.Outbox;

public class OutgoingMessage
{
    public Guid MessageId { get; }
    public DateTime CreatedAt { get; }
    public DateTime? LastRetry { get; set; }
    public int Version { get; set; }
    public string MessageType { get; }
    public string Message { get; }
    public bool IsProcessed { get; set; }

    private OutgoingMessage(Guid messageId, string messageType, string message) : this(
        messageId,
        messageType,
        message,
        DateTime.UtcNow,
        null,
        0,
        false
        )
    {

    }

    public OutgoingMessage(
        Guid messageId,
        string messageType,
        string message,
        DateTime createdAt,
        DateTime? lastRetry,
        int version,
        bool isProcessed
        )
    {
        MessageId = messageId;
        MessageType = messageType;
        Message = message;
        CreatedAt = createdAt;
        LastRetry = lastRetry;
        Version = version;
        IsProcessed = isProcessed;
    }

    public static OutgoingMessage Create<T>(Guid id, T message)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var messageType = typeof(T).Name;

        return new(id, messageType, messageJson);
    }
}
