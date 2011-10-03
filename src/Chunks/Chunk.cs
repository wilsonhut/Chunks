using System.Collections;
using System.Collections.Generic;

namespace Wilsonhut.Chunks
{
	public class Chunk<T> : IEnumerable<T>
	{
		private readonly IEnumerable<T> _chunk;

		public Chunk(IEnumerable<T> chunk, int first, int length)
		{
			_chunk = chunk;
			FirstIndex = first;
			Length = length;
		}

		public int FirstIndex { get; private set; }
		public int Length { get; private set; }
		public int LastIndex { get { return FirstIndex + Length - 1; } }

		public IEnumerator<T> GetEnumerator()
		{
			return _chunk.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _chunk.GetEnumerator();
		}
	}
}
