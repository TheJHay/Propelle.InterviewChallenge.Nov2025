using Propelle.InterviewChallenge.Application.Domain.Events;
using Propelle.InterviewChallenge.Application.EventBus;

namespace Propelle.InterviewChallenge.Infrastructure.Outbox;

public interface IOutboxMessageHandler<TMessage> where TMessage : class
{
    Task HandleAsync(TMessage message);
}

public class OnDepositMade : IOutboxMessageHandler<DepositMade>
{
    private readonly IEventBus _eventBus;

    public OnDepositMade(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async Task HandleAsync(DepositMade message)
    {
        await _eventBus.Publish(message);
    }
}