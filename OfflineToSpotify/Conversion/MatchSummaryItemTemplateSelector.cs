using Microsoft.UI.Xaml;
using OfflineToSpotify.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Conversion
{
	internal class MatchSummaryItemTemplateSelector : TemplateSelector<MatchSummaryItemViewModel>
	{
		public DataTemplate EllipsisTemplate { get; set; }
		public DataTemplate MatchedItemTemplate { get; set; }
		public DataTemplate UnmatchedItemTemplate { get; set; }

		protected override DataTemplate SelectTemplateInner(MatchSummaryItemViewModel item) => item switch
		{
			MatchSummaryItemViewModel.Ellipsis => EllipsisTemplate,
			MatchSummaryItemViewModel.MatchedTrack => MatchedItemTemplate,
			MatchSummaryItemViewModel.UnmatchedTrack => UnmatchedItemTemplate,
			_ => throw new InvalidOperationException("Unknown item type")
		};
	}
}
