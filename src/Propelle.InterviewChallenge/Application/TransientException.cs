namespace Propelle.InterviewChallenge.Application
{
    [Serializable]
    public class TransientException : Exception
    {
        public TransientException() : base("Oh no! Something didn't work") { }
    }
}
