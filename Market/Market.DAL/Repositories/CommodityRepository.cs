using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class CommodityRepository : GenericRepository<Commodity>
    {
        public CommodityRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
