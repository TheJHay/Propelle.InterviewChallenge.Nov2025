namespace Propelle.InterviewChallenge.EventHandling
{
    public class InMemoryEventExchange
    {
        private readonly IServiceProvider _serviceProvider;

        public InMemoryEventExchange(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Publish<TEvent>(TEvent @event)
            where TEvent : class
        {
            using var scope = _serviceProvider.CreateScope();

            var eventHandlers = scope.ServiceProvider.GetServices(typeof(IEventHandler<TEvent>))
                .Cast<IEventHandler<TEvent>>();

            foreach (var eventHandler in eventHandlers)
            {
                await RedriveOnFailure(() => eventHandler.Handle(@event));
            }
        }

        // Here i'm extending this simple mechanism to simulate redriven messages.
        // This could be extended to only redrive "redrivable" exceptions - e.g. if some business rules were broken
        // but likely in a real scenario we'd want to have that message consumed and do something else with it depending on the scenario.
        private static async Task RedriveOnFailure(Func<Task> handle)
        {
            var succeeded = false;
            do
            {
                try
                {
                    await handle();
                    succeeded = true;
                }
                catch { }
            }
            while(!succeeded);
        }
    }
}
