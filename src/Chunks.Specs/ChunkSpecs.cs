using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Wilsonhut.Chunks;

namespace Chunks.Specs
{
	public class when_making_chunks
	{
		private It should_defer_execution =
			() => Catch.Exception(() => Enumerable.Range(0, 1).Select(x => 1/x).ToChunks(2)).ShouldBeNull();

		private It should_only_enumerate_once =
			() => Catch.Exception(() => new OneShotEnumerable<int>(Enumerable.Range(0, 33)).ToChunks(2).ToList()).ShouldBeNull();
	}

	public class when_making_non_chunks
	{
		private It should_defer_execution =
			() => Catch.Exception(() => Enumerable.Range(0, 1).Select(x => 1 / x).Chunked(2)).ShouldBeNull();

		private It should_only_enumerate_once =
			() => Catch.Exception(() => new OneShotEnumerable<int>(Enumerable.Range(0, 33)).Chunked(2).ToList()).ShouldBeNull();
	}

	public class when_to_chunking
	{
		private static List<Chunk<int>> _actualChunks;
		private static readonly IEnumerable<int> _list = Enumerable.Range(0, 33);

		private Because of = () => _actualChunks = _list.ToChunks(10).ToList();

		private It should_make_correct_number_of_chunks = () => _actualChunks.Count.ShouldEqual(4);

		private It should_have_correct_first_chunk =
			() => _actualChunks.First().ToArray().ShouldEqual(Enumerable.Range(0, 10).ToArray());

		private It should_have_correct_second_chunk =
			() => _actualChunks.Skip(1).First().ToArray().ShouldEqual(Enumerable.Range(10, 10).ToArray());

		private It should_have_correct_last_chunk = () => _actualChunks.Last().ToArray().ShouldEqual(new[] {30, 31, 32});

		private It should_have_correct_first_chunk_count = () => _actualChunks.First().Length.ShouldEqual(10);

		private It should_have_correct_last_chunk_count = () => _actualChunks.Last().Length.ShouldEqual(3);

		private It should_have_correct_first_chunk_first_index = () => _actualChunks.First().FirstIndex.ShouldEqual(0);

		private It should_have_correct_last_chunk_first_index = () => _actualChunks.Last().FirstIndex.ShouldEqual(30);

		private It should_have_correct_first_chunk_last_index = () => _actualChunks.First().LastIndex.ShouldEqual(9);

		private It should_have_correct_last_chunk_last_index = () => _actualChunks.Last().LastIndex.ShouldEqual(32);
	}

	public class when_chunked
	{
		private static List<IEnumerable<int>> _actualChunks;
		private static readonly IEnumerable<int> _list = Enumerable.Range(0, 33);

		private Because of = () => _actualChunks = _list.Chunked(10).ToList();

		private It should_make_correct_number_of_chunks = () => _actualChunks.Count.ShouldEqual(4);

		private It should_have_correct_first_chunk =
			() => _actualChunks.First().ToArray().ShouldEqual(Enumerable.Range(0, 10).ToArray());

		private It should_have_correct_second_chunk =
			() => _actualChunks.Skip(1).First().ToArray().ShouldEqual(Enumerable.Range(10, 10).ToArray());

		private It should_have_correct_last_chunk = () => _actualChunks.Last().ToArray().ShouldEqual(new[] { 30, 31, 32 });

		private It should_have_correct_first_chunk_count = () => _actualChunks.First().Count().ShouldEqual(10);

		private It should_have_correct_last_chunk_count = () => _actualChunks.Last().Count().ShouldEqual(3);
	}
}