using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Propelle.InterviewChallenge.Application.Domain;
using Propelle.InterviewChallenge.Infrastructure.Outbox;

namespace Propelle.InterviewChallenge.Application
{
    public class PaymentsContext : DbContext
    {
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<OutgoingMessage> Outbox { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var depositConfig = modelBuilder.Entity<Deposit>();
            depositConfig.HasKey(x => x.Id);
            depositConfig.Property(x => x.UserId).IsRequired();
            depositConfig.Property(x => x.Amount).IsRequired();
            depositConfig.Property(x => x.CreatedAt).IsRequired();

            var outboxConfig = modelBuilder.Entity<OutgoingMessage>();
            outboxConfig.HasKey(x => x.MessageId);
            outboxConfig.Property(x => x.CreatedAt).IsRequired();
            outboxConfig.Property(x => x.LastRetry);
            outboxConfig.Property(x => x.Version).IsRequired();
            outboxConfig.Property(x => x.MessageType).IsRequired();
            outboxConfig.Property(x => x.Message).IsRequired();
            outboxConfig.Property(x => x.IsProcessed).IsRequired();
            outboxConfig.HasIndex(x => x.IsProcessed);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Payments");
            optionsBuilder.AddInterceptors(new FlakyDatabaseTransactionSimulator());
        }

        public class FlakyDatabaseTransactionSimulator : SaveChangesInterceptor
        {
            public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
            {
                PointOfFailure.SimulatePotentialFailure();

                return base.SavingChanges(eventData, result);
            }

            public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
            {
                PointOfFailure.SimulatePotentialFailure();

                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }
        }
    }
}
