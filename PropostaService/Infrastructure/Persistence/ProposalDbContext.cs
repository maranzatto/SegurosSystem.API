using Microsoft.EntityFrameworkCore;

namespace PropostaService.Infrastructure.Persistence
{
    public class ProposalDbContext : DbContext
    {
        public ProposalDbContext(DbContextOptions<ProposalDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Proposal>(entity =>
            {
                entity.ToTable("proposal");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id).HasColumnName("id");

                entity.Property(x => x.Status)
                      .HasColumnName("status")
                      .HasConversion<string>();

                entity.Property(x => x.Description).HasColumnName("description");
                entity.Property(x => x.CreatedAt).HasColumnName("created_at");
                entity.Property(x => x.UpdatedAt).HasColumnName("updated_at");
                entity.Property(x => x.RejectionReason).HasColumnName("rejection_reason");
            });
        }

        public DbSet<Proposal> Proposals { get; set; }
    }
}
