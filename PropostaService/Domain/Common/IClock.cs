namespace PropostaService.Domain.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
