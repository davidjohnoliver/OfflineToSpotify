using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Spotify.Extensions
{
	internal static class FullTrackExtensions
	{
		public static SpotifyTrackInfo ToTrackInfo(this FullTrack fullTrack)
			=> new SpotifyTrackInfo(fullTrack.Artists.First().Name, fullTrack.Name, fullTrack.Album.Name, fullTrack.Id);
	}
}
