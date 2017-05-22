using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using YaraTask.Data;
using System.Collections.Generic;
using Warehouse.Data;

namespace Warehouse.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Silos = new HashSet<Silo>();
            this.Tanks = new HashSet<Tank>();
        }

        public virtual ICollection<Silo> Silos { get; set; }

        public virtual ICollection<Tank> Tanks { get; set; }

        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

}