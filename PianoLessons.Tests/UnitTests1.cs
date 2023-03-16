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

        [SetUp]
        public void Setup()
        {
            repo = new();
            app = new(repo);
        }

        [Test]
        public async Task OneLogAdded_LogCountShouldBe1()
        {
            repo.Logs.Count.Should().Be(0);
            await app.AddLog(new PracticeLog
            {
                Duration= TimeSpan.FromMinutes(5),
                LogDate = DateTime.Now,
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
                Duration = TimeSpan.FromMinutes(5),
                LogDate = DateTime.Now,
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
                Duration = TimeSpan.FromMinutes(5),
                LogDate = DateTime.Now,
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
                Duration = TimeSpan.FromMinutes(5),
                LogDate = DateTime.Now,
                StudentId = 1,
                AssignmentId = 2
            };
            await app.AddLog(log);

            var updatedLog = new PracticeLog
            {
                Id = 1,
                Duration = TimeSpan.FromMinutes(50),
                LogDate = DateTime.Now,
                StudentId = 1,
                AssignmentId = 2
            };

            await app.UpdateLog(updatedLog);

            var logFromRepo = await app.GetLog(updatedLog.Id);
            logFromRepo.Duration.Should().Be(updatedLog.Duration);
        }
    }
}