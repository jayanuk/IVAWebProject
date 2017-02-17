using IVA.Common;
using IVA.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IVA.DbAccess
{
    public class AppDBContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageThread> MessageThreads { get; set; }
        public DbSet<QuotationTemplate> QuotationTemplates { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<ServiceProviderLocation> ServiceProviderLocations { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
        public DbSet<UserPasscode> UserPasscodes { get; set; }
        public DbSet<ServiceRequestHistory> ServiceRequestHistories { get; set; }
        public DbSet<RequestQuotation> RequestQuotations { get; set; }
        public DbSet<AgentServiceRequest> AgentServiceRequests { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public AppDBContext()
           : base(Constant.DEFAULT_DB_CONNECTION)
        {
            Database.SetInitializer<AppDBContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder ModelBuilder)
        {
            base.OnModelCreating(ModelBuilder);
        }
    }
}
