
namespace Portal.Authentication
{
    using Portal.Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// This class is to manage admin or member log in.
    /// </summary>
    public class AdminManager
    {
        private List<User> users = new List<User>();

        /// <summary>
        /// This method is to validate the username and password.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="password">Pass word</param>
        /// <returns>
        /// true
        /// false
        /// </returns>
        public AdminManager()
        {
            using (MarketContext db = new MarketContext())
            {
                var item = (from userProfile in db.UserProfiles
                            select userProfile).ToList();
                foreach (var user in item)
                {
                    if (user.Role.RoleName == null || user.Role.RoleName == "Member")
                    {
                        users.Add(new User() { Username = user.UserName, Password = Encryption.RSADecrypt(user.Password), Role = "Member" });
                    }

                    else if (user.Role.RoleName == "Administrator")
                    {
                        users.Add(new User() { Username = user.UserName, Password = Encryption.RSADecrypt(user.Password), Role = "Administrator" });
                    }

                    else if (user.Role.RoleName == "TemporaryAdmin")
                    {
                        users.Add(new User() { Username = user.UserName, Password = Encryption.RSADecrypt(user.Password), Role = "TemporaryAdmin" });
                    }

                }
            }
        }

        public bool Validate(string userName, string password)
        {
            foreach (var user in users)
            {
                if (user.Username == userName && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method check whether the user is admin
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <returns>
        /// true
        /// false
        /// </returns>
        public bool IsAdmin(string userName)
        {
            foreach (var user in users)
            {
                if (user.Username == userName && user.Role == "Administrator")
                {
                    return true;
                }

                if (user.Username == userName && user.Role == "TemporaryAdmin")
                {
                    return true;
                }
            }

            return false;
        }
    }
}