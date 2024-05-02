using System.Collections.Generic;
using System.Text;
namespace NzbDrone.Core.IndexerSearch.Definitions
{
    public class AnimeEpisodeSearchCriteria : SearchCriteriaBase
    {
        public readonly List<string> ModesSearchSpanish = new List<string>()
        {
            "Cap.{1:0}{2:00}",
            "{1:0}x{2:0}",
            "{1:0}x{2:00}"
        };
        public int AbsoluteEpisodeNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
        public bool IsSeasonSearch { get; set; }

        public override string ToString()
        {
            var searchTerms = new StringBuilder();

            searchTerms.AppendFormat($"[{Series.Title}]. Search term: ").AppendLine();
            foreach (var mode in ModesSearchSpanish)
            {
                foreach (var translate in SceneTitles)
                {
                    searchTerms.AppendFormat($"[{translate} {mode}]", "fakeparam", SeasonNumber, EpisodeNumber).AppendLine();
                }
            }

            return searchTerms.ToString();
        }
    }
}
