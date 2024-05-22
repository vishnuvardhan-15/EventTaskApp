using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EventDataAccessLayer.Models
{
    public partial class EventDBContext : DbContext
    {
        public EventDBContext(DbContextOptions<EventDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Events> EventTable { get; set; }

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

                entity.Property(e => e.TimeStampValue);
            });
        }
    }
}