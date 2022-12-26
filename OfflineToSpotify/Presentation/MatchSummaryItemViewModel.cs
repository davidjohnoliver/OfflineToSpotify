using OfflineToSpotify.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public abstract class MatchSummaryItemViewModel
	{

		public sealed class Ellipsis : MatchSummaryItemViewModel
		{
		}

		public abstract class TrackItem : MatchSummaryItemViewModel
		{
			public int Index0Based { get; init; }

			public int Index => Index0Based + 1;

			public TrackInfo Info { get; init; }
		}

		public sealed class UnmatchedTrack : TrackItem { }

		public sealed class MatchedTrack : TrackItem { }
	}
}
