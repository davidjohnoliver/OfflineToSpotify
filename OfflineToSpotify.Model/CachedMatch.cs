using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Model
{
	internal class CachedMatch
	{
		public int Id { get; set; }

		public string? ImportPath { get; set; }

		public SpotifyTrackInfo Match { get; set; } = SpotifyTrackInfo.NonexistentTrack;
	}
}
