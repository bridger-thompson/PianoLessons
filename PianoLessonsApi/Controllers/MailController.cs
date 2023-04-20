using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly ILogger<MailController> logger;
    private readonly MailjetClient mailClient;

    public MailController(IConfiguration config, ILogger<MailController> logger)
    {
        this.mailClient = new(config["MJ_APIKEY_PUBLIC"], config["MJ_APIKEY_PRIVATE"]);
        this.config = config;
        this.logger = logger;
    }

    [HttpPost("{toAddress}")]
    public async Task SendEmail(Appointment appointment)
    {
        var toAddress = appointment.StudentId;
        logger.LogInformation(toAddress);
        var email = new TransactionalEmailBuilder()
            .WithFrom(new SendContact("bridger.thompson@students.snow.edu"))
            .WithSubject("New Lesson Scheduled!")
            .WithHtmlPart(
            @$"
                    <h1>Lesson Scheduled!</h1>
                    <p>Your teacher scheduled a lesson with you.</p>
                    <p>Lesson: {appointment.StartAt} - {appointment.EndAt}</p>
                ")
            .WithTo(new SendContact(toAddress))
            .Build();
        var response = await mailClient.SendTransactionalEmailAsync(email);
        logger.LogInformation("Email Sent " + response.Messages);
    }
}
