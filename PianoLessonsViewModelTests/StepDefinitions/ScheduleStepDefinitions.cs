using Moq;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using PianoLessons.ViewModels;
using System;
using TechTalk.SpecFlow;

namespace PianoLessonsViewModelTests.StepDefinitions
{
    [Binding]
    public class ScheduleStepDefinitions
    {
		private readonly ScenarioContext context;
		public ScheduleStepDefinitions(ScenarioContext context)
		{
			this.context = context;
		}

		[Given(@"a schedule view model")]
        public void GivenAScheduleViewModel()
		{
			var mock = new Mock<IPianoLessonsService>();
            var authMock = new Mock<IAuthService>();
            authMock.SetupGet(x => x.User).Returns(new PianoLessonsUser("1", "test", true, "test@gmail.com"));
			SchedulePageViewModel viewModel = new(new ShellNavigationService(), mock.Object, authMock.Object);
			context.Set(mock);
            context.Set(authMock);
            context.Set(viewModel);
        }

        [Given(@"a service that returns (.*) appointments")]
        public void GivenAServiceThatReturnsAppointments(int p0)
        {
            List<Appointment> appointments = new List<Appointment>();
            for (int i = 0; i < p0; i++) 
            {
                appointments.Add(new Appointment());
            }
			var mock = context.Get<Mock<IPianoLessonsService>>();
            mock.Setup(x => x.GetAppointmentsForStudent(It.IsAny<string>())).ReturnsAsync(appointments);
        }

        [When(@"you load the page as a teacher")]
        public void WhenYouLoadThePage()
        {
            var authMock = context.Get<Mock<IAuthService>>();
            authMock.Setup(x => x.User.IsTeacher).Returns(true);
            var vm = context.Get<SchedulePageViewModel>();
            vm.LoadedCommand.ExecuteAsync(null);
        }

        [Then(@"the schedule events should have a count of (.*)")]
        public void ThenTheScheduleEventsShouldHaveACountOf(int p0)
        {
            var vm = context.Get<SchedulePageViewModel>();
            vm.Events.Count.Should().Be(p0);
        }
    }
}
