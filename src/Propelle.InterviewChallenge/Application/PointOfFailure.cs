namespace Propelle.InterviewChallenge.Application
{
    public static class PointOfFailure
    {
        public static Random _random;

        static PointOfFailure()
        {
            _random = new Random();
        }

        public static void SimulatePotentialFailure(double percentageChanceOfFailure = 0.2)
        {
            var val = _random.NextDouble();

            if (val < percentageChanceOfFailure)
            {
                throw new TransientException();
            }
        }
    }
}
