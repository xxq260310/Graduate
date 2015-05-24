using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class UserProfileRepository : GenericRepository<UserProfile>
    {
        public UserProfileRepository(MarketDB context)
            :base(context)
        {
        }
    }
}
