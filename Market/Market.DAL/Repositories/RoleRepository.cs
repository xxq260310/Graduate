using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
