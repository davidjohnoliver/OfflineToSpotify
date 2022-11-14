using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	internal class Navigator<TParam>
	{
		private readonly Page _page;

		public Navigator(Page page)
		{
			_page = page;
		}

		public bool Navigate<TPage>(TParam parameter) where TPage : Page
			=> _page.Frame.Navigate(typeof(TPage), parameter);
	}
}
