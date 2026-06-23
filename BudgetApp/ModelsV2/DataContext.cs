using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.ModelsV2;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
    //chaque DbSet = une table complète accessible en C#
    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<LigneAchat> LigneAchats { get; set; }

    public virtual DbSet<Magasin> Magasins { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.IdArticle).HasName("PK__Article__2CC641E44A6A75C6");

            entity.ToTable("Article");

            entity.HasIndex(e => e.NomArticleNormalized, "UX_Article_NomArticleNormalized").IsUnique();

            entity.Property(e => e.NomArticle).HasMaxLength(200);
            entity.Property(e => e.NomArticleNormalized)
                .HasMaxLength(200)
                .HasComputedColumnSql("(lower(ltrim(rtrim([NomArticle]))))", true);
            entity.Property(e => e.Unite)
                .HasMaxLength(10)
                .HasDefaultValue("piece");

            entity.HasOne(d => d.IdCategorieNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.IdCategorie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Article_fk_Categorie");
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.IdCategorie).HasName("PK__Categori__A3C02A1C0B912EA6");

            entity.ToTable("Categorie");

            entity.Property(e => e.NomCategorie).HasMaxLength(100);
        });

        modelBuilder.Entity<LigneAchat>(entity =>
        {
            entity.HasKey(e => e.IdAchatLigne).HasName("PK__LigneAch__E2C86BEDEA9ADB69");

            entity.ToTable("LigneAchat");

            entity.Property(e => e.PrixUnitaire).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Quantite)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(8, 3)");
            entity.Property(e => e.Rabais).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.IdArticleNavigation).WithMany(p => p.LigneAchats)
                .HasForeignKey(d => d.IdArticle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LigneAchat_fk_Article");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.LigneAchats)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LigneAchat_fk_Ticket");
        });

        modelBuilder.Entity<Magasin>(entity =>
        {
            entity.HasKey(e => e.IdMagasin).HasName("PK__Magasin__00E84C951928DE9C");

            entity.ToTable("Magasin");

            entity.Property(e => e.Localite).HasMaxLength(100);
            entity.Property(e => e.NomMagasin).HasMaxLength(100);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket).HasName("PK__Ticket__4B93C7E7BA2E06AA");

            entity.ToTable("Ticket");

            entity.HasOne(d => d.IdMagasinNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.IdMagasin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ticket_fk_Magasin");

            entity.Ignore(e => e.Lignes);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
