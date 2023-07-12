using FrogExhibitionDAL.Models;
using FrogExhibitionDAL.Models.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FrogExhibitionDAL.Database
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Frog> Frogs { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<FrogOnExhibition> FrogsOnExhibitions { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            
        }

        public override int SaveChanges()
        {
            //HandleEntityDelete();
            ProcessEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //HandleEntityDelete();
            ProcessEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Exhibition>().HasQueryFilter(m => m.DeletedAt == null);
            //modelBuilder.Entity<Frog>().HasQueryFilter(m => m.DeletedAt == null);
            //modelBuilder.Entity<Vote>().HasQueryFilter(m => m.DeletedAt == null);
            //modelBuilder.Entity<ApplicationUser>().HasQueryFilter(m => m.DeletedAt == null);
            //modelBuilder.Entity<FrogOnExhibition>().HasQueryFilter(m => m.DeletedAt == null);

            //many to many with join table
            modelBuilder.Entity<Frog>()
                .HasMany(e => e.Exhibitions)
                .WithMany(e => e.Frogs)
                .UsingEntity<FrogOnExhibition>();

            //No duplicate frogs on exebiton
            modelBuilder.Entity<FrogOnExhibition>()
                .HasIndex(e => new { e.FrogId, e.ExhibitionId }, "UniqueFrogId_ExhibitionId").IsUnique(true);

            //User can vote for certain frog on exebition only once
            modelBuilder.Entity<Vote>()
                .HasIndex(e => new { e.ApplicationUserId, e.FrogOnExhibitionId }, "UniqueApplicationUserId_FrogOnExhibitionId").IsUnique(true);
        }

        private void HandleEntityDelete()
        {
            var entities = ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Deleted);
            foreach (var entity in entities)
            {
                if (entity.Entity is BaseModel)
                {
                    entity.State = EntityState.Modified;
                    var book = entity.Entity as BaseModel;
                    book.DeletedAt = DateTime.Now;
                }
            }
        }

        private void HandleDependent(EntityEntry entry)
        {
            entry.CurrentValues["DeletedAt"] = DateTime.Now;
        }

        private void ProcessEntities()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                foreach (var navigationEntry in entry.Navigations
                    .Where(n => !((IReadOnlyNavigation)n.Metadata).IsOnDependent))
                {
                    if (navigationEntry is CollectionEntry collectionEntry)
                    {
                        foreach (var dependentEntry in collectionEntry.CurrentValue)
                        {
                            HandleDependent(Entry(dependentEntry));
                        }
                    }
                    else
                    {
                        var dependentEntry = navigationEntry.CurrentValue;
                        if (dependentEntry != null)
                        {
                            HandleDependent(Entry(dependentEntry));
                        }
                    }
                }
            }
        }
    }
}
