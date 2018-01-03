using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Algorithms.Implementation
{
    public class HeapPermutations : AlgoBase<HeapPermutations.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "Numbers")]
            public IList<int> Array { get; set; } = EmptyArray;
        }

        public override string Name { get => "Heap array permutation"; }

        public override string Description { get => @"Given an array of unique integers. Return all permutations.
<br/><pre>Example: Array = [1, 2, 3] -->
[
  '1, 2, 3',
  '1, 3, 2',
  '2, 1, 3',
  '2, 3, 1',
  '3, 1, 2',
  '3, 2, 1'
]
</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            var res = TestPermutations.Permute(input.Array);
            return new { NumberOfPermutations = res.Count, Array = res };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Array = new[]{1, 2, 3} },
                new Args(){Array = new[]{11, -1, 10, 99} },
                new Args(){Array = new int[]{1}},
                new Args(){Array = new int[]{}}
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class TestPermutations
    {
        public static IList<IList<int>> Permute (IList<int> nums)
        {
            return nums == null ? null:HeadPermutate(nums, nums.Count, nums.Count).ToArray();
        }

        public static IEnumerable<int[]> HeadPermutate(IList<int> list, int size, int n)
        {
            if (size == 1)
                yield return list.Take(n).ToArray();

            for (int i = 0; i < size; ++i)
            {
                foreach (var item in HeadPermutate(list, size - 1, n))
                    yield return item;

                if (size % 2 == 1)
                    (list[0], list[size - 1]) = (list[size - 1], list[0]);
                else
                    (list[i], list[size - 1]) = (list[size - 1], list[i]);
            }
        }
    }
}
