#nullable enable

using OfflineToSpotify.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	sealed class SpotifyMatchesPageViewModel : ViewModelBase
	{
		private const int PageSize = 20;

		private readonly PlaylistDB _playlistDB;
		private Track[]? _allTracks;
		private readonly ObservableCollection<TrackItemViewModel> _currentTracks = new();
		public IEnumerable<TrackItemViewModel> CurrentTracks => _currentTracks;

		private int _currentPage = 0;
		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				if (OnValueSet(ref _currentPage, value))
				{
					UpdateCurrentTracks();
				}
			}
		}

		private int MinPage => 0;
		public int MaxPage => (_allTracks?.Length - 1) / PageSize ?? 0;

		public SpotifyMatchesPageViewModel(PlaylistDB playlistDB)
		{
			_playlistDB = playlistDB;

			Initialize();
		}

		private async void Initialize()
		{
			// TODO: error handling
			_allTracks = (await _playlistDB.GetTracks(CancellationToken.None)).ToArray();
			var _ = 0;
			OnValueSet(ref _, 1, nameof(MaxPage)); // Hacky, but it works
			UpdateCurrentTracks();
		}

		public void DecrementPage()
		{
			if (CurrentPage <= MinPage)
			{
				return;
			}

			CurrentPage--;
		}

		public void IncrementPage()
		{
			if (CurrentPage >= MaxPage)
			{
				return;
			}

			CurrentPage++;
		}

		private void UpdateCurrentTracks()
		{
			if (_allTracks is null)
			{
				return;
			}
			_currentTracks.Clear();
			var currentTracks = _allTracks[(CurrentPage * PageSize)..((CurrentPage + 1) * PageSize)];
			foreach (var track in currentTracks)
			{
				// Consider optimized observable collection implementation w/ AddRange if this is too slow
				_currentTracks.Add(new TrackItemViewModel(track));
			}
		}
	}
}
