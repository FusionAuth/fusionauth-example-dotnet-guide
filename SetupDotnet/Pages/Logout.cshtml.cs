using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SetupDotnet.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        private readonly IConfiguration _configuration;

        public LogoutModel(ILogger<LogoutModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            SignOut("cookie", "oidc");
            var host = _configuration["SetupDotnet:Authority"];
            var cookieName = _configuration["SetupDotnet:CookieName"];

            var clientId = _configuration["SetupDotnet:ClientId"];
            var url = host + "/oauth2/logout?client_id=" + clientId;
            Response.Cookies.Delete(cookieName);
            return Redirect(url);
        }
    }
}