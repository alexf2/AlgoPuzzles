using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Algorithms.Implementation
{
    public class Fibos: AlgoBase<Fibos.Args>
    {
        public sealed class Args
        {
            [Display(Name = "Fibonacci sequence length")]
            public int Number { get; set; } = 0;
        }

        public override string Name { get => "Fibonacci numbers"; }

        public override string Description { get => @"Given a squence length. Generate Fibonacci numbers.
<br/><pre>Example: 5 --> 0, 1, 1, 2, 3</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new {Fibos = FiboTools.GetFiboSequence(input.Number).ToArray() };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){Number = 0},
                new Args(){Number = 1},
                new Args(){Number = 2},
                new Args(){Number = 3},
                new Args(){Number = 4},
                new Args(){Number = 5},
                new Args(){Number = 6},
                new Args(){Number = 7}
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class FiboTools
    {
        public static IEnumerable<ulong> GetFiboSequence(int len = 0)
        {
            ulong current = 0, next = 1;

            while (len-- > 0)
            {
                yield return current;
                (current, next) = (next, current + next);
            }
        }
    }
}
