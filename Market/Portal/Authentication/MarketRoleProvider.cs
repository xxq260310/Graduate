using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Portal.Authentication
{
    public class MarketRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            if (username == null || username == "")
            {
                throw new ProviderException("User Name cannot be empty or null");
            }

            if (roleName == null || roleName == "")
            {
                throw new ProviderException("Role Name cannot be empty or null");
            }

            bool userIsInRole = false;
            AdminManager adminManager = new AdminManager();
            if (adminManager.IsAdmin(username) && roleName == "Administrator")
            {
                userIsInRole = true;
            }

            if (adminManager.IsAdmin(username) && roleName == "TemporaryAdmin")
            {
                userIsInRole = true;
            }

            return userIsInRole;
        }

        public override string[] GetRolesForUser(string username)
        {
            AdminManager adminManager = new AdminManager();
            if (adminManager.IsAdmin(username))
            {
                return "Administrator,TemporaryAdmin".Split(',');
            }

            return "Member".Split(',');
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return true;
        }
    }
}