using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
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
            var modifiedEntities = ChangeTracker.Entries().ToList();
            var now = DateTime.Now;
            string rowId = null;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var entityState = change.State;

                foreach (var prop in change.OriginalValues.Properties.Cast<IProperty>().Select((r, i) => new { Name = r.Name, Index = i }))
                {
                    rowId = change.OriginalValues["Id"].ToString();

                    if (entityState == EntityState.Modified)
                    {
                        var originalValue = change.OriginalValues[prop.Name].ToString();
                        var currentValue = change.CurrentValues[prop.Name].ToString();
                        if (originalValue != currentValue)
                        {
                            ChangeLog log = new ChangeLog()
                            {
                                TableName = entityName,
                                ColumnName = prop.Name,
                                RowId = rowId,
                                State = entityState.ToString(),
                                OldValue = originalValue,
                                NewValue = currentValue,
                                DateChanged = now
                            };
                            ChangeLogs.Add(log);
                        } 
                    }
                    else if (entityState == EntityState.Deleted)
                    {
                        if(prop.Index == 1)
                        {
                         
                            ChangeLog log = new ChangeLog()
                            {

                                TableName = entityName,
                                ColumnName = "",
                                RowId = rowId,
                                State = entityState.ToString(),
                                OldValue = "",
                                NewValue = "",
                                DateChanged = now
                            };
                            ChangeLogs.Add(log);
                        }
                    }
                }
            }
            return base.SaveChanges();
        }




    }
}
