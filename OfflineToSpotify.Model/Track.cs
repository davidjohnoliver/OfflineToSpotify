using OfflineToSpotify.Core;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Model
{
	public class Track
	{
		public int Id { get; set; }
		public TrackInfo? TrackInfo {get;set;}
		public string? ImportPath { get; set; }
		public List<SpotifyTrackInfo> SpotifyMatches { get; set; } = new List<SpotifyTrackInfo>();
		public SpotifyTrackInfo CandidateMatch { get; set; } = SpotifyTrackInfo.NonexistentTrack;
		public bool IsCandidateConfirmed { get; set; }
		public bool WereNoCandidatesFound { get; set; }

		public override string ToString()
		{
			return $"Id={Id}, {TrackInfo}";
		}
	}
}
