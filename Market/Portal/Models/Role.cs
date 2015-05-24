using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Role
    {
        public Role()
        {
            this.UserProfiles = new HashSet<UserProfile>();
        }

        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}