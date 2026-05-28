using System.Text.RegularExpressions;
using NzbDrone.Common.Disk;
using NzbDrone.Common.EnvironmentInfo;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Configuration;
using Sonarr.Http;

namespace Sonarr.Api.V5.Logs;

[V5ApiController("log/file/update")]
public class UpdateLogFileController : LogFileControllerBase
{
    private readonly IAppFolderInfo _appFolderInfo;
    private readonly IDiskProvider _diskProvider;

    public UpdateLogFileController(IAppFolderInfo appFolderInfo,
                               IDiskProvider diskProvider,
                               IConfigFileProvider configFileProvider)
        : base(diskProvider, configFileProvider, "update")
    {
        _appFolderInfo = appFolderInfo;
        _diskProvider = diskProvider;
    }

    protected override IEnumerable<string> GetLogFiles()
    {
        if (!_diskProvider.FolderExists(_appFolderInfo.GetUpdateLogFolder()))
        {
            return Enumerable.Empty<string>();
        }

        return _diskProvider.GetFiles(_appFolderInfo.GetUpdateLogFolder(), false)
                                 .Where(f => Regex.IsMatch(Path.GetFileName(f), LOGFILE_ROUTE.TrimStart('/'), RegexOptions.IgnoreCase))
                                 .ToList();
    }

    protected override string GetLogFilePath(string filename)
    {
        return Path.Combine(_appFolderInfo.GetUpdateLogFolder(), filename);
    }

    protected override string DownloadUrlRoot
    {
        get
        {
            return "updatelogfile";
        }
    }
}
