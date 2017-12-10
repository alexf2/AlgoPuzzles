using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Algorithms;

namespace Algorithms.Implementation
{
    public sealed class SearchForSetSubstring : AlgoBase<SearchForSetSubstring.Args>
    {
        public sealed class Args
        {
            [Display(Name = "Target String S1")]
            public string Str { get; set; }

            [Display(Name = "Substring to search S2")]
            public string Substr { get; set; }
        }

        public override string Name { get => "Search substring as a set"; }

        public override string Description { get => @"Given an ASCII string S1 and a substring S2 to search in. 
 Find starting index in the target string S1 of first S2 occurence regardless of the character sequence. The complixity should be close to O(N * Log(M))."; }

        protected override dynamic ExecuteCore(Args input)
        {            
            return new { SubstringIndex = UnorderedSubstring.FindIndex(input.Str, input.Substr) };            
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Str = "hgtuhh23jh3hj34h3j34hj34hj", Substr = "hh23jh3hj"},
                new Args(){Str = "hgtuhh23jh3hj34h3j34hj34hj", Substr = ""},
                new Args(){Str = "hgtuhh23jh3hj34h3j34hj34hj", Substr = "hh23jh3hj"}
            };
        }        
    }

    static class UnorderedSubstring
    {
        /// <summary>
        /// Finds substring s in the string t without respecting characters order.
        /// </summary>
        /// <param name="t">Main string</param>
        /// <param name="s">Substring</param>
        /// <returns>Substring index inside of t. If not found returns -1.</returns>
        public static int FindIndex(string t, string s)
        {
            if (string.IsNullOrEmpty(t) || string.IsNullOrEmpty(s) || t.Length < s.Length)
                return -1;

            //Func<char[], int, int, int> sumEval = (buff, iStart, len) => buff.Skip(iStart).Take(len).Aggregate(0, (acc, ch) => acc + (int)ch);
            Func<char[], int, int, int> evalSum = (buff, iStart, len) => {
                var res = 0;
                for (int j = iStart; j < iStart + len; ++j)
                    res += (int)buff[j];

                return res;
            };

            var buffT = t.Substring(0, s.Length).ToCharArray();
            var sumT = evalSum(buffT, 0, s.Length); //Precalculating code summ of the window

            var buffS = s.ToCharArray();
            Array.Sort(buffS);
            var sumS = evalSum(buffS, 0, s.Length); //Calculating target summ, which we need to find

            for (var i = 0; i < t.Length - s.Length + 1; ++i)
            {
                //We use a sliding window of character codes summ. Each time we substract fading out character code and add next character code from the right side
                if (i > 0)
                {
                    sumT -= (int)t[i - 1]; //Removing previous character code (on the left side of window)
                    sumT += (int)t[s.Length + i - 1]; //Adding next character code (on the right side)
                }

                if (sumS == sumT)
                {
                    //If we found target summ, we need to validate respective characters in the window, as they may differ, but the summ may be the same
                    var buffTmp = t.Substring(i, s.Length).ToCharArray();
                    Array.Sort(buffTmp);
                    if (buffTmp.SequenceEqual(buffS))
                        return i;
                }
            }

            return -1;
        }
    }

}


