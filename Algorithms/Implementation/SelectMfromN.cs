using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Algorithms.Implementation
{
    public class SelectMfromN : AlgoBase<SelectMfromN.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "Array")]
            public int[] Array { get; set; } = EmptyArray;
            public int M { get; set; }
        }

        public override string Name { get => "Selecting M from N"; }

        public override string Description { get => @"Given an Array of unique integers of N size and a whole positive number M. M <= N. Return all possible unique selections of M items from the array.
<br/><pre>Example: Array = [1, 2, 3, 4, 5], M = 4 -->
[
  '1, 2, 3, 4',
  '1, 2, 3, 5',
  '1, 2, 4, 5',
  '1, 3, 4, 5',
  '2, 3, 4, 5'
]
</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            var res = SelectMfromNImpl.Combinations(input.Array, input.M).ToArray();
            return new { NumberOfSelections = res?.Length,  Selections = res};
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 6},
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 5},
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 4},
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 3},
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 2},
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 1},
                new Args(){Array = new[]{ 1, 2, 3, 4, 5 }, M = 0}
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class SelectMfromNImpl
    {
        static bool NextCombination(int[] num, int n, int k)
        {
            bool finished = false, changed = false;            

            if (k > 0)
            {
                for (var i = k - 1; !finished && !changed; i--)
                {
                    if (num[i] < (n - 1) - (k - 1) + i)
                    {
                        num[ i ]++;
                        if (i < k - 1)
                        {
                            for (var j = i + 1; j < k; j++)
                            {
                                num[j] = num[j - 1] + 1;
                            }
                        }
                        changed = true;
                    }
                    finished = (i == 0);
                }
            }

            return changed;
        }

        public static IEnumerable<T[]> Combinations<T>(IEnumerable<T> elements, int k)
        {
            var elem = elements.ToArray();
            var size = elem.Length;

            if (k <= size)
            {                
                var numbers = Enumerable.Range(0, k).ToArray();

                do
                {
                    yield return numbers.Select(n => elem[n]).ToArray();
                }
                while (NextCombination(numbers, size, k));
            }
        }
    }

}
