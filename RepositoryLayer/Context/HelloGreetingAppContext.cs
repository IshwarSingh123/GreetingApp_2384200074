using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class HelloGreetingAppContext:DbContext
    {
        public HelloGreetingAppContext(DbContextOptions<HelloGreetingAppContext> options):base(options) 
        {
            
        }
        public virtual DbSet<Entity.GreetingEntity> Greeting { get; set; }

        public virtual DbSet<Entity.UserEntity> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API for defining the One-to-Many relationship
            modelBuilder.Entity<GreetingEntity>()
                .HasOne(g => g.User)
                .WithMany(u => u.Greeting)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Delete greetings when user is deleted
        }
    }
}
