using Microsoft.EntityFrameworkCore;
using Domain.Entities.Items;
using Domain.Entities.Lists;
using Domain.Entities.Users;

namespace Persistance.Common
{
    public class ToDoListContext : DbContext
    {
        public ToDoListContext(DbContextOptions<ToDoListContext> options) : 
            base(options)
        {
            Database.Migrate();
        }
       public DbSet<User> Users { get; set; }
       public DbSet<Item> Items { get; set; }
       public DbSet<List> Lists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany<List>("Lists")
                .WithOne("User")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<List>()
                .HasMany<Item>("Items")
                .WithOne("List")
                .HasForeignKey("ListId")
                .OnDelete(DeleteBehavior.Cascade);


           
                
               
                
        }
    }
}
