namespace Market.DAL.UnitWork
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Market.DAL.Repositories;

    /// <summary>
    /// Implement the Unit of work pattern.
    /// This class holds a stack of repositories that share one same DB context.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        /// <summary>
        /// The database context object.
        /// </summary>
        private readonly MarketDB context = DbFactoryConfiguration.DbFactory.Create();

        /// <summary>
        /// A sign that tells if this object was disposed yet.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Holds an instance of <see cref="UserProfileRepository"/>
        /// </summary>
        private UserProfileRepository userProfileRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="RoleRepository"/>
        /// </summary>
        private RoleRepository roleRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="RequiredCommodityRepository"/>
        /// </summary>
        private RequiredCommodityRepository requiredCommodityRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="SiteFeedbackRepository"/>
        /// </summary>
        private SiteFeedbackRepository siteFeedbackRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="FeedbackRepository"/>
        /// </summary>
        private FeedbackRepository feedbackRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="OrderRepository"/>
        /// </summary>
        private OrderRepository orderRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="FavoriteRepository"/>
        /// </summary>
        private FavoriteRepository favoriteRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="ShoppingTrolleyRepository"/>
        /// </summary>
        private ShoppingTrolleyRepository shoppingTrolleyRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="AnnouncementRepository"/>
        /// </summary>
        private AnnouncementRepository announcementRepository = null;

        /// <summary>
        /// Holds an instance of <see cref="CommodityRepository"/>
        /// </summary>
        private CommodityRepository commodityRepository = null;

        /// <summary>
        /// Gets the instance of DB context for this unit of work.
        /// </summary>
        public MarketDB Context
        {
            get { return this.context; }
        }

        /// <summary>
        /// Gets an instance of UserProfileRepository.
        /// </summary>
        public UserProfileRepository UserProfileRepository
        {
            get
            {
                if (this.userProfileRepository == null)
                {
                    this.userProfileRepository = new UserProfileRepository(this.context);
                }

                return this.userProfileRepository;
            }
        }

        /// <summary>
        /// Gets an instance of RoleRepository.
        /// </summary>
        public RoleRepository RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new RoleRepository(this.context);
                }

                return this.roleRepository;
            }
        }

        /// <summary>
        /// Gets an instance of RequiredCommodityRepository.
        /// </summary>
        public RequiredCommodityRepository RequiredCommodityRepository
        {
            get
            {
                if (this.requiredCommodityRepository == null)
                {
                    this.requiredCommodityRepository = new RequiredCommodityRepository(this.context);
                }

                return this.requiredCommodityRepository;
            }
        }

        /// <summary>
        /// Gets an instance of SiteFeedbackRepository.
        /// </summary>
        public SiteFeedbackRepository SiteFeedbackRepository
        {
            get
            {
                if (this.siteFeedbackRepository == null)
                {
                    this.siteFeedbackRepository = new SiteFeedbackRepository(this.context);
                }

                return this.siteFeedbackRepository;
           }
        }

        /// <summary>
        /// Gets an instance of FeedbackRepository.
        /// </summary>
        public FeedbackRepository FeedbackRepository
        {
            get
            {
                if (this.feedbackRepository == null)
                {
                    this.feedbackRepository = new FeedbackRepository(this.context);
                }

                return this.feedbackRepository;
            }
        }

        /// <summary>
        /// Gets an instance of OrderRepository.
        /// </summary>
        public OrderRepository OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new OrderRepository(this.context);
                }

                return this.orderRepository;
            }
        }

        /// <summary>
        /// Gets an instance of FavoriteRepository.
        /// </summary>
        public FavoriteRepository FavoriteRepository
        {
            get
            {
                if (this.FavoriteRepository == null)
                {
                    this.favoriteRepository = new FavoriteRepository(this.context);
                }

                return this.favoriteRepository;
            }
        }

        /// <summary>
        /// Gets an instance of ShoppingTrolleyRepository.
        /// </summary>
        public ShoppingTrolleyRepository ShoppingTrolleyRepository
        {
            get
            {
                if (this.shoppingTrolleyRepository == null)
                {
                    this.shoppingTrolleyRepository = new ShoppingTrolleyRepository(this.context);
                }

                return this.shoppingTrolleyRepository;
            }
        }

        /// <summary>
        /// Gets an instance of AnnouncementRepository.
        /// </summary>
        public AnnouncementRepository AnnouncementRepository
        {
            get
            {
                if (this.announcementRepository  == null)
                {
                    this.announcementRepository = new AnnouncementRepository(this.context);
                }

                return this.announcementRepository;
            }
        }

        /// <summary>
        /// Gets an instance of CommodityRepository.
        /// </summary>
        public CommodityRepository CommodityRepository
        {
            get
            {
                if (this.commodityRepository == null)
                {
                    this.commodityRepository = new CommodityRepository(this.context);
                }

                return this.commodityRepository;
            }
        }

        /// <summary>
        ///  Gets an instance of <see cref="GenericRepository{TEntity}"/>
        /// </summary>
        /// <typeparam name="TEntity">Tells which type of data this repository is for.</typeparam>
        /// <returns>An instance of <see cref="GenericRepository{TEntity}"/>.</returns>
        public GenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(this.Context);
        }

        /// <summary>
        /// Save the changes in current database context.
        /// </summary>
        public void Save()
        {
            this.context.SaveChanges();
        }

        /// <summary>
        /// Implement the IDispose interface.
        /// </summary>
        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implement the IDispose interface.
        /// </summary>
        /// <param name="disposing">Tells if this call is from Dispose not from destructor.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
