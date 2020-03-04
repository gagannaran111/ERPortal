using ERPortal.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ERPortal.WebUI.Startup))]
namespace ERPortal.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "gagan@gmail.com";
                user.Email = "gagan@gmail.com";

                string userPWD = "Gn@123";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role     
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);



            }
            // creating Creating User role     
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }
            // creating Creating DG role     
            if (!roleManager.RoleExists("DG"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "DG";
                roleManager.Create(role);

            }
            // creating Creating ADG role     
            if (!roleManager.RoleExists("ADG"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "ADG";
                roleManager.Create(role);

            }
            // creating Creating Director role     
            if (!roleManager.RoleExists("Director"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Director";
                roleManager.Create(role);

            }
            // creating Creating Under Secretary role     
            if (!roleManager.RoleExists("Under Secretary"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Under Secretary";
                roleManager.Create(role);

            }
            // creating Creating Joint Secretary role     
            if (!roleManager.RoleExists("Joint Secretary"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Joint Secretary";
                roleManager.Create(role);

            }
            // creating Creating coordinator role     
            if (!roleManager.RoleExists("Coordinator"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Coordinator";
                roleManager.Create(role);

            }
            // creating Creating HoD role     
            if (!roleManager.RoleExists("HoD"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "HoD";
                roleManager.Create(role);

            }
            // creating Creating operator role     
            if (!roleManager.RoleExists("Operator"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Operator";
                roleManager.Create(role);

            }
            // creating Creating nodal role     
            if (!roleManager.RoleExists("nodal"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "nodal";
                roleManager.Create(role);

            }
            // creating Creating technical officer role     
            if (!roleManager.RoleExists("Technical Officer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Technical Officer";
                roleManager.Create(role);

            }
            // creating Creating state nodal officer role     
            if (!roleManager.RoleExists("State Nodal Officer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "State Nodal Officer";
                roleManager.Create(role);

            }
            // creating Creating state technical officer role     
            if (!roleManager.RoleExists("State Technical Officer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "State Technical Officer";
                roleManager.Create(role);

            }
            // creating Creating state district officer role     
            if (!roleManager.RoleExists("State District Officer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "State District Officer";
                roleManager.Create(role);

            }
            // creating Creating state district officer role     
            if (!roleManager.RoleExists("Consultant Enhanced Recovery"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Consultant Enhanced Recovery";
                roleManager.Create(role);

            }
        }
    }
}
