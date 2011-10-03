using System;
using System.Collections.Generic;

namespace Wilsonhut.Chunks
{
	public static class Extensions
	{
		public static IEnumerable<IEnumerable<T>> Chunked<T>(this IEnumerable<T> list, int chunkSize)
		{
			var enumerator = list.GetEnumerator();

			for (;;)
			{
				var chunk = enumerator.GetNext(chunkSize);
				if (chunk.Length == 0)
				{
					break;
				}
				yield return chunk;
			}
		}

		public static IEnumerable<Chunk<T>> ToChunks<T>(this IEnumerable<T> list, int chunkSize)
		{
			var enumerator = list.GetEnumerator();

			for (var i = 0;; i++)
			{
				var chunk = enumerator.GetNext(chunkSize);
				if (chunk.Length == 0)
				{
					break;
				}
				yield return new Chunk<T>(chunk, i*chunkSize, chunk.Length);
			}
		}

		private static T[] GetNext<T>(this IEnumerator<T> enumerator, int count)
		{
			var ts = new T[count];
			int i;
			for (i = 0; i < count; i++)
			{
				if (!enumerator.MoveNext()) break;
				ts[i] = enumerator.Current;
			}
			if (i < count)
			{
				Array.Resize(ref ts, i);
			}
			return ts;
		}
	}
}