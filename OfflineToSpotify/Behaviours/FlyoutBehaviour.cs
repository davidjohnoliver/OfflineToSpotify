using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Behaviours
{
	public static class FlyoutBehaviour
	{

		public static bool GetShouldDismissOnDeactivation(FlyoutBase obj)
		{
			return (bool)obj.GetValue(ShouldDismissOnDeactivationProperty);
		}

		public static void SetShouldDismissOnDeactivation(FlyoutBase obj, bool value)
		{
			obj.SetValue(ShouldDismissOnDeactivationProperty, value);
		}

		// Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ShouldDismissOnDeactivationProperty =
			DependencyProperty.RegisterAttached("ShouldDismissOnDeactivation", typeof(bool), typeof(FlyoutBehaviour), new PropertyMetadata(true, OnShouldDismissOnDeactivationChanged));

		private static void OnShouldDismissOnDeactivationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var flyout = (FlyoutBase)d;
			if (!(bool)e.NewValue)
			{
				flyout.Closing += OnFlyoutClosing;
			}
			else
			{
				flyout.Closing -= OnFlyoutClosing;
			}

			static void OnFlyoutClosing(FlyoutBase sender, FlyoutBaseClosingEventArgs args)
			{
				if (!MainWindow.IsActive)
				{
					args.Cancel = true;
				}
				;
			}
		}

	}
}
