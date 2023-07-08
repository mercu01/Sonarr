using System.Collections.Generic;
using System.Text;

namespace NzbDrone.Core.IndexerSearch.Definitions
{
    public class SeasonSearchCriteria : SearchCriteriaBase
    {
        public readonly List<string> ModesSearchSpanish = new List<string>(){
            "S{1:00}",
            "T{1:0}",
            "T {1:0}",
            "T{1:00}",
            "T {1:00}"
        };
        public int SeasonNumber { get; set; }

        public override string ToString()
        {
            StringBuilder searchTerms = new StringBuilder();

            searchTerms.AppendFormat($"[{Series.Title}]. Search term: ").AppendLine();
            foreach (var mode in ModesSearchSpanish)
            {
                foreach (var translate in SceneTitles)
                {
                    searchTerms.AppendFormat($"[{translate} {mode}]", "fakeparam", SeasonNumber).AppendLine();
                }
            }
            return searchTerms.ToString();
        }
    }
}