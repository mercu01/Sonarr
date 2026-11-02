using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using NzbDrone.Core.IndexerSearch;
using NzbDrone.Core.Jobs;
using NzbDrone.Core.Lifecycle;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.JobTests
{
    [TestFixture]
    public class TaskManagerFixture : CoreTest<TaskManager>
    {
        [SetUp]
        public void Setup()
        {
            Mocker.GetMock<IScheduledTaskRepository>()
                  .Setup(s => s.All())
                  .Returns(new List<ScheduledTask>());
        }

        [TestCase(typeof(MissingEpisodeSearchCommand))]
        [TestCase(typeof(CutoffUnmetEpisodeSearchCommand))]
        public void should_schedule_wanted_episode_search_every_six_hours(Type commandType)
        {
            Subject.Handle(new ApplicationStartedEvent());

            var task = Subject.GetAll().SingleOrDefault(t => t.TypeName == commandType.FullName);

            task.Should().NotBeNull();
            task.Interval.Should().Be(6 * 60);

            Mocker.GetMock<IScheduledTaskRepository>()
                  .Verify(v => v.Upsert(It.Is<ScheduledTask>(t => t.TypeName == commandType.FullName && t.Interval == 6 * 60)), Times.Once());
        }
    }
}
