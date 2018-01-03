using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Algorithms.Implementation
{
    public class GenerateParentheses : AlgoBase<GenerateParentheses.Args>
    {        
        public sealed class Args
        {
            [Display(Name = "Number of pairs")]
            public int N { get; set; }
        }

        public override string Name { get => "Generate Parentheses"; }

        public override string Description { get => @"Given n pairs of parentheses, write a function to generate all combinations of well-formed parentheses.
<br/><pre>Example: N = 3 -->
[
  '((()))',
  '(()())',
  '(())()',
  '()(())',
  '()()()'
]
</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            var res = TestParentheses.GenerateParenthesis(input.N);
            return new { NumberOfCombinations = res.Count, Parentheses = ToMatrix(res) };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {                
                new Args(){N = 3},
                new Args(){N = 1},
                new Args(){N = 0},
                new Args(){N = 10}
            };
        }

        public override string FileName { get => base.GetFileName(); }

        //to print the strings vertically
        string[,] ToMatrix(IList<string> lst)
        {
            if (lst == null || lst.Count == 0)
                return new string[,] { };

            var res = new string[lst.Count, 1];
            for (int i = 0; i < lst.Count; ++i)
                res[i, 0] = lst[i];
            return res;
        }
    }

    static class TestParentheses
    {
        public static IList<string> GenerateParenthesis(int n)
        {
            var result = new List<string>();
            Dfs(result, string.Empty, n, n);
            return result;
        }

        static void Dfs(List<string> result, string s, int left, int right)
        {
            if (left > right)
                return;

            if (left == 0 && right == 0)
            {
                result.Add(s);
                return;
            }

            if (left > 0)
            {
                Dfs(result, s + "(", left - 1, right);
            }

            if (right > 0)
            {
                Dfs(result, s + ")", left, right - 1);
            }
        }
    }
}
