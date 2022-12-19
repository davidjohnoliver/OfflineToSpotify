using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<(T Value, int Index)> Indexed<T>(this IEnumerable<T> collection)
			=> collection.Select((t, i) => (t, i));
	}
}
