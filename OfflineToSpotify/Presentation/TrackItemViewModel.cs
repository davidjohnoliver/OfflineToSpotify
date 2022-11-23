#nullable enable

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

		public string? Artist => Track.TrackInfo?.Artist;

		public string? Title => Track.TrackInfo?.Title;

		public IEnumerable<MatchItemViewModel> Matches { get; }

		private MatchItemViewModel? _candidateMatch;
		private readonly PlaylistDB _db;
		private readonly IProgressIndicator _progressIndicator;

		public MatchItemViewModel? CandidateMatch
		{
			get => _candidateMatch;
			set
			{
				if (OnValueSet(ref _candidateMatch, value))
				{
					UpdateDB();
				}
			}
		}

		private async void UpdateDB()
		{
			using (_progressIndicator.ShowIndicator())
			{
				Track.CandidateMatch = CandidateMatch?.Info;

				await _db.UpdateTrack(Track);
			}
		}

		public TrackItemViewModel(Track track, PlaylistDB db, IProgressIndicator progressIndicator)
		{
			Track = track;
			_db = db;
			_progressIndicator = progressIndicator;
			Matches = track.SpotifyMatches.Select(m => new MatchItemViewModel(m)).ToArray();

			_candidateMatch = Matches.FirstOrDefault(m => m.Info == track.CandidateMatch);
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
