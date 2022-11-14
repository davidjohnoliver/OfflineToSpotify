using LiteDB;
using OfflineToSpotify.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Model
{
	public sealed class PlaylistDB : IDisposable
	{
		private readonly LiteDatabase _liteDatabase;

		private readonly ILiteCollection<Track> _trackCollection;

		private PlaylistDB(string dbPath)
		{
			_liteDatabase = new LiteDatabase(dbPath);
			_trackCollection = _liteDatabase.GetCollection<Track>();
		}

		public async Task<IReadOnlyList<Track>> GetTracks(CancellationToken ct)
		{
			IReadOnlyList<Track>? tracks = null;
			await Task.Run(() =>
			{
				tracks = _trackCollection.FindAll().ToImmutableList();
			}, ct);

			return tracks ?? throw new InvalidOperationException();
		}

		public void Dispose()
		{
			_liteDatabase.Dispose();
		}

		public static async Task<PlaylistDB> InitializeFromImport(IEnumerable<(TrackInfo TrackInfo, string FilePath)> importedTracks, string dbPath)
		{
			var playlistDB = new PlaylistDB(dbPath);
			await Task.Run(() =>
			{
				// Clear db if file already existed
				playlistDB._trackCollection.DeleteMany(_ => true);

				foreach (var tuple in importedTracks)
				{
					var track = new Track
					{
						TrackInfo = tuple.TrackInfo,
						ImportPath = tuple.FilePath
					};
					playlistDB._trackCollection.Insert(track);
				}
			});

			return playlistDB;
		}

		public static PlaylistDB LoadFromDisk(string dbPath) => new PlaylistDB(dbPath);
	}
}
