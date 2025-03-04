using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Context
{
    public class HelloGreetingAppContext:DbContext
    {
        public HelloGreetingAppContext(DbContextOptions<HelloGreetingAppContext> options):base(options) 
        {
            
        }
        public virtual DbSet<Entity.GreetingEntity> Greeting { get; set; }
    }
}
