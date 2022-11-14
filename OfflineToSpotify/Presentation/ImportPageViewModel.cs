#nullable enable

using OfflineToSpotify.Import;
using OfflineToSpotify.Model;
using OfflineToSpotify.Pages;
using OfflineToSpotify.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfflineToSpotify.Presentation
{
	class ImportPageViewModel : ViewModelBase
	{
		private const string PlaylistFilePathSettingsKey = "ImportPage_PlaylistFilePath";
		private const string DBFilePathSettingsKey = "ImportPage_DBFilePath";

		private string _playlistFilePath = string.Empty;
		public string PlaylistFilePath
		{
			get => _playlistFilePath;
			set => OnValueSet(ref _playlistFilePath, value);
		}

		private string? _dbFilePath;
		private readonly Navigator<PlaylistDB> _navigator;

		public string? DBFilePath
		{
			get => _dbFilePath;
			set => OnValueSet(ref _dbFilePath, value);
		}

		public ICommand ImportPlaylistCommand { get; }
		public ICommand OpenExistingDBCommand { get; }

		public ImportPageViewModel(Navigator<PlaylistDB> navigator)
		{
			ImportPlaylistCommand = SimpleCommand.Create(ImportPlaylist);
			OpenExistingDBCommand = SimpleCommand.Create(OpenExistingDB);
			_navigator = navigator;

			LoadSettings();
		}

		private async void ImportPlaylist()
		{
			if (string.IsNullOrWhiteSpace(PlaylistFilePath))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(DBFilePath))
			{
				return;
			}

			var tracks = ImportUtil.ImportPlaylist(PlaylistFilePath);
			var db = await PlaylistDB.InitializeFromImport(tracks, DBFilePath);

			SaveSettings();

			_navigator.Navigate<ShowImportsPage>(db);
		}

		private void LoadSettings()
		{
			DBFilePath = SettingsUtil.GetSavedValue<string>(DBFilePathSettingsKey);
			PlaylistFilePath = SettingsUtil.GetSavedValue<string>(PlaylistFilePathSettingsKey);
		}

		private void SaveSettings()
		{
			SettingsUtil.SaveValue(DBFilePathSettingsKey, DBFilePath);
			SettingsUtil.SaveValue(PlaylistFilePathSettingsKey, PlaylistFilePath);
		}

		private void OpenExistingDB()
		{
			if (string.IsNullOrWhiteSpace(DBFilePath))
			{
				return;
			}

			var db = PlaylistDB.LoadFromDisk(DBFilePath);

			SaveSettings();

			_navigator.Navigate<ShowImportsPage>(db);
		}
	}
}
