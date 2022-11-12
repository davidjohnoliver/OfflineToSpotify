#nullable enable

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
		private string? _playlistFilePath;
		public string? PlaylistFilePath
		{
			get => _playlistFilePath;
			set => OnValueSet(ref _playlistFilePath, value);
		}

		private string? _dbFilePath;
		public string? DBFilePath
		{
			get => _dbFilePath;
			set => OnValueSet(ref _dbFilePath, value);
		}

		public ICommand ImportPlaylistCommand { get; }
		public ICommand OpenExistingDBCommand { get; }

		public ImportPageViewModel()
		{
			ImportPlaylistCommand = SimpleCommand.Create(ImportPlaylist);
			OpenExistingDBCommand = SimpleCommand.Create(OpenExistingDB);
		}

		public void ImportPlaylist()
		{
			throw new NotImplementedException();
		}

		public void OpenExistingDB()
		{
			throw new NotImplementedException();
		}
	}
}
