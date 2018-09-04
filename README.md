Entity Change Tracking using DbContext in .Net Core


To save change Log For A Table Entry we have:

1. Create data model

            public class ChangeLog
            {
               [Key]
               public int Id { get; set; }
               public string TableName { get; set; }
               public string ColumnName { get; set; }
               public string OldValue { get; set; }
               public string NewValue { get; set; }
               public DateTime DateChanged { get; set; }
            }
    
2. modyfiy DbContex
  2.1 Add table object 
  
            public virtual DbSet<ChangeLog> ChangeLogs { get; set; }    
  
  2.2 Overide SaveChanges

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
