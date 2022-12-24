using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public interface IReceiver<T>
	{
		void Set(T value);
	}
}
