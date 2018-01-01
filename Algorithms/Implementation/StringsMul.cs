using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Algorithms;

namespace Algorithms.Implementation
{
    public sealed class StringsMul : AlgoBase<StringsMul.Args>
    {
        public sealed class Args
        {
            [Display(Name = "Integer 1")]
            [Required]
            public string S1 { get; set; }

            [Display(Name = "Integer 2")]
            [Required]
            public string S2 { get; set; }
        }

        public override string Name { get => "Multiply Strings"; }

        public override string Description { get => @"Given two unbounded strings, representing whole numbers with no sign. 
 Return the product."; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new { Product = SolutionStringsMul.Multiply(input.S1, input.S2) };            
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){S1="123", S2="12"},
                new Args(){S1="10", S2="22"},
                new Args(){S1="78965", S2="1246"},
                new Args(){S1="657", S2="120"},
                new Args(){S1="7654", S2="18654"},
                new Args(){S1="1176", S2="57"}                
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class SolutionStringsMul
    {
        public enum Operation { Add, Mul };
        const int ZeroAsciiCode = 48;

        public static string Multiply(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
                return null;

            if (s1.Length < s2.Length)
            {
                var tmp = s1;
                s1 = s2;
                s2 = tmp;
            }

            string res = "0";
            for (int i = s2.Length - 1; i >= 0; --i)
            {
                var product = Eval(s1, s2[i].ToString(), Operation.Mul);
                //Console.WriteLine($"{s1} x {s2[i]} = {product}");
                res = Eval(res, product + new string('0', s2.Length - i - 1), Operation.Add);
            }
            return res;
        }

        static readonly Func<int, int, int> _evalAdd = (a, b) => a + b;
        static readonly Func<int, int, int> _evalMul = (a, b) => a * b;

        public static string Eval(string s1, string s2, Operation op)
        {
            var res = new StringBuilder();


            Func<int, int, int> eval = op == Operation.Add ? _evalAdd : _evalMul;
            if (op == Operation.Mul && !(s1.Length == 1 || s2.Length == 1))
                throw new Exception("Bad size");


            int excess = 0;
            for (int i = s1.Length - 1, j = s2.Length - 1; i >= 0 || j >= 0; i--, j--)
            {
                var v1 = (int)(i >= 0 ? s1[i] : '0') - ZeroAsciiCode;
                var v2 = (int)(j >= 0 ? s2[j] : '0') - ZeroAsciiCode;

                if (op == Operation.Mul)
                {
                    if (i < 0)
                        v1 = s1[0] - ZeroAsciiCode;
                    else if (j < 0)
                        v2 = s2[0] - ZeroAsciiCode;
                }

                //Console.WriteLine($"{v1} - {v2} / {i} : {j}");

                if (v1 < 0 || v1 > 9 || v2 < 0 || v2 > 9)
                    continue;

                var product = eval(v1, v2) + excess;

                if (product > 9)
                {
                    excess = product / 10;
                    product %= 10;
                }
                else
                    excess = 0;

                res.Append((char)(product + ZeroAsciiCode));
            }
            if (excess != 0)
                res.Append((char)(excess + ZeroAsciiCode));

            var tmp = new string(res.ToString().TrimEnd('0').Reverse().ToArray());
            return tmp.Length == 0 ? "0" : tmp;
        }
    }

}
