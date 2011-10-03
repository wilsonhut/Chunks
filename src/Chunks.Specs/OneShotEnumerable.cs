using System;
using System.Collections;
using System.Collections.Generic;

namespace Chunks.Specs
{
	public class OneShotEnumerable<T> : IEnumerable<T>
	{
		private readonly IEnumerable<T> _list;
		private bool _beenThere;

		public OneShotEnumerable(IEnumerable<T> list)
		{
			_list = list;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (_beenThere)
			{
				throw new InvalidOperationException("Sorry champ, you've already enumerated this.");
			}
			_beenThere = true;
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}