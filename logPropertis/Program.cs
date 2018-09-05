using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace logPropertis
{
    class Program
    {
        static void Main(string[] args)
        {
            using( var db = new MyDbContext())
            {
               
                // add value if not exist
                if (!db.Test.Any())
                {
                    var add = new Test();
                        add.A = "test1";
                        add.B = "test1";
                    db.Test.Add(add);
                      DisplayTrackedEntities(db.ChangeTracker); // Display tracking
                    db.SaveChanges();
                }

                // find and modify
                var test = db.Test.FirstOrDefault();
                test.B = "test123g";
                db.Test.Attach(test);
                  DisplayTrackedEntities(db.ChangeTracker);
                db.SaveChanges();

                // find and delete
                test = db.Test.FirstOrDefault();
                db.Test.Remove(test);
                  DisplayTrackedEntities(db.ChangeTracker);
                db.SaveChanges();



                // View Logs 
                foreach (var log in db.ChangeLogs.ToList())
                {
                    Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", log.Id, log.TableName,log.ColumnName,log.RowId, log.State, log.OldValue, log.NewValue, log.DateChanged);
                }
        

            }
            Console.ReadKey();
        }

        private static void DisplayTrackedEntities(ChangeTracker changeTracker)
        {
            Console.WriteLine("");

            var entries = changeTracker.Entries();
            foreach (var entry in entries)
            {
                Console.WriteLine("Table Name: {0}", entry.Entity.GetType().Name);
                Console.WriteLine("Status: {0}", entry.State);
                foreach (var prop in entry.OriginalValues.Properties)
                {
                    var originalValue = entry.OriginalValues[prop].ToString();
                    var currentValue =  entry.CurrentValues[prop].ToString();
                    if (originalValue != currentValue)
                    {
                        Console.WriteLine("Orginal: Column Name {0} value {1} ", prop.Name, entry.OriginalValues[prop].ToString());
                        Console.WriteLine("Current: Column Name {0} value {1}", prop.Name, entry.CurrentValues[prop].ToString());
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------");
        }
    }
    
}
