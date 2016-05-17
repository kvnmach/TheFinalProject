using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TheFinalProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Photo { get; set; }
        public string Phone { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public virtual ICollection<Tool> MyTools { get; set; } = new List<Tool>();
        public virtual ICollection<Tool> Workbench { get; set; } = new List<Tool>();

    

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class DbContext : IdentityDbContext<ApplicationUser>
    {
        public DbContext()
            : base("DefaultConnection", false)
        {
        }

        public DbSet<Tool> Tools { get; set; }

        public static DbContext Create()
        {
            return new DbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tool>().HasOptional(t => t.Owner).WithMany(o => o.MyTools);
            modelBuilder.Entity<Tool>().HasMany(t => t.OnPeoplesWorkBench).WithMany(o => o.Workbench).Map(m =>
            {
                m.ToTable("ToolsOnWorkBench");
                m.MapLeftKey("User_Id");
                m.MapLeftKey("Tool_Id");
            });
        }
    }
}