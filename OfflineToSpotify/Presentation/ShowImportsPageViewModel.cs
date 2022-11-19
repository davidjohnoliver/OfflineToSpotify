#nullable enable

using OfflineToSpotify.Model;
using OfflineToSpotify.Pages;
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
		private readonly Navigator<PlaylistDB> _navigator;
		private IReadOnlyList<Track>? _tracks;
		public IReadOnlyList<Track>? Tracks { get => _tracks; private set => OnValueSet(ref _tracks, value); }

		public ShowImportsPageViewModel(PlaylistDB playlistDB, Navigator<PlaylistDB> navigator)
		{
			_playlistDB = playlistDB;
			_navigator = navigator;
			SetTracks();
		}

		private async void SetTracks()
		{
			// TODO: error handling
			Tracks = await _playlistDB.GetTracks(CancellationToken.None);
		}

		public void OnContinue() => _navigator.Navigate<SpotifyMatchesPage>(_playlistDB);
	}
}
