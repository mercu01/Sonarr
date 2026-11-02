using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NzbDrone.Core.DataAugmentation.Scene;
using NzbDrone.Core.Parser;
using NzbDrone.Core.SeriesStats;
using NzbDrone.Core.Tv;
using NzbDrone.Test.Common;
using Sonarr.Api.V3.Series;

namespace NzbDrone.Api.Test.v3.Series
{
    [TestFixture]
    public class SeriesControllerFixture : TestBase<SeriesController>
    {
        private NzbDrone.Core.Tv.Series _series;
        private List<SceneMapping> _sceneMappings;

        [SetUp]
        public void Setup()
        {
            _series = new NzbDrone.Core.Tv.Series
            {
                Id = 1,
                TvdbId = 1234,
                Title = "The Original Title",
                Path = @"C:\Test\TV\The Original Title".AsOsAgnostic()
            };

            _sceneMappings = new List<SceneMapping>();

            Mocker.GetMock<ISeriesService>()
                  .Setup(s => s.GetSeries(_series.Id))
                  .Returns(_series);

            Mocker.GetMock<ISceneMappingService>()
                  .Setup(s => s.FindByTvdbId(_series.TvdbId))
                  .Returns(_sceneMappings);

            Mocker.GetMock<ISeriesStatisticsService>()
                  .Setup(s => s.SeriesStatistics(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(new SeriesStatistics());

            Mocker.GetMock<ISeriesTranslationService>()
                  .Setup(s => s.GetTranslations(It.IsAny<int>()))
                  .Returns(new List<SeriesTranslation>());
        }

        private SeriesResource GivenResourceWithTitle(string title)
        {
            var resource = _series.ToResource();
            resource.Title = title;

            return resource;
        }

        private void VerifySceneMappingNotAdded()
        {
            Mocker.GetMock<ISceneMappingService>()
                  .Verify(v => v.Add(It.IsAny<SceneMapping>()), Times.Never());
        }

        [Test]
        public void should_add_custom_scene_mapping_when_title_is_changed()
        {
            const string newTitle = "El Titulo Original";

            Subject.UpdateSeries(GivenResourceWithTitle(newTitle));

            Mocker.GetMock<ISceneMappingService>()
                  .Verify(v => v.Add(It.Is<SceneMapping>(m => m.TvdbId == _series.TvdbId &&
                                                              m.Title == newTitle &&
                                                              m.SearchTerm == newTitle &&
                                                              m.ParseTerm == newTitle.CleanSeriesTitle() &&
                                                              m.SeasonNumber == -1 &&
                                                              m.Type == "CustomScene")),
                          Times.Once());
        }

        [Test]
        public void should_update_series_when_title_is_changed()
        {
            var path = _series.Path;

            Subject.UpdateSeries(GivenResourceWithTitle("El Titulo Original"));

            Mocker.GetMock<ISeriesService>()
                  .Verify(v => v.UpdateSeries(_series, path, path, false), Times.Once());
        }

        [Test]
        public void should_not_add_scene_mapping_when_title_is_unchanged()
        {
            Subject.UpdateSeries(GivenResourceWithTitle(_series.Title));

            VerifySceneMappingNotAdded();
        }

        [Test]
        public void should_not_add_scene_mapping_when_title_is_already_a_search_term()
        {
            _sceneMappings.Add(new SceneMapping
            {
                TvdbId = _series.TvdbId,
                Title = "El Titulo Original",
                SearchTerm = "El Titulo Original"
            });

            Subject.UpdateSeries(GivenResourceWithTitle("El Titulo Original"));

            VerifySceneMappingNotAdded();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void should_not_add_scene_mapping_when_title_is_blank(string title)
        {
            Subject.UpdateSeries(GivenResourceWithTitle(title));

            VerifySceneMappingNotAdded();
        }
    }
}
