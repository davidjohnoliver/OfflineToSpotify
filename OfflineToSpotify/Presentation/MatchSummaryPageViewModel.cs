using OfflineToSpotify.Model;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public class MatchSummaryPageViewModel
	{
		private readonly Track[] _allTracks;

		public int TotalCount => _allTracks.Length;

		public int MatchedCount =>
			_allTracks.Count(IsMatched);

		public int UnmatchedCount =>
			_allTracks.Count(t => !IsMatched(t));

		public MatchSummaryPageViewModel(Track[] allTracks)
		{
			_allTracks = allTracks;
		}

		private static bool IsMatched(Track track) => track.CandidateMatch != SpotifyTrackInfo.NonexistentTrack;
	}
}
