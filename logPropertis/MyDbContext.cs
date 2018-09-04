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
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }

        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified).ToList();
            var now = DateTime.Now;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;

                foreach (var prop in change.OriginalValues.Properties)
                {
                    var originalValue = change.OriginalValues[prop].ToString();
                    var currentValue  = change.CurrentValues[prop].ToString();
                    if (originalValue != currentValue)
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            TableName = entityName,
                            ColumnName = prop.Name,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now
                        };
                        ChangeLogs.Add(log);
                    }
                }
            }
            return base.SaveChanges();
        }




    }
}
