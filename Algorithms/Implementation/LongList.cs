using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Algorithms.Implementation
{
    public class LongList : AlgoBase<LongList.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "List")]
            public int[] List { get; set; } = EmptyArray;

            [Display(Name = "Return last N")]
            public int TailSize { get; set; } = 1;
        }

        public override string Name { get => "Long list begin"; }

        public override string Description { get => @"Given a long list of integers. Return the elements except of last N.
You should not load all the list into the memory and should not iterate all the items twice."; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new { Array = TestListTail.GetLast(input.List, input.TailSize).ToArray() };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){List = new[]{1, 122, 15, 8, 1, 87, 122, 1}, TailSize = 3 },
                new Args(){List = new[]{1, 122, 15, 8, 1, 87, 122, 1}, TailSize = 1 },
                new Args(){List = new[]{1, 122, 15, 8, 1, 87, 122, 1}, TailSize = 0 },
                new Args(){List = new[]{1, 122}, TailSize = 15 },
                new Args(){List = new[]{1}, TailSize = 1 },
                new Args(){List = new int[]{}, TailSize = 5 }
            };
        }

        public override string FileName { get => base.GetFileName(); }
    }

    static class TestListTail
    {
        public static IEnumerable<T> GetLast<T>(IEnumerable<T> en, int n)
        {
            if (n < 0 || en == null)
                yield break;

            if (n == 0)
            {
                foreach (var item in en)
                    yield return item;
                yield break;
            }

            var enm = en.GetEnumerator();
            T[] lastFrame = null, currFrame = null;

            while (true)
            {
                currFrame = enm.TakeNextN(n);
                if (currFrame.Length < n)
                    break;

                if (lastFrame != null)
                    foreach (var item in lastFrame)
                        yield return item;

                lastFrame = currFrame;
                currFrame = null;
            }

            lastFrame = Merge(lastFrame, currFrame);

            if (lastFrame.Length > n)
                for (int i = 0; i < lastFrame.Length - n; ++i)
                    yield return lastFrame[i];
        }

        static T[] Merge<T>(T[] a1, T[] a2)
        {
            if (a1 == null)
            {
                return a2 == null ? new T[] { } : a2;
            }
            if (a2 == null)
            {
                return a1 == null ? new T[] { } : a1;
            }
            return a1.Concat(a2).ToArray();
        }

        static T[] TakeNextN<T>(this IEnumerator<T> enm, int n)
        {
            var res = new List<T>(n);

            while (n-- > 0 && enm.MoveNext())
                res.Add(enm.Current);

            return res.ToArray();
        }
    }
}
