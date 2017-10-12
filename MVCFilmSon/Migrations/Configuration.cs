namespace MVCFilmSon.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    internal sealed class Configuration : DbMigrationsConfiguration<MVCFilmSon.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MVCFilmSon.Models.ApplicationDbContext context)
        {
            context.Kategoriler.AddOrUpdate(x => x.KategoriID,
                new Kategori() { KategoriAdi = "Korku" },
                new Kategori() { KategoriAdi = "Gerilim" },
                new Kategori() { KategoriAdi = "Aksiyon" });

            //Admin
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var rstore = new RoleStore<IdentityRole>(context);
                var rmanager = new RoleManager<IdentityRole>(rstore);
                var role = new IdentityRole { Name = "Admin" };
                rmanager.Create(role);

                //var rstore = new RoleStore<IdentityRole>(context);
                //var rmanager = new RoleManager<IdentityRole>(rstore);
                //var role = new IdentityRole { Name = "Admin" };
                //rmanager.Create(role);
            }

            if(!context.Users.Any(x => x.Email == "m@m.com"))
            {
                //Kullanýcý

                var kstore = new UserStore<ApplicationUser>(context);
                var kmanager = new UserManager<ApplicationUser>(kstore);
                var user = new ApplicationUser { UserName = "m@m.com", Email = "m@m.com" };
                kmanager.Create(user, "123456");
                kmanager.AddToRole(user.Id, "Admin");

                //var kstore = new UserStore<ApplicationUser>(context);
                //var kmanager = new UserManager<ApplicationUser>(kstore);
                //var user = new ApplicationUser { UserName = "m@m.com", Email= "m@m.com" };
                //kmanager.Create(user, "123456");
                //kmanager.AddToRoleAsync(user.Id, "Admin");
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
