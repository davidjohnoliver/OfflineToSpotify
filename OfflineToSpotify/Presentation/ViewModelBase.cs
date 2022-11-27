#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	internal class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Call from a bindable property to raise the <see cref="PropertyChanged"/> event when needed.
		/// </summary>
		/// <param name="currentValue">Pass the backing field here.</param>
		/// <param name="newValue">Pass the set value here.</param>
		/// <returns>True if the value changed, false if the new value is equal to the previous.</returns>
		protected bool OnValueSet<T>(ref T currentValue, T newValue, [CallerMemberName] string? name = null)
		{
			if (!Equals(currentValue, newValue))
			{
				currentValue = newValue;
				OnPropertyChanged(name);
				return true;
			}

			return false;
		}

		protected bool OnValueSet<T>(T currentValue, Action<T> setter, T newValue, [CallerMemberName] string? name = null)
		{
			if (!Equals(currentValue, newValue))
			{
				setter(newValue);
				OnPropertyChanged(name);
				return true;
			}

			return false;
		}

		private void OnPropertyChanged(string? name)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
