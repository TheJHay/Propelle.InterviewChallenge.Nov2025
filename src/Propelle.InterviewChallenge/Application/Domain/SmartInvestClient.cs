namespace Propelle.InterviewChallenge.Application.Domain
{
    public class SmartInvestClient : ISmartInvestClient
    {
        private readonly List<(Guid UserId, decimal Amount)> _submittedDeposits;

        public IEnumerable<(Guid UserId, decimal Amount)> SubmittedDeposits => _submittedDeposits;

        public SmartInvestClient()
        {
            _submittedDeposits = new List<(Guid UserId, decimal Amount)>();
        }

        public Task SubmitDeposit(Guid userId, decimal amount)
        {
            PointOfFailure.SimulatePotentialFailure();

            _submittedDeposits.Add((userId, amount));

            return Task.CompletedTask;
        }
    }
}
