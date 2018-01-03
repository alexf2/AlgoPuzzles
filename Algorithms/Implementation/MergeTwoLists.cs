using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Algorithms;

namespace Algorithms.Implementation
{
    public sealed class MergeTwoLists : AlgoBase<MergeTwoLists.Args>
    {        
        public sealed class Args
        {
            [Display(Name = "First List")]
            public ListNode<int> List1 { get; set; }

            [Display(Name = "Second List")]
            public ListNode<int> List2 { get; set; }
        }


        public override string Name { get => "Merging two ordered lists"; }

        public override string Description { get => @"Given two sorted single-linked ordered lists of integers. Return a merged ordered list.
<br/><pre>Example: [10, 12, 20], [9, 11, 15, 20] --> [9, 10, 11, 12, 15, 20, 20]</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            return new { MergedArray = TestMergedLists.MergeTwoLists(input.List1, input.List2) };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){List1 = GetList1(), List2 = GetList2()},
                new Args(){List1 = GetListE(), List2 = GetList1()},
                new Args(){List1 = GetList2(), List2 = GetListE()}
            };
        }

        public override string FileName { get => base.GetFileName(); }

        ListNode<int> GetList1()
        {
            var lst = new ListNode<int>() { Value = 10 };
            lst.Add(12).Add(20);
            return lst;
        }
        ListNode<int> GetList2()
        {
            var lst = new ListNode<int>() { Value = 9 };
            lst.Add(11).Add(15).Add(20);
            return lst;
        }
        ListNode<int> GetListE()
        {
            return null;
        }
    }

    static class TestMergedLists
    {
        public static ListNode<int> MergeTwoLists(ListNode<int> l1, ListNode<int> l2)
        {
            ListNode<int> res = null, last = null;

            Action<int?> add = value => {
                var item = new ListNode<int>() { Value = value.Value };
                if (res == null)
                    last = res = item;
                else
                {
                    last.Next = item;
                    last = item;
                }
            };

            while (l1 != null || l2 != null)
            {
                int? i1 = l1?.Value, i2 = l2?.Value;
                if (i1 < i2 || l2 == null)
                {
                    add(i1);
                    l1 = l1.Next;
                }
                else
                {
                    add(i2);
                    l2 = l2.Next;
                }
            }
            return res;
        }
    }

}

