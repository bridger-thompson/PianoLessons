using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using PianoLessons.Shared.Data;
using PianoLessonsApi.Repositories;

namespace PianoLessonsApi.Data;

public class MailService
{
	private readonly MailjetClient mailClient;
	private readonly IPianoLessonsRepo repo;

	public MailService(IConfiguration config, IPianoLessonsRepo repo)
	{
		mailClient = new(config["MJ_APIKEY_PUBLIC"], config["MJ_APIKEY_PRIVATE"]);
		this.repo = repo;
	}

	public async Task SendEmail(Appointment appointment)
	{
		var student = await repo.GetStudent(appointment.StudentId);
		if (student != null)
		{
			var email = new TransactionalEmailBuilder()
				.WithFrom(new SendContact("anthony.hardman@students.snow.edu"))
				.WithSubject("New Lesson Scheduled!")
				.WithHtmlPart(
				@$"
                    <h1>Lesson Scheduled!</h1>
                    <p>Your teacher scheduled a lesson with you.</p>
                    <p>Lesson: {appointment.StartAt} - {appointment.EndAt}</p>
                ")
				.WithTo(new SendContact(student.Email))
				.Build();
			var response = await mailClient.SendTransactionalEmailAsync(email);
		}
	}
}
