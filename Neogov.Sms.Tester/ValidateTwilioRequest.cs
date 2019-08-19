using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Neogov.Sms.Tester.Models;
using System;
using System.Linq;
using Twilio.Security;

namespace Neogov.Sms.Tester
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateTwilioRequestAttribute : ActionFilterAttribute
    {
        #region Fields

        private readonly RequestValidator _requestValidator;

        #endregion

        #region Methods

        public ValidateTwilioRequestAttribute(AppConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (String.IsNullOrWhiteSpace(configuration.TwilioAuthToken))
                throw new InvalidOperationException("The Twilio authorization token is missing.");

            _requestValidator = new RequestValidator(configuration.TwilioAuthToken);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsValidRequest(context.HttpContext.Request))
                context.Result = new ForbidResult();
            else
                base.OnActionExecuting(context);
        }

        private bool IsValidRequest(HttpRequest request)
        {
#if DEBUG
            return true;
#else
            var requestUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            var parameters = request.Form.Keys
                .Select(key => new { Key = key, Value = request.Form[key] })
                .ToDictionary(p => p.Key, p => p.Value.ToString());
            var signature = request.Headers["X-Twilio-Signature"];

            return _requestValidator.Validate(requestUrl, parameters, signature);
#endif
        }

#endregion
    }
}
