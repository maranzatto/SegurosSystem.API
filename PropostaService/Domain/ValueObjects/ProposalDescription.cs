namespace PropostaService.Domain.ValueObjects
{
    public class ProposalDescription
    {
        public string Value { get; }
        public ProposalDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Descrição é obrigatória.");
            if (value.Trim().Length < 10)
                throw new ArgumentException("Descrição deve ter pelo menos 10 caracteres.");
            if (value.Trim().Length > 500)
                throw new ArgumentException("Descrição não pode exceder 500 caracteres.");
            Value = value.Trim();
        }
        public static implicit operator string(ProposalDescription description) => description.Value;
        public static implicit operator ProposalDescription(string value) => new(value);
        public override bool Equals(object? obj)
        {
            return obj is ProposalDescription other && Value == other.Value;
        }
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}