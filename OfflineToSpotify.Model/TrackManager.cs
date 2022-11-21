using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Model
{
	public static class TrackManager
	{
		public static async Task UpdateMissingMatches(Track track, PlaylistDB db, SearchHelper searchHelper, int matchesCount)
		{
			if (track.SpotifyMatches.Count > 0 || track.WereNoCandidatesFound || track.TrackInfo is null)
			{
				return;
			}

			var matches = await searchHelper.SearchTrack(track.TrackInfo, matchesCount);
			track.SpotifyMatches = matches.ToList();
			if (matches.Count == 0)
			{
				track.WereNoCandidatesFound = true;
			}

			await db.UpdateTrack(track);
		}
	}
}
