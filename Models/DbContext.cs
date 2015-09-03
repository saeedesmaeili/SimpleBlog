using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace Blog.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbContextInitializer());
        }



        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<>
            modelBuilder.Entity<BlogPost>()
                .HasMany(e => e.Tags)
                .WithMany(m => m.BlogPosts)
                .Map(m =>
                {
                    m.MapLeftKey("BlogPostId");
                    m.MapRightKey("TagId");
                    m.ToTable("BlogPostTags");
                });

            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationDbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {

            ApplicationDbContext db = new ApplicationDbContext();

            db.Categories.Add(new Category() { Name = "شخصی", ParentId = 0 });
            db.Categories.Add(new Category() { Name = "Sql Server", ParentId = 0 });


            IList<IdentityRole> defaultRoles = new List<IdentityRole>();
            context.Roles.Add(new IdentityRole() { Name = "Admin" });
            context.Roles.Add(new IdentityRole() { Name = "Author" });


            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var user = new ApplicationUser {
                UserName = "smaeily@gmail.com" ,
                FirstName ="سعید"   ,
                LastName = "اسماعیلی" ,
                Email = "smaeily@gmail.com" ,
                PhoneNumber ="+989125237880"
            };

            manager.Create(user, "adminadmin");
            manager.AddToRole(user.Id, "Admin");




            db.SaveChanges();

            base.Seed(context);
        }
    }
}