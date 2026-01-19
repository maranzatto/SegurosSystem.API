namespace ContratacaoService.Domain.ValueObjects
{
    public class PolicyPeriod
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public PolicyPeriod(DateTime start)
        {
            Start = start;
            End = start.AddYears(1);
        }
    }

}
