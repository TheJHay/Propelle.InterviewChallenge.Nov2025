using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Propelle.InterviewChallenge.Application;
using Propelle.InterviewChallenge.Application.Domain;
using Propelle.InterviewChallenge.Application.Domain.Events;

namespace Propelle.InterviewChallenge.Endpoints
{
    public static class MakeDeposit
    {
        public class Request
        {
            public Guid UserId { get; set; }

            public decimal Amount { get; set; }
        }

        public class Response
        {
            public Guid DepositId { get; set; }
        }

        public class Endpoint : Endpoint<Request, Response>
        {
            private readonly PaymentsContext _paymentsContext;
            private readonly Application.EventBus.IEventBus _eventBus;

            public Endpoint(
                PaymentsContext paymentsContext,
                Application.EventBus.IEventBus eventBus)
            {
                _paymentsContext = paymentsContext;
                _eventBus = eventBus;
            }

            public override void Configure()
            {
                Post("/api/deposits/{UserId}");
            }

            public override async Task HandleAsync(Request req, CancellationToken ct)
            {
                var deposit = new Deposit(req.UserId, req.Amount);

                // Use a window to de-duplicate incoming bank deposits / make the retries idempotent
                // It's not a great solution as we could have valid deposits which look like duplicates within the window.
                // Either an appropriate window could be agreed with the business
                // Or solutions such as in/outbox pattern and/or some breaking changes to the API such as using idempotency keys will help.
                var duplicate = await _paymentsContext.Deposits.SingleOrDefaultAsync(x => 
                    x.UserId == req.UserId 
                    && x.Amount == req.Amount
                    && x.CreatedAt > (DateTime.UtcNow.AddSeconds(-30)),
                    ct);

                if (duplicate is not null)
                {
                    await SendAsync(new Response { DepositId = duplicate.Id }, 201, ct);
                    return;
                }
                await _paymentsContext.Deposits.AddAsync(deposit, ct);
                
                await _paymentsContext.SaveChangesAsync(ct);

                await _eventBus.Publish(new DepositMade
                {
                    Id = deposit.Id
                });

                await SendAsync(new Response { DepositId = deposit.Id }, 201, ct);
            }
        }
    }
}
