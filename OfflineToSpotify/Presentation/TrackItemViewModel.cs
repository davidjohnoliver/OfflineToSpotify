using OfflineToSpotify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	sealed class TrackItemViewModel : ViewModelBase
	{
		public Track Track { get; }

		public TrackItemViewModel(Track track)
		{
			Track = track;
		}
	}
}
