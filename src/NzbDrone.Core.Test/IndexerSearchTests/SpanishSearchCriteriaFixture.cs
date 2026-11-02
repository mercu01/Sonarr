using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.IndexerSearch.Definitions;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Core.Tv;

namespace NzbDrone.Core.Test.IndexerSearchTests
{
    [TestFixture]
    public class SpanishSearchCriteriaFixture : CoreTest
    {
        private static string[] GetLines(SearchCriteriaBase criteria)
        {
            return criteria.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        [Test]
        public void season_search_should_describe_every_spanish_season_format_for_each_scene_title()
        {
            var criteria = new SeasonSearchCriteria
            {
                Series = new Series { Title = "Black Mirror" },
                SceneTitles = new List<string> { "Black Mirror", "Espejo Negro" },
                SeasonNumber = 6
            };

            GetLines(criteria).Should().Equal(
                "[Black Mirror]. Search term: ",
                "[Black Mirror Temporada 6]",
                "[Espejo Negro Temporada 6]",
                "[Black Mirror S06]",
                "[Espejo Negro S06]",
                "[Black Mirror T6]",
                "[Espejo Negro T6]",
                "[Black Mirror T 6]",
                "[Espejo Negro T 6]",
                "[Black Mirror T06]",
                "[Espejo Negro T06]",
                "[Black Mirror T 06]",
                "[Espejo Negro T 06]");
        }

        [TestCase(2, 1, "Cap.201", "2x1", "2x01")]
        [TestCase(12, 15, "Cap.1215", "12x15", "12x15")]
        public void anime_episode_search_should_describe_every_spanish_episode_format(int seasonNumber, int episodeNumber, string capFormat, string shortFormat, string paddedFormat)
        {
            var criteria = new AnimeEpisodeSearchCriteria
            {
                Series = new Series { Title = "Rapa" },
                SceneTitles = new List<string> { "Rapa" },
                SeasonNumber = seasonNumber,
                EpisodeNumber = episodeNumber
            };

            GetLines(criteria).Should().Equal(
                "[Rapa]. Search term: ",
                $"[Rapa {capFormat}]",
                $"[Rapa {shortFormat}]",
                $"[Rapa {paddedFormat}]");
        }
    }
}
