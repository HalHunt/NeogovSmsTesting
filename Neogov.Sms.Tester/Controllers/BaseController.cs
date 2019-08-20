using Microsoft.AspNetCore.Mvc;
using Neogov.Sms.Tester.Models;
using System;

namespace Neogov.Sms.Tester.Controllers
{
    [ApiController, Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        public BaseController(AppConfiguration configuration)
        {
            AppConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected AppConfiguration AppConfiguration { get; }
    }
}
