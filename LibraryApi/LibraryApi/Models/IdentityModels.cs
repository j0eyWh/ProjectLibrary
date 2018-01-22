using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace LibraryApi.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApiUser : IdentityUser
    {
        public int UserId { get; set; }

        public string Name { get; set; }


        public string Surname { get; set; }

        public string Idnp { get; set; }

        public byte[] UserPic { get; set; }

        public DateTime BirthDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApiUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("membership.ApiUsers");
            modelBuilder.Entity<ApiUser>().ToTable("membership.ApiUsers");
            modelBuilder.Entity<IdentityUserRole>().ToTable("membership.ApiUserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("membership.ApiUserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("membership.ApiUserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("membership.ApiRoles");
        }
    }
}