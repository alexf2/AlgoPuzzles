using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Algorithms.Implementation
{
    public class TwoSumm : AlgoBase<TwoSumm.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "Array")]
            public int[] Array { get; set; } = EmptyArray;

            [Display(Name = "Target summ")]
            public int Target { get; set; } = 0;
        }

        public override string Name { get => "Two Summ"; }

        public override string Description { get => @"Given an array of integers, return indices of the two numbers which gave the target sum. You should return two different indices.
<br/><pre>Example: [2, 7, 11, 15], Target = 9 --> [0, 1]</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new { Indices = TestTwoSum.TwoSum(input.Array, input.Target) };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Array = new[]{2, 7, 11, 15}, Target = 9 },
                new Args(){Array = new[]{2, 7, 11, 15}, Target = 13 },
                new Args(){Array = new[]{2, 7, 11, 15}, Target = 12 }
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class TestTwoSum
    {
        public static int[] TwoSum(int[] nums, int target)
        {
            Dictionary<int, int> values = new Dictionary<int, int>();

            for (int i = 0; i < nums.Length; ++i)
            {
                var baseVal = target - nums[i];
                if (values.ContainsKey(baseVal))
                    return new int[] { values[baseVal], i };

                values[nums[i]] = i;
            }

            return new int[] { };
        }
    }
}
