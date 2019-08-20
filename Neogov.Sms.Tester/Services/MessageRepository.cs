using Neogov.Sms.Tester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neogov.Sms.Tester.Services
{
    public class MessageRepository : IMessageRepository
    {
        #region Fields

        private readonly MessageDbContext _db;

        #endregion

        #region Constructors

        public MessageRepository(MessageDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Message>> GetAllMessagesAsync(string filter, int pageIndex, int pageSize)
        {
            IQueryable<Message> items;

            if (String.IsNullOrWhiteSpace(filter))
                items = _db.Messages.OrderByDescending(x => x.CreatedUtc);
            else
                items = _db.Messages.Where(x => x.CreatedUtc.ToLocalTime().ToString().Contains(filter) || x.To.Contains(filter) || x.From.Contains(filter) || x.Body.Contains(filter))
                    .OrderByDescending(x => x.CreatedUtc);

            return await items.ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _db.Messages.FindAsync(id);
        }

        public async Task<Message> AddMessageAsync(Message item)
        {
            _db.Messages.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        #endregion
    }
}
