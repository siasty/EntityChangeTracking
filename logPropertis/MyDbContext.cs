using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace logPropertis
{
    public class MyDbContext: DbContext
    {
      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID=postgres;Password=Th9S38vk;Server=localhost;Port=5432;Database=dbWebTest;Integrated Security=true;Pooling=true;");
        }

        public DbSet<Test> Test { get; set; }

    }
}
