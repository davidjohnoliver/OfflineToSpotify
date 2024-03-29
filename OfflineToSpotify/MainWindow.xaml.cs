﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OfflineToSpotify.Presentation;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OfflineToSpotify;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
	public static bool IsActive { get; private set; }

	public MainWindow()
	{
		this.InitializeComponent();

		Activated += MainWindow_Activated;
	}

	private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
	{
		IsActive = args.WindowActivationState switch
		{
			WindowActivationState.PointerActivated => true,
			WindowActivationState.CodeActivated => true,
			WindowActivationState.Deactivated => false,
			_ => true
		};
	}
}
