using Microsoft.EntityFrameworkCore;
using MyFirstMvcApp.Data.Models;


namespace MyFirstMvcApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions dbContextOptions)
            :base(dbContextOptions)
        {

        }

        DbSet<User> Users { get; set; }

        DbSet<Card> Cards { get; set; }

        DbSet<UserCard> UserCards { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=BattleCards;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>().HasKey(x => new { x.CardId, x.UserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
