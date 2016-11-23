namespace Rad301ClubsV1.Migrations.ClubModelMigrations
{
    using CsvHelper;
    using Models.ClubModel;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Rad301ClubsV1.Models.ClubModel.ClubContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ClubModelMigrations";
        }

        protected override void Seed(Rad301ClubsV1.Models.ClubModel.ClubContext context)
        {
            context.Clubs.AddOrUpdate(c => c.ClubName,
                new Club { ClubName = "The Tiddly Winks Club", CreationDate = DateTime.Now,
                clubEvents = new List<ClubEvent>()
                    {
                        new ClubEvent {StartDateTime = DateTime.Now.Subtract(new TimeSpan(5,0,0,0,0)),
                                       EndDateTime = DateTime.Now.Subtract(new TimeSpan(5,0,0,0,0)),
                                       Location = "Sligo", Venue="Arena"
                        },
                        new ClubEvent {StartDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0,0)),
                                       EndDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0,0)),
                                       Location = "Sligo", Venue="Main Canteen"
                        },
                    }// End List
                });
            context.Clubs.AddOrUpdate(c => c.ClubName,
                new Club { ClubName = "The Chess Club", CreationDate = DateTime.Now,
                clubEvents = new List<ClubEvent>()
                {
                  new ClubEvent
                    {
                        Location= "Sligo",
                        Venue="Arena",
                        StartDateTime=DateTime.Now.Add(new TimeSpan(5,0,0,0,0)),
                        EndDateTime = DateTime.Now.Add(new TimeSpan(5,0,0,0,0)),
                    },
                  new ClubEvent
                  {
                      Location = "Sligo",
                      Venue = "Main Canteen",
                      StartDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0,0)),
                      EndDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0,0)),

                      /*StartDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0,0)),
                                       EndDateTime = DateTime.Now.Subtract(new TimeSpan(3,0,0,0,0)),
                                       Location = "Sligo", Venue="Main Canteen"*/
                  }
                }
                });

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Rad301ClubsV1.Migrations.ClubModelMigrations.TestStudents.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    var testStudents = csvReader.GetRecords<Student>().ToArray();
                    context.Students.AddOrUpdate(s => s.StudentID, testStudents);
                }
            }
            //try
            //{
            //    StudentData testStudents = new StudentData();
            //}
            //catch(Exception e) {
            //    throw new Exception { Source = e.Message };
            //}
        }
    }
}
