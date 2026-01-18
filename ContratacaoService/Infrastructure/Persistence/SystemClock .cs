using ContratacaoService.Domain.Common;

namespace PropostaService.Infrastructure.Persistence
{
    public class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
