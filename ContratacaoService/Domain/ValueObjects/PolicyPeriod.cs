namespace ContratacaoService.Domain.ValueObjects
{
    public class PolicyPeriod
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public PolicyPeriod(DateTime start)
        {
            if (start < DateTime.UtcNow)
                throw new ArgumentException("Início da vigência inválido.");

            Start = start;
            End = start.AddYears(1);
        }
    }

}
