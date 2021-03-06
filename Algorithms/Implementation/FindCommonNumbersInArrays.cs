﻿using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Algorithms;
using System;

namespace Algorithms.Implementation
{
    public class FindCommonNumbersInArrays : AlgoBase<FindCommonNumbersInArrays.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "First Array")]            
            public int[] A1 { get; set; } = EmptyArray;

            [Display(Name = "Second Array")]                                    
            public int[] A2 { get; set; } = EmptyArray;

            [Display(Name = "Third Array")]            
            public int[] A3 { get; set; } = EmptyArray;
        }        

        public override string Name { get => "Common array numbers"; }

        public override string Description { get => @"Given three positive int arrays of unique numbers. 
 There is a need to find common numbers of all the arrays.
<br/><pre>Example: [12; 11; 15; 8], [7; 3; 11; 18; 8], [1; 2; 11; 7; 8] --> [11, 8]</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {            
            return new { CommonNumbers = TestCommonNumbersThree.FindCommonNumbers(input.A1, input.A2, input.A3) };
        }

        public override IEnumerable<Args> TestSet { get => new [] {
                new Args(){A1 = new []{1,7,4}, A2 = new []{18, 7, 22, 1}, A3 = new []{18, 1, 4, 5, 7} },
                new Args(){A1 = new []{1,7,4}, A2 = new int[]{}, A3 = new []{2, 1, 4, 5, 7} },
                new Args(){A1 = new []{11,12,11, 3, 7, 9, 15, 1, 10, 2}, A2 = new []{1, 12, 11, 9, 55, 88, 99}, A3 = new []{9, 1, 88, 121, 12} }
            };
        }        

        public override string FileName { get => base.GetFileName(); }
    }

    static class TestCommonNumbersThree
    {
        class FreqPair
        {
            internal int Val;
            internal int Count;
        }

        public static int[] FindCommonNumbers(params int[][] arr)
        {
            int size = arr.Sum(a => a?.Length ?? 0);
            if (size == 0)
                return new int[] {};

            FreqPair[] freq = new FreqPair[size]; //будем использовать хэш-таблицу

            for (int i = 0; i < arr.Length; i++)
            {
                int[] a = arr[i];
                if (a == null)
                    continue;
                for (int j = 0; j < a.Length; j++)
                {
                    if (a[j] < 1)
                        throw new Exception($"Found zero or negative number: {a[j]}.");
                    int idx = a[j] % size; //в качестве хэш-функции - остаток от деления
                    if (freq[idx] == null)
                        freq[idx] = new FreqPair() { Val = a[j] };

                    freq[idx].Count++;
                }
            }

            return freq.Where(f => f != null && f.Count == arr.Length).Select(f => f.Val).ToArray();
        }
    }
}
