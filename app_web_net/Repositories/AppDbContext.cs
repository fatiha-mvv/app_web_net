using app_web_net.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app_web_net.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets pour faire correspondance entere models et Tables BD
        public DbSet<Article> StockProduits { get; set; }
        public DbSet<Order> Ventes { get; set; }
        public DbSet<OrderItem> DetailsCommande { get; set; }


        // Méthode system ... protégée héritée de la classe DbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // appliquer les configurations des entités
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new VenteConfiguration());
            modelBuilder.ApplyConfiguration(new DetailCommandeConfiguration());
        }
    }

    // configuration pour l'entité Article
    internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(a => a.Tarif)
                .HasPrecision(18, 2);

            builder.Property(a => a.Libelle)
                .HasMaxLength(150);

            builder.Property(a => a.Categorie)
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(1000);
        }
    }

    // configuration pour l'entité Order
    internal class VenteConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasKey(v => v.CommandeId); // <-- keyyyyy

            builder.Property(v => v.Montant)
                .HasPrecision(18, 2);

            builder.Property(v => v.AdresseExpedition)
                .HasMaxLength(200);

            builder.Property(v => v.Statut)
                .HasMaxLength(50)
                .HasDefaultValue("En traitement");

            builder.HasMany(v => v.ElementsCommande)
                .WithOne()
                .HasForeignKey(dc => dc.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(v => v.Statut);

            builder.HasIndex(v => v.DateEnregistrement);
        }
    }

    // configuration pour l'entité OrderItem
    internal class DetailCommandeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.HasKey(dc => dc.ElementId); //keyyyyyy

            builder.Property(dc => dc.CoutUnitaire)
                .HasPrecision(18, 2);

            builder.Property(dc => dc.LibelleProduit)
                .HasMaxLength(150);

            builder.HasOne<Order>()
                .WithMany(v => v.ElementsCommande)
                .HasForeignKey(dc => dc.CommandeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Article>()
                .WithMany()
                .HasForeignKey(dc => dc.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}