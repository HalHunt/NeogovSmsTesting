using Neogov.Sms.Tester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neogov.Sms.Tester.Services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDbContext _db;

        public MessageRepository(MessageDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync(string filter, int pageIndex, int pageSize)
        {
            IQueryable<Message> items;

            if (String.IsNullOrWhiteSpace(filter))
                items = _db.Messages.OrderByDescending(x => x.CreatedUtc)
                    .Select(x => x.WithFormattedPhoneNumbers());
            else
                items = _db.Messages.Where(x => x.CreatedUtc.ToLocalTime().ToString().Contains(filter) || x.To.Contains(filter) || x.From.Contains(filter) || x.Body.Contains(filter))
                    .OrderByDescending(x => x.CreatedUtc)
                    .Select(x => x.WithFormattedPhoneNumbers());

            return await items.ToPaginatedList(pageIndex, pageSize);
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            var item = await _db.Messages.FindAsync(id);
            if (item == null)
                return null;
            else
                return item.WithFormattedPhoneNumbers();
        }

        public async Task<Message> AddMessageAsync(Message item)
        {
            _db.Messages.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
