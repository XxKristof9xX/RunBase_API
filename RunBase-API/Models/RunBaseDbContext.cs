using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Runbase_API.Models;

namespace RunBase_API.Models;

public partial class RunBaseDbContext : DbContext
{
    public RunBaseDbContext()
    {
    }

    public RunBaseDbContext(DbContextOptions<RunBaseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Felhasznalok> Felhasznaloks { get; set; }

    public virtual DbSet<Jogosultsag> Jogosultsags { get; set; }

    public virtual DbSet<Versenytav> Versenytavs { get; set; }
    public virtual DbSet<Versenyek> Versenyeks { get; set; }

    public virtual DbSet<Versenyindulas> Versenyindulas { get; set; }

    public virtual DbSet<Versenyzo> Versenyzos { get; set; }
    public virtual DbSet<Forum> ForumBejegyzesek { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Felhasznalok>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_felhasznalok_Id");

            entity.ToTable("felhasznalok", "runbase");

            entity.HasIndex(e => e.VersenyzoId, "felhasznalok$versenyzoID").IsUnique();

            entity.Property(e => e.Jelszo)
                .HasMaxLength(100)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("jelszo");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
            entity.Property(e => e.Tipus)
                .HasMaxLength(13)
                .HasColumnName("tipus");
            entity.Property(e => e.VersenyzoId)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("versenyzoID");

            entity.HasOne(d => d.Versenyzo).WithOne(p => p.Felhasznalok)
                .HasForeignKey<Felhasznalok>(d => d.VersenyzoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("felhasznalok$felhasznalok_ibfk_1");
        });

        modelBuilder.Entity<Jogosultsag>(entity =>
        {
            entity.HasKey(e => new { e.FelhasznaloId, e.VersenyId });

            entity.ToTable("jogosultsag", "runbase");

            entity.HasIndex(e => e.FelhasznaloId, "jogosultsag_felhasznalo_fk");
            entity.HasIndex(e => e.VersenyId, "jogosultsag_verseny_fk");

            entity.Property(e => e.FelhasznaloId).HasColumnName("felhasznaloID");
            entity.Property(e => e.VersenyId).HasColumnName("versenyID");

            entity.HasOne(d => d.Felhasznalo)
                .WithMany()
                .HasForeignKey(d => d.FelhasznaloId)
                .HasConstraintName("jogosultsag$jogosultsag_felhasznalo_fk");

            entity.HasOne(d => d.Verseny)
                .WithMany()
                .HasForeignKey(d => d.VersenyId)
                .HasConstraintName("jogosultsag$jogosultsag_verseny_fk");
        });




        modelBuilder.Entity<Versenyek>(entity =>
        {
            entity.HasKey(e => e.VersenyId).HasName("PK_versenyek_versenyID");

            entity.ToTable("versenyek", "runbase");

            entity.Property(e => e.VersenyId)
                .ValueGeneratedNever()
                .HasColumnName("versenyID");

            entity.Property(e => e.Datum).HasColumnName("datum");

            entity.Property(e => e.Helyszin)
                .HasMaxLength(100)
                .HasColumnName("helyszin");

            entity.Property(e => e.Leiras).HasColumnName("leiras");

            entity.Property(e => e.MaxLetszam).HasColumnName("max_letszam");

            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");

            entity.Property(e => e.Kep)
                .HasColumnName("kep")
                .HasColumnType("VARBINARY(MAX)");
        });


        modelBuilder.Entity<Versenyindulas>(entity =>
        {
            entity.HasKey(e => new { e.VersenyzoId, e.VersenyId, e.Tav }).HasName("PK_versenyindulas_versenyzoID");

            entity.ToTable("versenyindulas", "runbase");

            entity.HasIndex(e => e.VersenyId, "versenyID");

            entity.Property(e => e.VersenyzoId).HasColumnName("versenyzoID");
            entity.Property(e => e.VersenyId).HasColumnName("versenyID");
            entity.Property(e => e.Tav).HasColumnName("tav");
            entity.Property(e => e.Erkezes).HasColumnName("erkezes");
            entity.Property(e => e.Indulas).HasColumnName("indulas");
            entity.Property(e => e.Rajtszam).HasColumnName("rajtszam");

            entity.HasOne(d => d.Verseny).WithMany(p => p.Versenyindulas)
                .HasForeignKey(d => d.VersenyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("versenyindulas$versenyindulas_ibfk_2");

            entity.HasOne(d => d.Versenyzo).WithMany(p => p.Versenyindulas)
                .HasForeignKey(d => d.VersenyzoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("versenyindulas$versenyindulas_ibfk_1");
        });

        modelBuilder.Entity<Versenytav>(entity =>
        {
            entity.HasKey(e => new { e.Tav, e.VersenyId }).HasName("PK_versenytav_tavID");

            entity.ToTable("versenytav", "runbase");

            entity.HasIndex(e => e.Tav, "tavID");

            entity.HasIndex(e => e.VersenyId, "versenyID");

            entity.Property(e => e.Tav).HasColumnName("tav");
            entity.Property(e => e.VersenyId).HasColumnName("versenyID");

            entity.HasOne(d => d.Verseny).WithMany(p => p.Versenytavs)
                .HasForeignKey(d => d.VersenyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("versenytav$versenytav_ibfk_2");
        });

        modelBuilder.Entity<Versenyzo>(entity =>
        {
            entity.HasKey(e => e.VersenyzoId).HasName("PK_versenyzo_versenyzoID");

            entity.ToTable("versenyzo", "runbase");

            entity.Property(e => e.VersenyzoId).HasColumnName("versenyzoID");
            entity.Property(e => e.Neme)
                .HasMaxLength(100)
                .HasColumnName("neme");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
            entity.Property(e => e.SzuletesiEv).HasColumnName("szuletesi_ev");
            entity.Property(e => e.TajSzam)
                .HasMaxLength(100)
                .HasColumnName("taj_szam");
        });

        modelBuilder.Entity<Forum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_forum_Id");
            entity.ToTable("forum", "runbase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FelhasznaloId).HasColumnName("felhasznaloId");
            entity.Property(e => e.Tartalom).HasColumnName("tartalom").IsRequired();
            entity.Property(e => e.Kep).HasColumnName("kep").HasColumnType("VARBINARY(MAX)");
            entity.Property(e => e.Datum)
                .HasColumnName("datum")
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Felhasznalo)
                .WithMany(p => p.ForumBejegyzesek)
                .HasForeignKey(d => d.FelhasznaloId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("forum$felhasznalo_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
