using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace EventDataAccessLayer.Models
{
    public partial class EventDBContext : DbContext
    {
        public EventDBContext()
        {
        }

        public EventDBContext(DbContextOptions<EventDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Events> Event { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("EventDBConnectionString");
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.CompanyId);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.JobId);

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .IsUnicode(false);
                entity.Property(e => e.FundValue);

                entity.Property(e => e.EventTriggerType)
                    .IsRequired();

                entity.Property(e => e.EventTriggeredBy).IsRequired();

                entity.Property(e => e.PaymentStatus)
                    .IsRequired();

                entity.Property(e => e.RefundStatus)
                    .IsRequired();

                entity.Property(e => e.UserComments).IsRequired();

                entity.Property(e => e.TimeStampValue)
                    .HasColumnType("datetime");

            });
        }
    }
}
