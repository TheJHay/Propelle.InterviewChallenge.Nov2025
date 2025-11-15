namespace Propelle.InterviewChallenge.Application.Domain
{
    public class Deposit
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public decimal Amount { get; }

        public DateTime CreatedAt { get; }

        public Deposit(Guid userId, decimal amount) : this(Guid.NewGuid(), userId, amount, DateTime.UtcNow) { }

        public Deposit(Guid id, Guid userId, decimal amount, DateTime createdAt)
        {
            Id = id;
            UserId = userId;
            Amount = amount;
            CreatedAt = createdAt;
        }
    }
}
