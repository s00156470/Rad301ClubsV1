namespace Rad301ClubsV1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.ClubModel;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Rad301ClubsV1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Rad301ClubsV1.Models.ApplicationDbContext context)
        {
            var manager =
                 new UserManager<ApplicationUser>(
                     new UserStore<ApplicationUser>(context));

            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));



            roleManager.Create(new IdentityRole { Name = "Admin" });
            roleManager.Create(new IdentityRole { Name = "ClubAdmin" });
            roleManager.Create(new IdentityRole { Name = "Member" });

            context.Users.AddOrUpdate(u => u.Email, new ApplicationUser
            {
                StudentID = "S12345678",
                Email = "S12345678@mail.itsligo.ie",
                DateJoined = DateTime.Now,
                EmailConfirmed = true,
                UserName = "S12345678@mail.itsligo.ie",
                PasswordHash = new PasswordHasher().HashPassword("Ss1234567$1"),
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            context.Users.AddOrUpdate(u => u.Email, new ApplicationUser
            {
                StudentID = "ppowell",
                Email = "powell.paul@itsligo.ie",
                DateJoined = DateTime.Now,
                EmailConfirmed = true,
                UserName = "powell.paul@itsligo.ie",
                PasswordHash = new PasswordHasher().HashPassword("Ppowell$1"),
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            context.Users.AddOrUpdate(u => u.Email, new ApplicationUser
            {
                StudentID = "S00000001",
                Email = "S00000001@mail.itsligo.ie",
                DateJoined = DateTime.Now,
                EmailConfirmed = true,
                UserName = "S00000001@mail.itsligo.ie",
                PasswordHash = new PasswordHasher().HashPassword("SS00000001$1"),
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            ApplicationUser admin = manager.FindByEmail("powell.paul@itsligo.ie");
            if (admin != null)
            {
                manager.AddToRoles(admin.Id, new string[] { "Admin", "Member", "ClubAdmin" });
            }
           /* else {
                throw new Exception { Source = "Did not find user" };
            }*/

            ApplicationUser member = manager.FindByEmail("S12345678@mail.itsligo.ie");
            if (member != null)
            {
                manager.AddToRoles(member.Id, new string[] { "Member"});
            }

            ApplicationUser clubAdmin = manager.FindByEmail("S00000001@mail.itsligo.ie");
            if (manager.FindByEmail("S00000001@mail.itsligo.ie") != null)
            {
                manager.AddToRoles(clubAdmin.Id, new string[] { "ClubAdmin" });
            }
            //seedStudents(context);

        }
        public void seedStudents(ApplicationDbContext current)
        {

            List<Student> selectedStudents = new List<Student>();

            using (ClubContext ctx = new ClubContext())
            {
                var randomStudentSet = ctx.Students
                    .Select(s => new { s.StudentID, r = Guid.NewGuid() });

                List<string> subset = randomStudentSet.OrderBy(s => s.r).Select(s => s.StudentID).Take(10).ToList();

                foreach (string s in subset)
                {
                    selectedStudents.Add(
                        ctx.Students.First(st => st.StudentID == s)
                        );
                }

                Club chosen = ctx.Clubs.First();

                foreach (Student s in selectedStudents)
                {
                    ctx.members.AddOrUpdate(m => m.StudentID,
                        new Member
                        {
                            ClubId = chosen.ClubId,
                            StudentID = s.StudentID
                        });
                }
                ctx.SaveChanges();
            }

            // Add application users 
            foreach (Student s in selectedStudents)
            {
                current.Users.AddOrUpdate(u => u.StudentID,
                    new ApplicationUser
                    {
                        StudentID = s.StudentID,
                        UserName = s.StudentID + "@mail.itsligo.ie",
                        Email = s.StudentID + "@mail.itsligo.ie",
                        EmailConfirmed = true,
                        DateJoined = DateTime.Now,
                        PasswordHash = new PasswordHasher().HashPassword(s.StudentID + "$1"),
                        SecurityStamp = Guid.NewGuid().ToString(),
                    });
            }
            current.SaveChanges();
        }
    }
}
