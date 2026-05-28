using NzbDrone.Core.CustomFormats;
using NzbDrone.Core.History;
using NzbDrone.Core.Languages;
using NzbDrone.Core.Qualities;
using Sonarr.Api.V5.CustomFormats;
using Sonarr.Api.V5.Episodes;
using Sonarr.Api.V5.Series;
using Sonarr.Http.REST;

namespace Sonarr.Api.V5.History;

public class HistoryResource : RestResource
{
    public int EpisodeId { get; set; }
    public int SeriesId { get; set; }
    public required string SourceTitle { get; set; }
    public required List<Language> Languages { get; set; }
    public required QualityModel Quality { get; set; }
    public required List<CustomFormatResource> CustomFormats { get; set; }
    public int CustomFormatScore { get; set; }
    public bool QualityCutoffNotMet { get; set; }
    public DateTime Date { get; set; }
    public string? DownloadId { get; set; }
    public EpisodeHistoryEventType EventType { get; set; }
    public required Dictionary<string, string> Data { get; set; }
    public EpisodeResource? Episode { get; set; }
    public SeriesResource? Series { get; set; }
}

public static class HistoryResourceMapper
{
    public static HistoryResource ToResource(this EpisodeHistory model, ICustomFormatCalculationService formatCalculator)
    {
        var customFormats = formatCalculator.ParseCustomFormat(model, model.Series);
        var customFormatScore = model.Series.QualityProfile.Value.CalculateCustomFormatScore(customFormats);

        return new HistoryResource
        {
            Id = model.Id,
            EpisodeId = model.EpisodeId,
            SeriesId = model.SeriesId,
            SourceTitle = model.SourceTitle,
            Languages = model.Languages,
            Quality = model.Quality,
            CustomFormats = customFormats.ToResource(false),
            CustomFormatScore = customFormatScore,
            Date = model.Date,
            DownloadId = model.DownloadId,
            EventType = model.EventType,
            Data = model.Data
        };
    }
}
