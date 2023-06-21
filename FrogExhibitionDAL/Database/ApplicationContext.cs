using FrogExhibitionDAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
    }
}
