using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Core.Utils
{
	public static class ListUtils
	{
		public static List<T> FromValues<T>(params T[] values) => values.ToList();
	}
}
