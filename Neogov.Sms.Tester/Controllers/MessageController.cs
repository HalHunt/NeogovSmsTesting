using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neogov.Sms.Tester.Models;
using Neogov.Sms.Tester.Services;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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
        public async Task<ActionResult> ReceiveMessage([FromForm, Required] string body, [FromForm, Required] string from)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(AppConfiguration.MessageToNumber))
                    throw new InvalidOperationException("The message 'To' number is missing.");

                var item = await _messageRepository.AddMessageAsync(new Models.Message { From = from, Body = body, To = AppConfiguration.MessageToNumber });
                await _messageHubContext.Clients.All.SendAsync("SmsReceived", item);
                return Ok();
                // TODO: find out why there is no XML formatter for CreatedAtActionResult().
                //return CreatedAtAction(nameof(GetMessgeById), item.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(AppConfiguration.TwilioAccountSid) || String.IsNullOrWhiteSpace(AppConfiguration.TwilioAuthToken))
                    throw new InvalidOperationException("The Twilio 'account sid' or 'auth token' is missing.");

                TwilioClient.Init(AppConfiguration.TwilioAccountSid, AppConfiguration.TwilioAuthToken);
                for (var i = 0; i < request.Count; i++)
                    await MessageResource.CreateAsync(from: new PhoneNumber(AppConfiguration.MessageToNumber), to: new PhoneNumber(AppConfiguration.MessageToNumber), body: request.Message);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion
    }
}
