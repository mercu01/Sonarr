using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;
using NzbDrone.Common.Disk;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Core.Tv;
using NzbDrone.Core.Tv.Events;
using NzbDrone.Test.Common;

namespace NzbDrone.Core.Test.TvTests.SeriesServiceTests
{
    [TestFixture]
    public class AddSeriesFixture : CoreTest<SeriesService>
    {
        private Series _series;

        [SetUp]
        public void Setup()
        {
            _series = Builder<Series>.CreateNew()
                                     .With(s => s.Path = @"C:\Test\TV\Series Title".AsOsAgnostic())
                                     .Build();
        }

        private void GivenSeriesFolderExists(bool exists)
        {
            Mocker.GetMock<IDiskProvider>()
                  .Setup(s => s.FolderExists(_series.Path))
                  .Returns(exists);
        }

        [Test]
        public void should_create_series_folder_when_it_does_not_exist()
        {
            GivenSeriesFolderExists(false);

            Subject.AddSeries(_series);

            Mocker.GetMock<IDiskProvider>().Verify(v => v.CreateFolder(_series.Path), Times.Once());
        }

        [Test]
        public void should_not_create_series_folder_when_it_already_exists()
        {
            GivenSeriesFolderExists(true);

            Subject.AddSeries(_series);

            Mocker.GetMock<IDiskProvider>().Verify(v => v.CreateFolder(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void should_insert_series_and_publish_series_added_event()
        {
            GivenSeriesFolderExists(false);

            Subject.AddSeries(_series);

            Mocker.GetMock<ISeriesRepository>().Verify(v => v.Insert(_series), Times.Once());
            VerifyEventPublished<SeriesAddedEvent>();
        }
    }
}
