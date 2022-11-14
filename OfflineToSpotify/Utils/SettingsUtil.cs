using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OfflineToSpotify.Utils
{
	public static class SettingsUtil
	{
		public static T GetSavedValue<T>(string key)
		{
			if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var value))
			{
				return (T)value;
			}

			return default;
		}

		public static void SaveValue<T>(string key, T value)
		{
			ApplicationData.Current.LocalSettings.Values[key] = value;
		}
	}
}
