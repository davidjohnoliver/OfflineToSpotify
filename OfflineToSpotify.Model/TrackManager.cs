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
		public static async Task UpdateTrack(Track track, PlaylistDB db, CachedMatchesDB matchesDB)
		{
			await MaybeUpdateCachedMatch(track, matchesDB);
			await db.UpdateTrack(track);
		}

		public static async Task UpdateTracks(IEnumerable<Track> tracks, PlaylistDB db, CachedMatchesDB matchesDB)
		{
			foreach (var track in tracks)
			{
				// TODO: CachedMatchesDB should support bulk update
				await MaybeUpdateCachedMatch(track, matchesDB);
			}
			await db.UpdateTracks(tracks);
		}

		private static async Task MaybeUpdateCachedMatch(Track track, CachedMatchesDB matchesDB)
		{
			if (track.IsCandidateConfirmed && track.ImportPath != null)
			{
				await matchesDB.UpdateMatch(track.ImportPath, track.CandidateMatch);
			}
		}

		public static async Task UpdateMissingMatches(Track track, PlaylistDB db, CachedMatchesDB matchesDB, SearchHelper searchHelper, int matchesCount)
		{
			if (track.SpotifyMatches.Count > 0 || track.WereNoCandidatesFound || track.TrackInfo is null)
			{
				return;
			}

			var matches = await searchHelper.SearchTrack(track.TrackInfo, matchesCount, QueryFormat.SimpleNoAlbum);
			track.SpotifyMatches = matches.ToList();

			// Check cached matches, and use as confirmed candidate if available
			if (track.ImportPath != null && matchesDB.GetMatch(track.ImportPath) is { } cachedMatch)
			{
				if (!track.SpotifyMatches.Contains(cachedMatch))
				{
					track.SpotifyMatches.Insert(0, cachedMatch);
				}
				track.CandidateMatch = cachedMatch;
				track.IsCandidateConfirmed = true;
			}
			else
			{
				if (matches.Count == 0)
				{
					track.WereNoCandidatesFound = true;
				}
				else
				{
					track.CandidateMatch = matches.First();
				} 
			}

			await db.UpdateTrack(track);
		}
	}
}
