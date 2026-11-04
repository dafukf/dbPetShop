using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Petshop_SAEP.Models;

public partial class DbPetTutorContext : DbContext
{
    public DbPetTutorContext()
    {
    }

    public DbPetTutorContext(DbContextOptions<DbPetTutorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendamento> Agendamento { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Tutor> Tutor { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConexaoSqlServer");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Agendamento");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.MedicoConsulta)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoPetNavigation).WithMany(p => p.Agendamento)
                .HasForeignKey(d => d.CodigoPet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agendamento_Pets");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Raca)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CodigoTutorNavigation).WithMany(p => p.Pets)
                .HasForeignKey(d => d.CodigoTutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pets_Tutor");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Codigo);

            entity.ToTable("Tutor");

            entity.Property(e => e.Cpf)
                .IsUnicode(false)
                .HasColumnName("CPF");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
