namespace ContratacaoService.Domain.ValueObjects
{
    public class PolicyNumber
    {
        public string Value { get; }

        public PolicyNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Número da apólice inválido.");

            Value = value;
        }

        public static PolicyNumber Generate()
            => new($"POL-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Substring(0, 25));

        public static implicit operator string(PolicyNumber number) => number.Value;
        public static implicit operator PolicyNumber(string value) => new(value);
    }

}
