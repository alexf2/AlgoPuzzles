using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Algorithms.Implementation
{
    public class ArrayDedup : AlgoBase<ArrayDedup.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "Array")]            
            public int[] Array { get; set; } = EmptyArray;
        }

        public override string Name { get => "Array deduplication"; }

        public override string Description { get => @"Given an array of integers. Return deduplicated array preserving number orders.
<br/><pre>Example: [1, 122, 15, 8, 1, 87, 122, 1] --> [1, 122, 15, 8, 87]</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new { Array = TestDedup.Dedup(input.Array) };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Array = new[]{1, 122, 15, 8, 1, 87, 122, 1} },
                new Args(){Array = new[]{1} },
                new Args(){Array = new int[]{}}
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }    

    static class TestDedup
    {
        public static int[] Dedup(int[] arr)
        {
            if (arr == null || arr.Length < 2)
                return arr;

            var result = new List<int>(arr.Length);
            var contentSet = new HashSet<int>();
            foreach (var i in arr)
                if (!contentSet.Contains(i))
                {
                    contentSet.Add(i);
                    result.Add(i);
                }

            return result.ToArray();
        }
    }
}
