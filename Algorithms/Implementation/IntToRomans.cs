using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Algorithms.Implementation
{
    public class IntToRomans : AlgoBase<IntToRomans.Args>
    {        
        public sealed class Args
        {
            [Display(Name = "An arabic number")]
            public int Number { get; set; } = 0;
        }

        public override string Name { get => "Arabic numbers to romans"; }

        public override string Description { get => @"Given an arabic number [1, 3999]. Convert to a roman string.
<br/><pre>Example: 12 --> XII</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new { RomanNumber = TestArabRoman.IntToRoman(input.Number) };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Number = 12},
                new Args(){Number = 29},
                new Args(){Number = 19},
                new Args(){Number = 21},
                new Args(){Number = 0},
                new Args(){Number = 5000},
                new Args(){Number = 187}
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }    

    static class TestArabRoman
    {
        readonly static string[] _romans = new[] { "I", "IV", "V", "IX", "X", "XL", "L", "XC", "C", "CD", "D", "CM", "M" };
        readonly static int[] _ints = new[] { 1, 4, 5, 9, 10, 40, 50, 90, 100, 400, 500, 900, 1000 };

        public static string IntToRoman (int num)
        {
            if (num < 1 || num > 3999)
                throw new Exception($"Can not convert. Argument {num} should be in range [1, 3999].");

            var times = 0;
            var sb = new StringBuilder();

            for (var i = _ints.Length - 1; i >= 0; i--)
            {

                times = Math.DivRem(num, _ints[i], out num);

                while (times > 0)
                {
                    sb.Append(_romans[i]);
                    times--;
                }
            }
            return sb.ToString();
        }
    }
}
