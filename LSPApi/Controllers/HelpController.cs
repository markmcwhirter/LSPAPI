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
    private readonly IConfiguration _configuration;

    public HelpController( IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail([FromBody] EMail data)
    {

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var creds = _configuration["SendGridKey"].ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        var sendGridClient = new SendGridClient(creds);

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
        _ = await sendGridClient.SendEmailAsync(message);

        return Ok();
    }
}
