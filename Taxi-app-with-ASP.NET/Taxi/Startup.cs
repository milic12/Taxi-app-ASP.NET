using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Taxi_04.Models;

[assembly: OwinStartupAttribute(typeof(Taxi_04.Startup))]
namespace Taxi_04
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            CreateRolesandUsers();
            ConfigureAuth(app);
        }

        // Ovdje ćemo kreirati osnovne korisničke role,
        // ali i administratora
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Prilikom prvog pokretanja, kreirat cemo Admin rolu te prvog admin korisnika
            if (!roleManager.RoleExists("Admin"))
            {
                //admin rola 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //superuser (admin)                 
                var user = new ApplicationUser();
                user.UserName = "vaseime";
                user.Email = "vaseime@vaseime.hr";
                string userPWD = "@vaseime12345";
                var chkUser = UserManager.Create(user, userPWD);

                //useru dodijeliti ulogu admina
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            //kreiranje nove role za vozača
            if (!roleManager.RoleExists("Driver"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Driver";
                roleManager.Create(role);
            }

            //kreiranje nove role za putnika
            if (!roleManager.RoleExists("Passenger"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Passenger";
                roleManager.Create(role);
            }
        }


    }


}
