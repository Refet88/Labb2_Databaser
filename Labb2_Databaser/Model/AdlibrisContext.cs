using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Labb2_Databaser.Model;

public partial class AdlibrisContext : DbContext
{
    public AdlibrisContext()
    {
    }

    public AdlibrisContext(DbContextOptions<AdlibrisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Butiker> Butiker { get; set; }

    public virtual DbSet<Böcker> Böcker { get; set; }

    public virtual DbSet<Författare> Författare { get; set; }

    public virtual DbSet<Förlag> Förlag { get; set; }

    public virtual DbSet<FörsäljningPerKund> FörsäljningPerKund { get; set; }

    public virtual DbSet<Kunder> Kunder { get; set; }

    public virtual DbSet<LagerSaldo> LagerSaldo { get; set; }

    public virtual DbSet<OrderDetaljer> OrderDetaljer { get; set; }

    public virtual DbSet<Ordrar> Ordrar { get; set; }

    public virtual DbSet<TitlarPerFörfattare> TitlarPerFörfattare { get; set; }

    public virtual DbSet<FörfattareBöcker> FörfattareBöcker {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<AdlibrisContext>().Build();
        var connectionString = config["ConnectionString"];
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Butiker>(entity =>
        {
            entity.HasKey(e => e.ButikId).HasName("PK__Butiker__B5D66BFA94885CF2");

            entity.ToTable("Butiker");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Adress).HasMaxLength(255);
            entity.Property(e => e.Butiksnamn).HasMaxLength(100);
            entity.Property(e => e.Ort).HasMaxLength(100);
            entity.Property(e => e.PostNummer).HasMaxLength(6);
        });

        modelBuilder.Entity<Böcker>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Böcker__3BF79E034CB8E506");

            entity.ToTable("Böcker");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN13");
            entity.Property(e => e.AntalSidor).HasColumnName("Antal_sidor");
            entity.Property(e => e.FörlagId).HasColumnName("FörlagID");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Språk).HasMaxLength(50);
            entity.Property(e => e.Titel).HasMaxLength(255);

            entity.HasOne(d => d.Förlag).WithMany(p => p.Böcker)
                .HasForeignKey(d => d.FörlagId)
                .HasConstraintName("FK__Böcker__FörlagID__4AB81AF0");
        });

        modelBuilder.Entity<Författare>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Författa__3214EC27ED9A1422");

            entity.ToTable("Författare");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Efternamn).HasMaxLength(50);
            entity.Property(e => e.Förnamn).HasMaxLength(50);

            
        });

        modelBuilder.Entity<Förlag>(entity =>
        {
            entity.HasKey(e => e.FörlagId).HasName("PK__Förlag__DE6A852C9B0007C1");

            entity.ToTable("Förlag");

            entity.Property(e => e.FörlagId).HasColumnName("FörlagID");
            entity.Property(e => e.Adress).HasMaxLength(255);
            entity.Property(e => e.Namn).HasMaxLength(100);
            entity.Property(e => e.Ort).HasMaxLength(100);
            entity.Property(e => e.PostNummer).HasMaxLength(6);
            entity.Property(e => e.TelefonNummer)
                .HasMaxLength(20)
                .HasColumnName("Telefon_Nummer");
        });

        modelBuilder.Entity<FörsäljningPerKund>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("FörsäljningPerKund");

            entity.Property(e => e.KundId).HasColumnName("KundID");
            entity.Property(e => e.KundNamn).HasMaxLength(101);
            entity.Property(e => e.TotalFörsäljning).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<Kunder>(entity =>
        {
            entity.HasKey(e => e.KundId).HasName("PK__Kunder__F2B5DEAC676B8B49");

            entity.ToTable("Kunder");

            entity.Property(e => e.KundId).HasColumnName("KundID");
            entity.Property(e => e.Adress).HasMaxLength(255);
            entity.Property(e => e.Efternamn).HasMaxLength(50);
            entity.Property(e => e.Epost).HasMaxLength(100);
            entity.Property(e => e.Förnamn).HasMaxLength(50);
            entity.Property(e => e.Ort).HasMaxLength(100);
            entity.Property(e => e.PostNummer).HasMaxLength(6);
            entity.Property(e => e.TelefonNummer)
                .HasMaxLength(20)
                .HasColumnName("Telefon_Nummer");
        });

        modelBuilder.Entity<LagerSaldo>(entity =>
        {
            entity.HasKey(e => new { e.ButikId, e.Isbn }).HasName("PK__LagerSal__1191B894FD876E18");

            entity.ToTable("LagerSaldo");

            entity.Property(e => e.ButikId).HasColumnName("ButikID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");

            entity.HasOne(d => d.Butik).WithMany(p => p.LagerSaldo)
                .HasForeignKey(d => d.ButikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LagerSald__Butik__3E52440B");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.LagerSaldo)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LagerSaldo__ISBN__3F466844");
        });

        modelBuilder.Entity<OrderDetaljer>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.Isbn }).HasName("PK__OrderDet__67D788C1FDBA446D");

            entity.ToTable("OrderDetaljer");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISBN");
            entity.Property(e => e.Pris).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IsbnNavigation).WithMany(p => p.OrderDetaljer)
                .HasForeignKey(d => d.Isbn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDetal__ISBN__47DBAE45");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetaljer)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__46E78A0C");
        });

        modelBuilder.Entity<Ordrar>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Ordrar__C3905BAFF02E51B6");

            entity.ToTable("Ordrar");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.KundId).HasColumnName("KundID");

            entity.HasOne(d => d.Kund).WithMany(p => p.Ordrar)
                .HasForeignKey(d => d.KundId)
                .HasConstraintName("FK__Ordrar__KundID__440B1D61");
        });

        modelBuilder.Entity<TitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TitlarPerFörfattare");

            entity.Property(e => e.Lagervärde).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Namn).HasMaxLength(101);
        });

        modelBuilder.Entity<FörfattareBöcker>(entity =>
        {
            entity.ToTable("FörfattareBöcker");

            entity.HasKey(e => new { e.FörfattareId, e.Isbn });

            entity.HasOne(fb => fb.Författare)
                .WithMany(f => f.FörfattareBöcker)
                .HasForeignKey(fb => fb.FörfattareId);

            entity.HasOne(fb => fb.Bok)
                .WithMany(b => b.FörfattareBöcker)
                .HasForeignKey(fb => fb.Isbn)
                .HasPrincipalKey(b => b.Isbn13);

            entity.Property(e => e.FörfattareId).HasColumnName("FörfattareId");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .IsUnicode(false)
                .IsFixedLength();
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
