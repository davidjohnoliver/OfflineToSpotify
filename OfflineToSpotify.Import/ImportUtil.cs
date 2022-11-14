using OfflineToSpotify.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Import;

public static class ImportUtil
{
	public static IEnumerable<(TrackInfo TrackInfo, string FilePath)> ImportPlaylist(string playlistPath)
	{
		foreach (var line in File.ReadLines(playlistPath))
		{
			yield return (ImportTrack(line), line);
		}
	}

	private static TrackInfo ImportTrack(string trackPath)
	{
		using var tf = TagLib.File.Create(trackPath);

		return new TrackInfo(tf.Tag.FirstPerformer, tf.Tag.Title, tf.Tag.Album);
	}
}
