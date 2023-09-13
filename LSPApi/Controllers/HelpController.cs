using LSPApi.DataLayer;

using Microsoft.AspNetCore.Mvc;

using System.Net.Mail;
using LSPApi.DataLayer.Model;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelpController : ControllerBase
{
    private readonly ILogger<HelpController> _logger;
    private readonly IConfiguration _configuration;

    public HelpController(ILogger<HelpController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EMail data)
    {
        
        var sendGridClient = new SendGridClient(_configuration["SendGridKey"].ToString());

        if (string.IsNullOrEmpty(data.To)) return BadRequest();
        if (string.IsNullOrEmpty(data.From)) return BadRequest();
        if (string.IsNullOrEmpty(data.Subject)) return BadRequest();
        if (string.IsNullOrEmpty(data.Text)) return BadRequest();

        var from = new EmailAddress("info@lightswitchpress.com", "Light Switch Press ");
        var to = new EmailAddress(data.To, data.To);
        var subject = data.Subject;
        var plainText = data.Text;
        var htmlContent = data.Text;

        var message = MailHelper.CreateSingleEmail(from, to, subject, plainText, htmlContent);
        var response = await sendGridClient.SendEmailAsync(message);

        return Ok();
    }
}
