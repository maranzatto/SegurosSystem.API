using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ContratacaoService.Infrastructure.Persistence
{
    public class PolicyDbContext : DbContext
    {
        public PolicyDbContext(DbContextOptions<PolicyDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Policy>(entity =>
            {
                entity.ToTable("policy");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id).HasColumnName("id");

                entity.Property(x => x.ProposalId)
                      .HasColumnName("proposal_id")
                      .IsRequired();

                entity.Property(x => x.Status)
                      .HasColumnName("status")
                      .HasConversion<string>();

                entity.Property(x => x.ContractedAt)
                      .HasColumnName("contracted_at");

                entity.Property(x => x.IsDeleted)
                      .HasColumnName("is_deleted");

                entity.Property(x => x.PolicyNumber)
                      .HasColumnName("policy_number")
                      .HasConversion(
                          v => v.Value,
                          v => new PolicyNumber(v)
                      );

                entity.OwnsOne(x => x.Period, period =>
                {
                    period.Property(p => p.Start).HasColumnName("effective_date");
                    period.Property(p => p.End).HasColumnName("expiration_date");
                });
            });
        }

        public DbSet<Policy> Policy { get; set; }
    }
}
