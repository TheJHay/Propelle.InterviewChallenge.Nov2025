
using Microsoft.EntityFrameworkCore;
using Propelle.InterviewChallenge.Application;
using System.Text.Json;

namespace Propelle.InterviewChallenge.Infrastructure.Outbox;

public class OutboxProcessor<TMessage> : BackgroundService where TMessage : class
{
    private readonly IServiceProvider _serviceProvider;

    // background service poll and process outbox messages
    // register per message type to process
    // I would use wolverine to do this but changing the db to postgres/sql server felt out of scope for the task.

    public OutboxProcessor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBatchAsync(stoppingToken);
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
                }
            }
        });

    }

    // this polls the outbox table and attempts to send messages
    private async Task ProcessBatchAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IOutboxMessageHandler<TMessage>>();
        var context = scope.ServiceProvider.GetRequiredService<PaymentsContext>();

        var messages = await context.Outbox
            .Where(m => !m.IsProcessed && m.MessageType == typeof(TMessage).Name)
            .ToListAsync(ct);

        foreach (var message in messages)
        {
            try
            {
                var messageToSend = JsonSerializer.Deserialize<TMessage>(message.Message);
                await handler.HandleAsync(messageToSend);
                message.IsProcessed = true;
            }
            catch { }
            finally
            {
                message.Version++;
                message.LastRetry = DateTime.UtcNow;
            }
        }

        while (true)
        {
            try
            {
                await context.SaveChangesAsync(ct);
                break;
            }
            catch(TransientException) 
            {

            }
        }

        
    }
}
