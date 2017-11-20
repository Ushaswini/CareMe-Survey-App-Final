﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Homework05.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string StudyGroupId { get; set; }
        public string Gender { get; set; }   
        
        public string DeviceId { get; set; }

        //Navigation Properties
        public StudyGroup StudyGroup { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("AzureDatabaseConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Homework05.Models.Survey> Surveys { get; set; }

        public System.Data.Entity.DbSet<Homework05.Models.StudyGroup> StudyGroups { get; set; }

        public System.Data.Entity.DbSet<Homework05.Models.SurveyResponse> SurveyResponses { get; set; }

        public System.Data.Entity.DbSet<Homework05.Models.Question> Questions { get; set; }
    }
}