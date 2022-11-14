#nullable enable

using OfflineToSpotify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	internal class ShowImportsPageViewModel : ViewModelBase
	{
		private readonly PlaylistDB _playlistDB;

		private IReadOnlyList<Track>? _tracks;
		public IReadOnlyList<Track>? Tracks { get => _tracks; private set => OnValueSet(ref _tracks, value); }

		public ShowImportsPageViewModel(PlaylistDB playlistDB)
		{
			_playlistDB = playlistDB;

			SetTracks();
		}

		private async void SetTracks()
		{
			// TODO: error handling
			Tracks = await _playlistDB.GetTracks(CancellationToken.None);
		}
	}
}
