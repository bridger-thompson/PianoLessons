using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Data;

public class MailService
{
	private readonly MailjetClient mailClient;

	public MailService(IConfiguration config)
	{
		mailClient = new(config["MJ_APIKEY_PUBLIC"], config["MJ_APIKEY_PRIVATE"]);
	}

	public async Task SendEmail(Appointment appointment)
	{
		var toAddress = appointment.StudentId;
		if (toAddress != null)
		{
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
		}
	}
}
