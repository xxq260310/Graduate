using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>
    {
        public FeedbackRepository(MarketDB context)
            : base(context)
        {
        }
    }
}
