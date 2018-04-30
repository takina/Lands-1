namespace Lands.Backend
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Helpers;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Esta línea de código, es la que se encarga de migrar automáticamente la base de datos,
            //Es decir, que podemos cambiarle los campos, agregarle campos o crear nuevas tablas etc. etc.
            //Sin que haya problema con la base de datos, que sean los modelos los que dominen la base de datos
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Models.LocalDataContext, 
                Migrations.Configuration>());

            this.CheckRolesAndSuperUser();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");
            UsersHelper.CheckSuperUser();
        }
    }
}
