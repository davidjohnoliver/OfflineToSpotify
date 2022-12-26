using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Core.Extensions
{
	public static class ListExtensions
	{
		public static bool ContainsIndex<T>(this IList<T> list, int index) => index >= 0 && index < list.Count;
	}
}
