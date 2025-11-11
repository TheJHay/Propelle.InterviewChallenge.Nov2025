namespace Propelle.InterviewChallenge.Application.Domain
{
    public class Deposit
    {
        public Guid Id { get; }

        public Guid UserId { get; }

        public decimal Amount { get; }

        public Deposit(Guid userId, decimal amount) : this(Guid.NewGuid(), userId, amount) { }

        public Deposit(Guid id, Guid userId, decimal amount)
        {
            Id = id;
            UserId = userId;
            Amount = amount;
        }
    }
}
