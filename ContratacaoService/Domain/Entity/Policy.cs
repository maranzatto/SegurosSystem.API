using System;
using ContratacaoService.Domain.Enums; // Crie um enum PolicyStatus aqui, se não existir

namespace ContratacaoService.Domain.Entities
{
    public class Policy
    {
        public Guid Id { get; private set; }
        public Guid ProposalId { get; private set; }          // Referência obrigatória à proposta aprovada
        public string PolicyNumber { get; private set; }      // Número da apólice (gerado, único)
        public DateTime ContractedAt { get; private set; }    // Data da contratação
        public DateTime? EffectiveDate { get; private set; }  // Início da vigência (pode ser futura)
        public DateTime? ExpirationDate { get; private set; } // Fim da vigência
        public PolicyStatus Status { get; private set; }      // Ativa, Cancelada, etc.

        // Campos comuns em apólices de seguro (adicione conforme seu domínio)
        // public decimal TotalPremium { get; private set; }          // Prêmio total
        // public string InsuredName { get; private set; }            // Nome do segurado
        // public string InsuredCpfCnpj { get; private set; }         // CPF/CNPJ
        // public IReadOnlyCollection<Coverage> Coverages { get; private set; } = new List<Coverage>(); // Coberturas

        // Construtor principal (usado quando contrata a partir de uma proposta aprovada)
        public Policy(Guid proposalId, DateTime? effectiveDate = null)
        {
            if (proposalId == Guid.Empty)
                throw new ArgumentException("ID da proposta é obrigatório para contratar.", nameof(proposalId));

            Id = Guid.NewGuid();
            ProposalId = proposalId;
            PolicyNumber = GeneratePolicyNumber(); // Lógica de geração (veja abaixo)
            ContractedAt = DateTime.UtcNow;
            EffectiveDate = effectiveDate ?? ContractedAt; // Default: vigência imediata
            ExpirationDate = EffectiveDate.Value.AddYears(1); // Exemplo: 1 ano de vigência (ajuste conforme regras)
            Status = PolicyStatus.Active;

            // Validações adicionais se precisar (ex: vigência futura não pode ser no passado)
            if (EffectiveDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Data de início de vigência não pode ser no passado.");
        }

        // Método para cancelar (exemplo de comportamento)
        public void Cancel(string? reason = null)
        {
            if (Status != PolicyStatus.Active)
                throw new InvalidOperationException($"Apólice no status {Status} não pode ser cancelada.");

            Status = PolicyStatus.Canceled;
            // CancellationReason = reason; // se quiser guardar

            // Opcional: Domain Event → PolicyCanceledEvent
        }

        // Outros métodos futuros: Renew(), Endorse(), Suspend(), CalculatePremium()...

        // Geração de número da apólice (exemplo simples; pode vir de um serviço sequencial)
        private string GeneratePolicyNumber()
        {
            // Em produção: use um sequence no banco ou serviço distribuído
            return $"POL-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }

        // Construtor vazio para EF Core (shadow properties, etc.)
        private Policy() { }
    }
}