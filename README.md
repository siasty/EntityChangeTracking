Entity Change Tracking using DbContext in .Net Core


To save change Log For A Table Entry we have:

1. Create data model

            public class ChangeLog
            {
              [Key]
              public int Id { get; set; }
              public string TableName { get; set; }
              public string ColumnName { get; set; }
              public string RowId { get; set; }
              public string State { get; set; }
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
