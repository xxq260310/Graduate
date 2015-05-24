using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Authentication
{
    public class User
    {
        /// <summary>
        /// Gets or sets the Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the IsAdmin is true
        /// </summary>
        public  string Role { get; set; }
    }
}