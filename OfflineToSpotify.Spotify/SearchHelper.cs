using OfflineToSpotify.Core;
using OfflineToSpotify.Spotify.Extensions;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Spotify
{
	public sealed class SearchHelper
	{
		private readonly SpotifyClient _client;

		public SearchHelper(SpotifyClient spotifyClient)
		{
			_client = spotifyClient;
		}

		public async Task<SpotifyTrackInfo> FindTrackById(string id)
		{
			var track = await _client.Tracks.Get(id);
			return track.ToTrackInfo();
		}

		public Task<IList<SpotifyTrackInfo>> SearchTrack(TrackInfo trackInfo, int matches, QueryFormat queryFormat)
			=> SearchTrack(trackInfo.Title, trackInfo.Artist, trackInfo.Album, matches, queryFormat);

		private async Task<IList<SpotifyTrackInfo>> SearchTrack(string title, string artist, string? album, int matches, QueryFormat queryFormat)
		{
			var query = queryFormat switch
			{
				QueryFormat.SimpleWithAlbum => $"{title} {artist} {album}",
				QueryFormat.SimpleNoAlbum => $"{title} {artist}",
				QueryFormat.StructuredWithAlbum => BuildStructuredQuery(title, artist, album),
				QueryFormat.StructuredNoAlbum => BuildStructuredQuery(title, artist, null),
				_ => throw new ArgumentException()
			};

			var item = await _client.Search.Item(new SearchRequest(SearchRequest.Types.Track, query));

			return (IList<SpotifyTrackInfo>?)item.Tracks.Items?.Take(matches).Select(FullTrackExtensions.ToTrackInfo).ToList() ?? Array.Empty<SpotifyTrackInfo>();
		}

		private static string BuildStructuredQuery(string title, string artist, string? album)
		{
			var sb = new StringBuilder();
			AppendQueryField(sb, "title", title);
			AppendQueryField(sb, "artist", artist);
			if (album != null)
			{
				AppendQueryField(sb, "album", album);
			}
			return sb.ToString();
		}

		private static void AppendQueryField(StringBuilder sb, string fieldName, string fieldValue)
		{
			sb.Append(fieldName);
			sb.Append(": \"");
			sb.Append(fieldValue);
			sb.Append("\" ");
		}
	}
}
