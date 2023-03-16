using FluentAssertions;
using Moq;
using PianoLessonsApi.Data;
using PianoLessons.Shared.Data;
using PianoLessonsApi.Repositories;

namespace PianoLessons.Tests
{
    public class Tests
    {
        private PianoLessonsApplication app;
        private TestPianoLessonRepo repo;
        private DateTime today;

        [SetUp]
        public void Setup()
        {
            repo = new();
            app = new(repo);
            today = DateTime.Today;
        }

        [Test]
        public async Task OneLogAdded_LogCountShouldBe1()
        {
            repo.Logs.Count.Should().Be(0);
            await app.AddLog(new PracticeLog
            {
                StartTime = today,
                EndTime = today.AddMinutes(5),   
                StudentId = 1,
                AssignmentId = 2
            });
            repo.Logs.Count.Should().Be(1);
        }

        [Test]
        public async Task OneLogAddedOneLogDeleted_LogCountShouldBe0()
        {
            repo.Logs.Count.Should().Be(0);
            var log = new PracticeLog
            {
                Id = 1,
                StartTime = today,
                EndTime = today.AddMinutes(5),
                StudentId = 1,
                AssignmentId = 2
            };
            await app.AddLog(log);
            repo.Logs.Count.Should().Be(1);
            await app.DeleteLog(log.Id);
            repo.Logs.Count.Should().Be(0);
        }

        [Test]
        public async Task GetLog()
        {
            var log = new PracticeLog
            {
                Id = 1,
                StartTime = today,
                EndTime = today.AddMinutes(5),
                StudentId = 1,
                AssignmentId = 2
            };
            await app.AddLog(log);

            var gottenLog = await repo.GetLog(log.Id);

            gottenLog.Should().Be(log);
        }

        [Test]
        public async Task OneLogAddedandUpdated_LogIsUpdated()
        {
            var log = new PracticeLog
            {
                Id = 1,
                StartTime = today,
                EndTime = today.AddMinutes(5),
                StudentId = 1,
                AssignmentId = 2
            };
            await app.AddLog(log);

            var updatedLog = new PracticeLog
            {
                Id = 1,
                StartTime = today,
                EndTime = today.AddMinutes(50),
                StudentId = 1,
                AssignmentId = 2
            };

            await app.UpdateLog(updatedLog);

            var logFromRepo = await app.GetLog(updatedLog.Id);
            logFromRepo.Duration.Should().Be(updatedLog.Duration);
        }
    }
}