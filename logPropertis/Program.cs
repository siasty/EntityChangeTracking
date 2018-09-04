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
                var test = db.Test.Find(1);
                test.B = "test123g";
                db.Test.Attach(test);
            //    db.Entry(test).State = EntityState.Modified;
                DisplayTrackedEntities(db.ChangeTracker);
                db.SaveChanges();

                

                Console.WriteLine(db.Test.Find(1).B);

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
