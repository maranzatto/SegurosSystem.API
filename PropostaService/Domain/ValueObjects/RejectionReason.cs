namespace PropostaService.Domain.ValueObjects
{
    public class RejectionReason
    {
        public string Value { get; }
        public RejectionReason(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Motivo da rejeição é obrigatório.");
            if (value.Trim().Length < 5)
                throw new ArgumentException("Motivo deve ter pelo menos 5 caracteres.");
            if (value.Trim().Length > 200)
                throw new ArgumentException("Motivo não pode exceder 200 caracteres.");
            Value = value.Trim();
        }
        public static implicit operator string(RejectionReason reason) => reason.Value;
        public static implicit operator RejectionReason(string value) => new(value);
        public override bool Equals(object? obj)
        {
            return obj is RejectionReason other && Value == other.Value;
        }
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}