using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Spotify
{

	/// <summary>
	/// Lightweight holder for info about a Spotify track listing.
	/// </summary>
	public record SpotifyTrackInfo(string Artist, string Title, string Album, string TrackId)
	{
		// We set empty fields to null to match the DB, and because we are too lazy to mess with item template selectors
		public static SpotifyTrackInfo NonexistentTrack { get; } = new("None", null!, null!, "-1");
	}
}
