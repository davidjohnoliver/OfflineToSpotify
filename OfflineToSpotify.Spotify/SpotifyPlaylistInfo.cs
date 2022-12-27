using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Spotify
{
	public sealed class SpotifyPlaylistInfo
	{
		internal string Id { get; }

		internal SpotifyPlaylistInfo(string id)
		{
			Id = id;
		}

	}
}
