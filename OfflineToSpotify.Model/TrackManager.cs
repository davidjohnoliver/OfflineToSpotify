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
		public static async Task UpdateTrack(Track track, PlaylistDB db)
		{
			await db.UpdateTrack(track);
		}

		public static async Task UpdateTracks(IEnumerable<Track> tracks, PlaylistDB db)
		{
			await db.UpdateTracks(tracks);
		}

		public static async Task UpdateMissingMatches(Track track, PlaylistDB db, SearchHelper searchHelper, int matchesCount)
		{
			if (track.SpotifyMatches.Count > 0 || track.WereNoCandidatesFound || track.TrackInfo is null)
			{
				return;
			}

			var matches = await searchHelper.SearchTrack(track.TrackInfo, matchesCount, QueryFormat.SimpleNoAlbum);
			track.SpotifyMatches = matches.ToList();
			if (matches.Count == 0)
			{
				track.WereNoCandidatesFound = true;
			}
			else
			{
				track.CandidateMatch = matches.First();
			}

			await db.UpdateTrack(track);
		}
	}
}
