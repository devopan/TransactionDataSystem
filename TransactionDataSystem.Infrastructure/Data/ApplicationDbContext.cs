using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TransactionDataSystem.Domain.Entities;

namespace TransactionDataSystem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Transaction> Transactions { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<UserTransaction>()
                .HasOne(ua => ua.User)
                .WithMany()
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserTransaction>()
                .HasOne(ua => ua.Transaction)
                .WithMany()
                .HasForeignKey(ua => ua.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasIndex(tr => tr.TransactionType);

        }
    }
}