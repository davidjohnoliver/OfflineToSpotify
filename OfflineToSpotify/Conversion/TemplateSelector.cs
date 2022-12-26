using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Conversion
{
	public abstract class TemplateSelector<T> : DataTemplateSelector
	{
		protected override sealed DataTemplate SelectTemplateCore(object item) => SelectTemplatePrivate(item);

		protected override sealed DataTemplate SelectTemplateCore(object item, DependencyObject container)
			=> SelectTemplatePrivate(item);

		private DataTemplate SelectTemplatePrivate(object obj) => obj switch
		{
			T item => SelectTemplateInner(item),
			_ => SelectDefaultTemplate()
		};

		protected abstract DataTemplate SelectTemplateInner(T item);

		protected virtual DataTemplate SelectDefaultTemplate() => null;
	}
}
