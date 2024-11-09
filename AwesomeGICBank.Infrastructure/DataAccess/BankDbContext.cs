using AwesomeGICBank.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGICBank.Infrastructure.DataAccess
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) :
            base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<InterestRule> InterestsRules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<BankAccount>()
                .HasIndex(t => t.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<BankAccount>()
                .Property(t => t.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Type)
                .HasConversion<string>()
                .HasMaxLength(1);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BankAccount>()
                .HasMany(b => b.Transactions)
                .WithOne(t => t.BankAccount)
                .HasForeignKey(t => t.BankAccountId);

            modelBuilder.Entity<InterestRule>()
                .HasKey(ir => ir.Id);

            modelBuilder.Entity<InterestRule>()
                .Property(ir => ir.Rate)
                .HasPrecision(4, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
