using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DataAccessClassLibrary.EFModels
{
    public partial class ElectionDBContext : DbContext
    {
        private string _conStr;
        public ElectionDBContext(string conStr)
        {
            _conStr = conStr;
        }

        public ElectionDBContext(DbContextOptions<ElectionDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Candidates> Candidates { get; set; }
        public virtual DbSet<Elections> Elections { get; set; }
        public virtual DbSet<VotersOfElection> VotersOfElection { get; set; }
        public virtual DbSet<VotesOfElection> VotesOfElection { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_conStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UC_AdminName")
                    .IsUnique();

                entity.Property(e => e.HashedPw)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Candidates>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Election)
                    .WithMany(p => p.Candidates)
                    .HasForeignKey(d => d.ElectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Candidate__Elect__32E0915F");
            });

            modelBuilder.Entity<Elections>(entity =>
            {
                entity.HasIndex(e => e.Header)
                    .HasName("UQ_ElectionHeader")
                    .IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Elections)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Elections__Admin__30F848ED");
            });

            modelBuilder.Entity<VotersOfElection>(entity =>
            {
                entity.HasIndex(e => e.HashedPw)
                    .HasName("UQ_HashedPw")
                    .IsUnique();

                entity.HasIndex(e => new { e.Email, e.ElectionId })
                    .HasName("UQ_EmailAndElectionId")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.HashedPw)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Election)
                    .WithMany(p => p.VotersOfElection)
                    .HasForeignKey(d => d.ElectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VotersOfE__Elect__300424B4");
            });

            modelBuilder.Entity<VotesOfElection>(entity =>
            {
                entity.Property(e => e.Vote)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Election)
                    .WithMany(p => p.VotesOfElection)
                    .HasForeignKey(d => d.ElectionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VotesOfEl__Elect__31EC6D26");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
