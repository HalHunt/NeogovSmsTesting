using Microsoft.EntityFrameworkCore;
using Neogov.Sms.Tester.Models;

namespace Neogov.Sms.Tester.Services
{
    public class MessageDbContext : DbContext
    {
        #region Constructors

        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        #endregion

        #region Properties

        public DbSet<Message> Messages { get; set; }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        #endregion
    }
}
