#nullable enable

using OfflineToSpotify.Core.Extensions;
using OfflineToSpotify.Model;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfflineToSpotify.Presentation
{
	sealed class SpotifyMatchesPageViewModel : ViewModelBase
	{
		private const int PageSize = 20;
		private readonly IProgressIndicator _progressIndicator;
		private readonly PlaylistDB _playlistDB;
		private readonly SearchHelper _searchHelper;

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
					LaunchUpdateCurrentTracks();
				}
			}
		}

		private int MinPage => 0;
		public int MaxPage => (_allTracks?.Length - 1) / PageSize ?? 0;

		private SimpleCommand<object> _skipToUnmatchedCommand;
		public ICommand SkipToUnmatchedCommand => _skipToUnmatchedCommand;

		public SpotifyMatchesPageViewModel(IProgressIndicator progressIndicator, PlaylistDB playlistDB, string token)
		{
			_progressIndicator = progressIndicator;
			_playlistDB = playlistDB;
			_searchHelper = new(new(token));
			_skipToUnmatchedCommand = SimpleCommand.Create(SkipToUnmatched);

			Initialize();
		}

		private async void Initialize()
		{
			// TODO: error handling
			_allTracks = (await _playlistDB.GetTracks(CancellationToken.None)).ToArray();
			var _ = 0;
			OnValueSet(ref _, 1, nameof(MaxPage)); // Hacky, but it works
			await UpdateCurrentTracks();
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

		public async void ConfirmMatchesAndIncrementPage()
		{
			await ConfirmAllMatchesInPage();

			// Give UI a chance to update
			await Task.Delay(200);

			IncrementPage();
		}

		private async Task ConfirmAllMatchesInPage()
		{
			using (_progressIndicator.ShowIndicator())
			{
				foreach (var track in _currentTracks)
				{
					track.IsMatchConfirmed = true;
				}

				await TrackManager.UpdateTracks(_currentTracks.Select(tvm => tvm.Track), _playlistDB);
			}
		}

		private void SkipToUnmatched()
		{
			var firstUnmatched = _allTracks?.Indexed().SkipWhile(tpl => tpl.Value.IsCandidateConfirmed).FirstOrDefault().Index;
			if (firstUnmatched is not { } firstUnmatchedValue)
			{
				return;
			}

			var firstPageWithUnmatched = firstUnmatchedValue / PageSize;

			CurrentPage = firstPageWithUnmatched;
		}

		private async void LaunchUpdateCurrentTracks()
		{
			await UpdateCurrentTracks();
		}

		private async Task UpdateCurrentTracks()
		{
			if (_allTracks is null)
			{
				return;
			}
			_currentTracks.Clear();
			var currentTracks = _allTracks[(CurrentPage * PageSize)..((CurrentPage + 1) * PageSize)];
			foreach (var track in currentTracks)
			{
				using (_progressIndicator.ShowIndicator())
				{
					await TrackManager.UpdateMissingMatches(track, _playlistDB, _searchHelper, 3);
					// Consider optimized observable collection implementation w/ AddRange if this is too slow
					_currentTracks.Add(new(track, _playlistDB, _searchHelper, _progressIndicator));
				}
			}
		}
	}
}
