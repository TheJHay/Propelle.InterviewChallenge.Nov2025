namespace Propelle.InterviewChallenge.EventHandling
{
    public interface IEventHandler<TEvent> where TEvent : class
    {
        Task Handle(TEvent @event);
    }
}
