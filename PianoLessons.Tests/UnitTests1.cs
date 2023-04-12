using FluentAssertions;
using Moq;
using PianoLessonsApi.Data;
using PianoLessons.Shared.Data;
using System.Text.RegularExpressions;
using PianoLessonsApi.Repositories;

namespace PianoLessons.Tests
{
    public class Tests
    {
        private PianoLessonsApplication app;
        private TestPianoLessonRepo repo;
        private RecordingRepo recordingRepo;
        private DateTime today;

        [SetUp]
        public void Setup()
        {
            repo = new();
            app = new(repo, recordingRepo);
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
                StudentId = "1",
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
                StudentId = "1",
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
                StudentId = "1"
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
                StudentId = "1",
            };
            await app.AddLog(log);

            var updatedLog = new PracticeLog
            {
                Id = 1,
                StartTime = today,
                EndTime = today.AddMinutes(50),
                StudentId = "1",
            };

            await app.UpdateLog(updatedLog);

            var logFromRepo = await app.GetLog(updatedLog.Id);
            logFromRepo.Duration.Should().Be(updatedLog.Duration);
        }

        [Test]
        public async Task CalculateScoreOfZero()
        {
            List<PracticeLog> logs = new()
            {
            };
            app.CalculateScore(logs, 10).Should().Be(0);
        }

        [Test]
        public async Task CalculateScoreForOneLog()
        {
            List<PracticeLog> logs = new()
            {
                new()
                {
                    Id = 1,
                    StartTime = DateTime.Today,
                    EndTime = DateTime.Today.AddMinutes(1),
                    StudentId = "1",
                }
            };
            app.CalculateScore(logs, 10).Should().Be(10);
            logs[0].EndTime = DateTime.Today.AddMinutes(10);
            app.CalculateScore(logs, 10).Should().Be(100);
        }

        [Test]
        public async Task CalculateScoreForManyLogs()
        {
            List<PracticeLog> logs = new()
            {
                new()
                {
                    Id = 1,
                    StartTime = DateTime.Today,
                    EndTime = DateTime.Today.AddMinutes(1),
                    StudentId = "1",
                },
                new()
                {
                    Id = 1,
                    StartTime = DateTime.Today,
                    EndTime = DateTime.Today.AddMinutes(5),
                    StudentId = "1",
                }
            };
            app.CalculateScore(logs, 10).Should().Be(60);
            logs.Add(new()
            {
                Id = 1,
                StartTime = DateTime.Today,
                EndTime = DateTime.Today.AddMinutes(5),
                StudentId = "1",
            });
            app.CalculateScore(logs, 10).Should().Be(110);
        }

        [Test]
        public void TestDoubleDutch()
        {
            var nibame = ToDoubleDutch("name");
            nibame.Should().Be("nibamibe");
			var bridger = ToDoubleDutch("bridger");
			bridger.Should().Be("bribidgiber");
		}

        public string ToDoubleDutch(string input)
        {
			string pattern = @"\b\w+\b";
			string DoubleDutchDelegate(System.Text.RegularExpressions.Match match)
            {
                string word = match.Value; 
                string doubledWord = "";
			    foreach (char c in word) 
                { 
                    if ("aeiouAEIOU".Contains(c)) 
                    { 
                        doubledWord += "ib" + c.ToString().ToLower(); 
                    } 
                    else { doubledWord += c; } 
                }
			    return doubledWord;
            }
			string output = Regex.Replace(input, pattern, DoubleDutchDelegate); return output;
		}
    }
}