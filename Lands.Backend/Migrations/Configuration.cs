namespace Lands.Backend.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //Esta clase se genera, al habilitar las migraciones autom�ticas desde el power shell o console
    internal sealed class Configuration : DbMigrationsConfiguration<Lands.Backend.Models.LocalDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            //Esta l�nea habilita la migraci�n , as� se pierdan datos
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Lands.Backend.Models.LocalDataContext";
        }

        protected override void Seed(Lands.Backend.Models.LocalDataContext context)
        {
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
