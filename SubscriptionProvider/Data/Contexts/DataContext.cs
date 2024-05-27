using Microsoft.EntityFrameworkCore;
using SubscriptionProvider.Data.Entities;


namespace SubscriptionProvider.Data.Contexts;

    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<SubscribeEntity> Subscribers { get; set; }
    }


