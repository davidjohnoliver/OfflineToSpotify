using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public class FlyoutDismissibleContainer : IDismissibleContainer
	{
		private readonly FlyoutBase _flyout;

		public FlyoutDismissibleContainer(FlyoutBase flyout)
		{
			_flyout = flyout;
		}

		public void Dismiss()
		{
			_flyout.Hide();
		}
	}
}
