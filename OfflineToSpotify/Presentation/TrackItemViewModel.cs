using OfflineToSpotify.Model;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	sealed class TrackItemViewModel : ViewModelBase
	{
		public Track Track { get; }

		public string Artist => Track.TrackInfo.Artist;

		public string Title => Track.TrackInfo.Title;

		public IEnumerable<MatchItemViewModel> Matches { get; }

		public TrackItemViewModel(Track track)
		{
			Track = track;

			Matches = track.SpotifyMatches.Select(m => new MatchItemViewModel(m)).ToArray();
		}

		public class MatchItemViewModel
		{
			public SpotifyTrackInfo Info { get; }

			public MatchItemViewModel(SpotifyTrackInfo spotifyTrackInfo)
			{
				Info = spotifyTrackInfo;
			}
		}
	}
}
