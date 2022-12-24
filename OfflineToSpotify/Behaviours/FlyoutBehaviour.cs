using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using OfflineToSpotify.Presentation;
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


		public static FlyoutBase GetSetAsDismissible(DependencyObject obj)
		{
			return (FlyoutBase)obj.GetValue(SetAsDismissibleProperty);
		}

		public static void SetSetAsDismissible(DependencyObject obj, FlyoutBase value)
		{
			obj.SetValue(SetAsDismissibleProperty, value);
		}

		// Using a DependencyProperty as the backing store for SetAsDismissible.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SetAsDismissibleProperty =
			DependencyProperty.RegisterAttached("SetAsDismissible", typeof(FlyoutBase), typeof(FlyoutBehaviour), new PropertyMetadata(null, OnSetAsDismissibleChanged));

		private static void OnSetAsDismissibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var flyoutChild = (FrameworkElement)d;

			if (e.NewValue is FlyoutBase flyout)
			{
				flyoutChild.DataContextChanged += FlyoutChild_DataContextChanged;
				;
				void FlyoutChild_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
				{
					if (args.NewValue is IReceiver<IDismissibleContainer> receiver)
					{
						receiver.Set(new FlyoutDismissibleContainer(flyout));
					}
				}
			}
		}

	}
}
