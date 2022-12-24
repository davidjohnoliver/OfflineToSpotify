using LiteDB;
using OfflineToSpotify.Core.Extensions;
using OfflineToSpotify.Spotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Model
{
	public sealed class CachedMatchesDB : IDisposable
	{
		private readonly LiteDatabase _liteDatabase;

		private readonly ILiteCollection<CachedMatch> _matchCollection;

		private Dictionary<string, CachedMatch>? _matches;

		private IDictionary<string, CachedMatch> Matches => _matches ?? throw new InvalidOperationException("Not initialized");

		private CachedMatchesDB(string dbPath)
		{
			_liteDatabase = new LiteDatabase(dbPath);
			_matchCollection = _liteDatabase.GetCollection<CachedMatch>();
		}

		internal SpotifyTrackInfo? GetMatch(string importPath) => Matches.GetOrDefault(importPath)?.Match;

		internal async Task UpdateMatch(string importPath, SpotifyTrackInfo confirmedMatch)
		{
			if (Matches.TryGetValue(importPath, out var cachedMatch))
			{
				if (cachedMatch.Match == confirmedMatch)
				{
					// Nothing to update
					return;
				}
				else
				{
					cachedMatch.Match = confirmedMatch;

					await Task.Run(() =>
					{
						_matchCollection.Update(cachedMatch);
					});
				}
			}
			else
			{
				var newMatch = new CachedMatch { ImportPath = importPath, Match = confirmedMatch };
				Matches[importPath] = newMatch;

				await Task.Run(() =>
				{
					_matchCollection.Insert(newMatch);
				});
			}
		}

		public void Dispose()
		{
			_liteDatabase.Dispose();
		}

		public static async Task<CachedMatchesDB> Initialize(string dbPath)
		{
			var db = new CachedMatchesDB(dbPath);

			await Task.Run(() =>
			{
				db._matches = db._matchCollection.FindAll().ToDictionary(m => m.ImportPath ?? throw new InvalidOperationException("No import path for match"));
			});

			return db;
		}
	}
}
