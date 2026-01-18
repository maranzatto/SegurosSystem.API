namespace ContratacaoService.Domain.Common
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
