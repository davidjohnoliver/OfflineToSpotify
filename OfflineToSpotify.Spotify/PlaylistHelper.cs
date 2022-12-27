using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Spotify
{
	public sealed class PlaylistHelper
	{
		private readonly SpotifyClient _client;

		public PlaylistHelper(SpotifyClient client)
		{
			_client = client;
		}

		public async Task<SpotifyPlaylistInfo> CreatePlaylist(string playlistName)
		{
			var user = await _client.UserProfile.Current();
			var playlist = await _client.Playlists.Create(user.Id, new PlaylistCreateRequest(playlistName));

			return new SpotifyPlaylistInfo(playlist.Id ?? throw new InvalidOperationException("Playlist was not created"));
		}

		public async Task AddTracksToPlaylist(SpotifyPlaylistInfo playlist, IEnumerable<SpotifyTrackInfo> tracks)
		{
			var tracksToAdd = new Queue<SpotifyTrackInfo>(tracks);

			var pendingTracks = new List<string>();

			while (tracksToAdd.Count > 0)
			{
				var track = tracksToAdd.Dequeue();
				pendingTracks.Add($"spotify:track:{track.TrackId}");

				if (pendingTracks.Count == 100)
				{
					await _client.Playlists.AddItems(playlist.Id, new(pendingTracks));
					pendingTracks.Clear();
				}
			}

			if (pendingTracks.Count > 0)
			{
				await _client.Playlists.AddItems(playlist.Id, new(pendingTracks));
			}
		}
	}
}
