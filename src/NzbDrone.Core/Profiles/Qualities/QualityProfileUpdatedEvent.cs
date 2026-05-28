using NzbDrone.Common.Messaging;

namespace NzbDrone.Core.Profiles.Qualities;

public class QualityProfileUpdatedEvent(int id) : IEvent
{
    public int Id { get; private set; } = id;
}
