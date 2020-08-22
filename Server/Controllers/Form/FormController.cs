using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Controllers.Form
{
    public abstract class FormController : ControllerBase
    {
        public IActionResult Back()
        {
            var referrer = Request.Headers["Referer"];

            if (string.IsNullOrEmpty(referrer)) return Ok();

            return Redirect(referrer);
        }

        public void SendError(string message) => Response.Cookies.Append("error", message);
    }
}
