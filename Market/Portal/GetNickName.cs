using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal
{
    public static class GetNickName
    {
        public static string GetNickNameByUserName(string userName)
        {
            MarketContext db = new MarketContext();
            var nickName = db.UserProfiles.Where(x => x.UserName == userName).Select(x => x.Nickname).FirstOrDefault();
            return nickName;
        }
    }
}