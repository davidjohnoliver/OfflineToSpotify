using Microsoft.UI.Xaml;
using OfflineToSpotify.Core.Disposables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public class VisibilityProgressIndicator : IProgressIndicator
	{
		private readonly FrameworkElement _indicatorElement;

		public VisibilityProgressIndicator(FrameworkElement indicatorElement)
		{
			_indicatorElement = indicatorElement;
		}
		public IDisposable ShowIndicator()
		{
			_indicatorElement.Visibility = Visibility.Visible;

			return Disposable.Create(() => _indicatorElement.Visibility = Visibility.Collapsed);
		}
	}
}
