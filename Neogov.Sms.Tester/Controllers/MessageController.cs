using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neogov.Sms.Tester.Models;
using Neogov.Sms.Tester.Services;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Neogov.Sms.Tester.Controllers
{
    public class MessageController : BaseController
    {
        #region Fields

        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<MessageHub> _messageHubContext;

        #endregion

        #region Constructors

        public MessageController(AppConfiguration configuration, IMessageRepository messageRepository, IHubContext<MessageHub> messageHubContext) : base(configuration)
        {

            if (String.IsNullOrWhiteSpace(configuration.MessageToNumber))
                throw new InvalidOperationException("The message 'To' number is missing.");

            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _messageHubContext = messageHubContext ?? throw new ArgumentNullException(nameof(messageHubContext));
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<ActionResult> GetAllMessages([FromQuery] string filter, [FromQuery] int page = 1, [FromQuery] int size = 0)
        {
            return Ok(await _messageRepository.GetAllMessagesAsync(filter, page, size <= 0 ? AppConfiguration.MessagesPageSize : size));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessgeById(int id)
        {
            var item = await _messageRepository.GetMessageByIdAsync(id);
            if (item != null)
                return Ok(item);
            else
                return NotFound(id);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidateTwilioRequestAttribute))]
        [ProducesResponseType(StatusCodes.Status403Forbidden), ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [Consumes("application/x-www-form-urlencoded"), Produces("application/xml")]
        public async Task<ActionResult> AddMessage([FromForm, Required] string body, [FromForm, Required] string from)
        {
            try
            {
                var item = await _messageRepository.AddMessageAsync(new Models.Message { From = from, Body = body, To = AppConfiguration.MessageToNumber });
                await _messageHubContext.Clients.All.SendAsync("SmsReceived", item.WithFormattedPhoneNumbers());
                return Ok();
                // TODO: find out why there is no XML formatter for CreatedAtActionResult().
                //return CreatedAtAction(nameof(GetMessgeById), item.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion
    }
}
