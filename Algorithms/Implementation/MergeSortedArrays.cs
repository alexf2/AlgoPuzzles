using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Algorithms;

namespace Algorithms.Implementation
{     
    public sealed class MergeSortedArrays : AlgoBase<MergeSortedArrays.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "First Array")]                        
            public int[] A1 { get; set; } = EmptyArray;

            [Display(Name = "Second Array")]                        
            public int[] A2 { get; set; } = EmptyArray;
        }
        

        public override string Name { get => "Merge Sorted Arrays"; }

        public override string Description { get => @"Given two sorted arrays of int type. 
 There should be returned a merged sorted array without any duplication. The solution should be optimal: no additional sorting or using hash maps.
<br/><pre>Example: [11; 15; 17; 20; 21], [-1; 9; 12; 19; 20; 21; 22] --> [-1; 9; 11; 12; 15; 17; 19; 20; 21; 22]</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {            
            return new { MergedArray = TestMergedArrays.MergeArrays(input.A1, input.A2) };            
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){A1 = new int[]{}, A2 = new []{1,10,12,15}},
                new Args(){A1 = new int[]{1,2,3}, A2 = new int[]{}},
                new Args(){A1 = new int[]{1,2,3}, A2 = new int[]{0}},
                new Args(){A1 = new int[]{1,12,13}, A2 = new int[]{0, 5}},
                new Args(){A1 = new []{5,6,7,8,9,10}, A2 = new int[]{8,9,10,11,15}},
                new Args(){A1 = new []{-1, 0, 2, 10, 15, 18, 20}, A2 = new int[]{9, 10, 11, 12, 14, 16, 19, 22}},
                new Args(){A1 = new []{10,12}, A2 = new []{1,10,12,15}},
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class TestMergedArrays
    {
        public static int[] MergeArrays(int[] a1, int[] a2)
        {
            if (a1 == null || a2 == null)
                throw new Exception("Invalid data: detected null array");

            if (a1.Length == 0)
                return a2;
            if (a2.Length == 0)
                return a1;

            var res = new List<int>(a1.Length + a2.Length);
            int? prevValue = null;

            for (int i = 0, j = 0; i < a1.Length || j < a2.Length;)
            {
                int val;

                if (i < a1.Length && j < a2.Length)
                {
                    if (a1[i] < a2[j])
                        val = a1[i++];
                    else
                        val = a2[j++];
                }
                else if (i < a1.Length)
                    val = a1[i++];
                else
                    val = a2[j++];

                if (prevValue != val)
                {
                    prevValue = val;
                    res.Add(val);
                }
            }
            return res.ToArray();
        }

    }

}

