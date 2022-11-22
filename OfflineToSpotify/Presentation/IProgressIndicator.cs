using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfflineToSpotify.Presentation
{
	public interface IProgressIndicator
	{
		public IDisposable ShowIndicator();
	}
}
