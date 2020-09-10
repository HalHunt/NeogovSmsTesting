using Microsoft.AspNetCore.Mvc;
using Neogov.Sms.Tester.Models;

namespace Neogov.Sms.Tester.Controllers
{
    public class ConfigurationController : BaseController
    {
        #region Constructors

        public ConfigurationController(AppConfiguration configuration) : base(configuration)
        {
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetConfiguration()
        {
            return Ok(AppConfiguration);
        }

        #endregion
    }
}
