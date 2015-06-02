using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal
{
    public static class GetInfo
    {
        public static string GetNickNameByUserName(string userName)
        {
            using (MarketContext db = new MarketContext())
            {
                var nickName = db.UserProfiles.Where(x => x.UserName == userName).Select(x => x.Nickname).FirstOrDefault();
                return nickName;
            }

        }

        public static string GetRoleNameByUserName(string userName)
        {
            using (MarketContext db = new MarketContext())
            {
                var role = db.UserProfiles.Where(x => x.UserName == userName).Select(s => s.Role.RoleName).FirstOrDefault();
                return role;
            }
        }

        public static int GetUserIdByUserName(string userName)
        {
            using (MarketContext db = new MarketContext())
            {
                var userId = db.UserProfiles.Where(s => s.UserName == userName).Select(p => p.UserId).FirstOrDefault();
                return userId;
            }
        }
    }
}