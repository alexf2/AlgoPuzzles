﻿using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Algorithms;


namespace Algorithms.Implementation
{
    public class FindCommonNumbersInArrays : AlgoBase<FindCommonNumbersInArrays.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "First Array")]
            [TypeConverter(typeof(SemicolonSeparatedArrayConvertor<int>))]
            public int[] A1 { get; set; } = EmptyArray;

            [Display(Name = "Second Array")]                        
            [TypeConverter(typeof(SemicolonSeparatedArrayConvertor<int>))]
            public int[] A2 { get; set; } = EmptyArray;

            [Display(Name = "Third Array")]
            [TypeConverter(typeof(SemicolonSeparatedArrayConvertor<int>))]
            public int[] A3 { get; set; } = EmptyArray;
        }

        public sealed class Result
        {
            [UIHint("Collection")]
            public int[] ReversedList { get; set; }
        }

        public override string Name { get => "Common array numbers"; }

        public override string Description { get => @"Given three int arrays. 
 There is a need to find common numbers of all the arrays."; }

        protected override dynamic ExecuteCore(Args input)
        {            
            return new Result() { ReversedList = TestCommonNumbersThree.FindCommonNumbers(input.A1, input.A2, input.A3) };
        }

        public override IEnumerable<Args> TestSet { get => new [] {
            new Args(){A1 = new []{1,7,4}, A2 = new []{18, 7, 22, 1}, A3 = new []{0, 1, 4, 5, 7} },
            new Args(){A1 = new []{1,7,4}, A2 = new int[]{}, A3 = new []{0, 1, 4, 5, 7} },
            new Args(){A1 = new []{11,12,11, 0, 7, 9, 15, 1, 10, 2}, A2 = new []{1, 12, 11, 9, 55, 88, 99}, A3 = new []{9, 1, 88, 121, 12} }
        }; }        
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
