using System.Collections.Generic;
using System.Text;

namespace NzbDrone.Core.IndexerSearch.Definitions
{
    public class SeasonSearchCriteria : SearchCriteriaBase
    {
        public readonly List<string> ModesSearchSpanish = new List<string>()
        {
            "Temporada {0:0}",
            "S{0:00}",
            "T{0:0}",
            "T {0:0}",
            "T{0:00}",
            "T {0:00}"
        };
        public int SeasonNumber { get; set; }

        public override string ToString()
        {
            var searchTerms = new StringBuilder();

            searchTerms.AppendFormat($"[{Series.Title}]. Search term: ").AppendLine();
            foreach (var mode in ModesSearchSpanish)
            {
                foreach (var translate in SceneTitles)
                {
                    searchTerms.AppendFormat($"[{translate} {mode}]", SeasonNumber).AppendLine();
                }
            }

            return searchTerms.ToString();
        }
    }
}
