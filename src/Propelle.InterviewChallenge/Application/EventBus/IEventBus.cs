namespace Propelle.InterviewChallenge.Application.EventBus
{
    public interface IEventBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : class;
    }
}
