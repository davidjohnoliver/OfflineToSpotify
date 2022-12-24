#nullable enable

using OfflineToSpotify.Model;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfflineToSpotify.Presentation
{
	sealed class TrackItemViewModel : ViewModelBase
	{
		public Track Track { get; }

		public string? Artist => Track.TrackInfo?.Artist;

		public string? Title => Track.TrackInfo?.Title;

		public string? Album => Track.TrackInfo?.Album;

		public bool IsMatchConfirmed
		{
			get => Track.IsCandidateConfirmed;
			set => OnValueSet(Track.IsCandidateConfirmed, t => Track.IsCandidateConfirmed = t, value);
		}

		private bool _shouldIncludeNoMatch;

		private IEnumerable<MatchItemViewModel>? _matches;
		public IEnumerable<MatchItemViewModel> Matches
		{
			get => _matches!;
			set => OnValueSet(ref _matches, value);
		}

		private MatchItemViewModel? _candidateMatch;
		public MatchItemViewModel? CandidateMatch
		{
			get => _candidateMatch;
			set
			{
				if (value == null)
				{
					// Disallow null values from the UI layer
					return;
				}
				if (OnValueSet(ref _candidateMatch, value))
				{
					IsMatchConfirmed = true;
					Track.CandidateMatch = value?.Info ?? SpotifyTrackInfo.NonexistentTrack;
					UpdateDB();
				}
			}
		}

		public TrackItemFlyoutViewModel FlyoutContext { get; }

		private readonly PlaylistDB _playlistDB;
		private readonly CachedMatchesDB _cachedMatchesDB;
		private readonly SearchHelper _searchHelper;
		private readonly IProgressIndicator _progressIndicator;

		public TrackItemViewModel(Track track, PlaylistDB playlistDB, CachedMatchesDB cachedMatchesDB, SearchHelper searchHelper, IProgressIndicator progressIndicator)
		{
			Track = track;
			_playlistDB = playlistDB;
			_cachedMatchesDB = cachedMatchesDB;
			_searchHelper = searchHelper;
			_progressIndicator = progressIndicator;
			UpdateMatches();

			_candidateMatch = Matches.FirstOrDefault(m => m.Info == track.CandidateMatch);

			FlyoutContext = new(this);
		}

		private void UpdateMatches()
		{
			if (Track.SpotifyMatches.Count == 0)
			{
				_shouldIncludeNoMatch = true;
			}
			if (Track.IsCandidateConfirmed && Track.CandidateMatch == SpotifyTrackInfo.NonexistentTrack && !Track.SpotifyMatches.Contains(SpotifyTrackInfo.NonexistentTrack))
			{
				_shouldIncludeNoMatch = true;
			}

			var matchVMs = Track.SpotifyMatches.Select(m => new MatchItemViewModel(m));
			Matches = (_shouldIncludeNoMatch ? new[] { MatchItemViewModel.NoMatch }.Concat(matchVMs) : matchVMs).ToArray();
		}

		private async void UpdateDB()
		{
			using (_progressIndicator.ShowIndicator())
			{
				await TrackManager.UpdateTrack(Track, _playlistDB, _cachedMatchesDB);
			}
		}

		private void SetUserMatch(SpotifyTrackInfo? match)
		{
			MatchItemViewModel? matchModel;
			if (match == null)
			{
				if (!Matches.Contains(MatchItemViewModel.NoMatch))
				{
					_shouldIncludeNoMatch = true;
					UpdateMatches();
				}

				matchModel = MatchItemViewModel.NoMatch;
			}
			else
			{
				if (!Track.SpotifyMatches.Contains(match))
				{
					Track.SpotifyMatches.Insert(0, match);
					UpdateMatches();
				}

				matchModel = Matches.FirstOrDefault(m => m.Info == match);
			}

			IsMatchConfirmed = true;

			// Relying on CandidateMatch setter to update DB with changes to SpotifyMatches & IsMatchConfirmed, which is not best practice
			CandidateMatch = matchModel;
		}

		private void SetNoExistingMatch() => SetUserMatch(null);

		public class MatchItemViewModel
		{
			public SpotifyTrackInfo? Info { get; }

			public MatchItemViewModel(SpotifyTrackInfo spotifyTrackInfo)
			{
				Info = spotifyTrackInfo;
			}

			public static MatchItemViewModel NoMatch { get; } = new(SpotifyTrackInfo.NonexistentTrack);
		}

		public class TrackItemFlyoutViewModel : ViewModelBase, IReceiver<IDismissibleContainer>
		{
			private string? _manualSongLink;
			private readonly TrackItemViewModel _parent;

			private static readonly Regex _idMatcher = new("track\\/([^\\?]+)");

			public string? Title => _parent.Title;

			public string? ManualSongLink
			{
				get => _manualSongLink;
				set
				{
					if (OnValueSet(ref _manualSongLink, value) && !string.IsNullOrEmpty(_manualSongLink))
					{
						SearchManual(_manualSongLink);
					}
				}
			}

			public SpotifyTrackInfo? _infoFromLink;
			public SpotifyTrackInfo? InfoFromLink
			{
				get => _infoFromLink;
				set => OnValueSet(ref _infoFromLink, value);
			}

			private SimpleCommand<object> _confirmMatch;
			public ICommand ConfirmMatch => _confirmMatch;

			public ICommand ConfirmNoMatchingSong { get; }

			private IDismissibleContainer? _dismissibleContainer;

			public TrackItemFlyoutViewModel(TrackItemViewModel parent)
			{
				_parent = parent;

				_confirmMatch = SimpleCommand.Create(() =>
				{
					_parent.SetUserMatch(InfoFromLink);
					_dismissibleContainer?.Dismiss();
				});
				_confirmMatch.CanExecute = false;
				ConfirmNoMatchingSong = SimpleCommand.Create(() =>
				{
					_parent.SetNoExistingMatch();
					_dismissibleContainer?.Dismiss();
				});
			}

			private async void SearchManual(string songLink)
			{
				try
				{
					using (_parent._progressIndicator.ShowIndicator())
					{
						var id = GetIDFromSongLink(songLink);
						InfoFromLink = await _parent._searchHelper.FindTrackById(id);
						_confirmMatch.CanExecute = true;
					}
				}
				catch
				{
					// TODO: log or report error
				}
			}

			private static string GetIDFromSongLink(string songLink)
			{
				var match = _idMatcher.Match(songLink);
				if (match.Groups.Count > 1)
				{
					return match.Groups[1].Value;
				}

				// Well perhaps it was just an id, we can give it a try as-is
				return songLink;
			}

			public void Set(IDismissibleContainer value)
			{
				_dismissibleContainer = value;
			}
		}
	}
}
