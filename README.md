# Modified version of sonarr for atomoHD, atomixHQ, etc.

Serie type: "anime", will generate the following search format in jackett:

"{Series.Title} Cap.{SeasonNumber:0}{EpisodeNumber:00}"
<img src="Sample1.jpg" alt="Sample anime">

The title field allows add search terms (scene mapping):
 
<img src="Sample2.jpg" alt="Sample change title">

## How to use

1. Download patch: "patch_build_mono.7z" or build branch. [Download](patch_build_mono.7z)

2. Override writes the following files to your sonarr installation:
- File: Sonarr.Core.dll
- File: Sonarr.Api.V3.dll
- Folder: /UI

## Docker compose
```yaml
  sonarr:
    image: linuxserver/sonarr
    container_name: sonarr
    networks:
      - default
    depends_on:
      - qbittorrent
      - jackett
    volumes:
      - /path/to/conf:/config
      - /path/to/tv:/tv
      - /path/to/downloads:/downloads
      - type: bind
        source: patch/UI
        target: /app/sonarr/bin/UI
      - type: bind
        source: patch/Sonarr.Core.dll
        target: /app/sonarr/bin/Sonarr.Core.dll
      - type: bind
        source: patch/Sonarr.Api.V3.dll
        target: /app/sonarr/bin/Sonarr.Api.V3.dll
    ports:
      - 8989:8989
    restart: unless-stopped
    environment:
      - UMASK_SET=022 #optional
      - PUID=1000
      - PGID=1000
```
## Tips 

- Jackett indexer: BTDigg (need config flaresolverr)
- Sonarr setting - Profile - Language: English, Spanish, Unknown
- Sonarr setting - Profile - Quality profiles: WEB 1080p
- Sonarr setting - Media Management - Anime Episode Format: "{Series Title} - S{season:00}E{episode:00} - {Episode Title} - {Quality Full} - Spanish - audio {MediaInfo AudioLanguages} - sub {MediaInfo SubtitleLanguages}"
-----------------------------------

# <img width="24px" src="./Logo/256.png" alt="Sonarr"></img> Sonarr 

Sonarr is a PVR for Usenet and BitTorrent users. It can monitor multiple RSS feeds for new episodes of your favorite shows and will grab, sort and rename them. It can also be configured to automatically upgrade the quality of files already downloaded when a better quality format becomes available.

## Getting Started

- [Download/Installation](https://sonarr.tv/#downloads-v3)
- [FAQ](https://wiki.servarr.com/sonarr/faq)
- [Wiki](https://wiki.servarr.com/Sonarr)
- [(WIP) API Documentation](https://github.com/Sonarr/Sonarr/wiki/API)
- [Donate](https://sonarr.tv/donate)

## Support
Note: GitHub Issues are for Bugs and Feature Requests Only

- [Forums](https://forums.sonarr.tv/)
- [Discord](https://discord.gg/M6BvZn5)
- [GitHub - Bugs and Feature Requests Only](https://github.com/Sonarr/Sonarr/issues)
- [IRC](https://web.libera.chat/?channels=#sonarr)
- [Reddit](https://www.reddit.com/r/sonarr)
- [Wiki](https://wiki.servarr.com/sonarr)



## Features

### Current Features

- Support for major platforms: Windows, Linux, macOS, Raspberry Pi, etc.
- Automatically detects new episodes
- Can scan your existing library and download any missing episodes
- Can watch for better quality of the episodes you already have and do an automatic upgrade. *eg. from DVD to Blu-Ray*
- Automatic failed download handling will try another release if one fails
- Manual search so you can pick any release or to see why a release was not downloaded automatically
- Fully configurable episode renaming
- Full integration with SABnzbd and NZBGet
- Full integration with Kodi, Plex (notification, library update, metadata)
- Full support for specials and multi-episode releases
- And a beautiful UI

## Contributing

### Development
This project exists thanks to all the people who contribute. [Contribute](CONTRIBUTING.md).

<a href="https://github.com/Sonarr/Sonarr/graphs/contributors"><img src="https://opencollective.com/Sonarr/contributors.svg?width=890&button=false" /></a>

### Supporters

This project would not be possible without the support of our users and software providers. 
[**Become a sponsor or backer**](https://opencollective.com/sonarr) to help us out!

#### Mega Sponsors

[![Sponsors](https://opencollective.com/sonarr/tiers/mega-sponsor.svg?width=890)](https://opencollective.com/sonarr/contribute/mega-sponsor-21443/checkout)

#### Sponsors

[![Flexible Sponsors](https://opencollective.com/sonarr/sponsors.svg?width=890)](https://opencollective.com/sonarr/contribute/sponsor-21457/checkout)

#### Backers

[![Backers](https://opencollective.com/sonarr/backers.svg?width=890)](https://opencollective.com/sonarr/contribute/backer-21442/checkout)

#### JetBrains

Thank you to [<img src="/Logo/Jetbrains/jetbrains.svg" alt="JetBrains" width="32"> JetBrains](http://www.jetbrains.com/) for providing us with free licenses to their great tools

* [<img src="/Logo/Jetbrains/teamcity.svg" alt="TeamCity" width="32"> TeamCity](http://www.jetbrains.com/teamcity/)
* [<img src="/Logo/Jetbrains/resharper.svg" alt="ReSharper" width="32"> ReSharper](http://www.jetbrains.com/resharper/)
* [<img src="/Logo/Jetbrains/dottrace.svg" alt="dotTrace" width="32"> dotTrace](http://www.jetbrains.com/dottrace/)

### Licenses

- [GNU GPL v3](http://www.gnu.org/licenses/gpl.html)	
- Copyright 2010-2021
