using Neogov.Sms.Tester.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neogov.Sms.Tester.Services
{
    public interface IMessageRepository
    {
        Task<Message> GetMessageByIdAsync(int id);

        Task<IEnumerable<Message>> GetAllMessagesAsync(string filter, int pageIndex, int pageSize);

        Task<Message> AddMessageAsync(Message item);
    }
}
