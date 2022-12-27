#nullable enable

using OfflineToSpotify.Core.Extensions;
using OfflineToSpotify.Model;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public class MatchSummaryPageViewModel
	{
		private readonly Track[] _allTracks;
		private readonly IProgressIndicator _progressIndicator;

		public int TotalCount => _allTracks.Length;

		public int MatchedCount =>
			_allTracks.Count(IsMatched);

		public int UnmatchedCount =>
			_allTracks.Count(t => !IsMatched(t));

		public IList<MatchSummaryItemViewModel>? Items { get; private set; }

		public string? SpotifyToken
		{
			get;
			set;
		}

		public MatchSummaryPageViewModel(Track[] allTracks, IProgressIndicator progressIndicator)
		{
			_allTracks = allTracks;
			_progressIndicator = progressIndicator;
			InitializeItems();
		}

		private void InitializeItems()
		{
			var items = new List<MatchSummaryItemViewModel>();
			for (int i = 0; i < _allTracks.Length; i++)
			{
				var current = _allTracks[i];
				if (!IsMatched(current))
				{
					if (_allTracks.ContainsIndex(i - 1) && !LastMatchesIndex(i - 1))
					{
						if (_allTracks.ContainsIndex(i - 2) && !LastMatchesIndex(i - 2) && items.Count > 0)
						{
							items.Add(new MatchSummaryItemViewModel.Ellipsis());
						}

						var leftBookend = _allTracks[i - 1];
						Debug.Assert(IsMatched(leftBookend));
						items.Add(new MatchSummaryItemViewModel.MatchedTrack { Index0Based = i - 1, Info = leftBookend.TrackInfo });
					}

					items.Add(new MatchSummaryItemViewModel.UnmatchedTrack { Index0Based = i, Info = current.TrackInfo });

					if (_allTracks.ContainsIndex(i + 1) && IsMatched(_allTracks[i + 1]))
					{
						var rightBookend = _allTracks[i + 1];
						items.Add(new MatchSummaryItemViewModel.MatchedTrack { Index0Based = i + 1, Info = rightBookend.TrackInfo });
					}
				}
			}

			Items = items;

			bool LastMatchesIndex(int i)
			{
				return items.Count > 0 && items.Last() is MatchSummaryItemViewModel.TrackItem ti && ti.Index0Based == i;
			}
		}

		public async void ExportToSpotify()
		{
			if (string.IsNullOrWhiteSpace(SpotifyToken))
			{
				return;
			}

			var helper = new PlaylistHelper(new(SpotifyToken));

			using (_progressIndicator.ShowIndicator())
			{
				try
				{
					var playlist = await helper.CreatePlaylist($"{DateTime.Now:yyyyMMdd}_Imported");

					await helper.AddTracksToPlaylist(playlist, _allTracks.Where(IsMatched).Select(t => t.CandidateMatch));
				}
				catch (Exception e)
				{
					throw;
				}
			}
		}

		private static bool IsMatched(Track track) => track.CandidateMatch != SpotifyTrackInfo.NonexistentTrack;
	}
}
