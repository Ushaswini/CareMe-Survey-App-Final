using Homework05.DTOs;
using Homework05.Identity;
using Homework05.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Homework05.API_Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        // GET: api/Users

        public List<UserDTO> GetAllUsers()
        {
            string roleName = "User";
            var role = AppRoleManager.Roles.Single(r => r.Name == roleName);
            /*var users = from user in UserManager.Users
            where user.Roles.Any(r => r.RoleId == role.Id)
            select user;
            var usersInRoles = users.Include("StudyGroup");
            var usersDTO = usersInRoles
                                       .Select(u => new UserDTO
                                       {
                                           Id = u.Id,
                                           UserName = u.UserName,
                                           Email = u.Email,
                                           StudyGroupId = u.StudyGroupId,
                                           StudyGroupName = u.StudyGroup.StudyName
                                       }).ToList();

            return usersDTO;*/

            // Find the users in that role
            var roleUsers = UserManager.Users
                .Include(u => u.StudyGroup)
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))

                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    StudyGroupId = u.StudyGroupId,
                    StudyGroupName = u.StudyGroup.StudyName
                }).ToList();

            return roleUsers;
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDeviceId(UserInfoViewModel user)
        {
            var userToModify = db.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            if (userToModify == null)
            {
                return NotFound();
            }
            userToModify.DeviceId = user.DeviceId;
            db.Entry(userToModify).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}
