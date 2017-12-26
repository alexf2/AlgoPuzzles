using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Algorithms;

namespace Algorithms.Implementation
{
    public sealed class ReverseList : AlgoBase<ReverseList.Args>
    {
        public sealed class Args
        {
            [Display(Name = "Original single linked List")]                     
            public ListNode<int> List { get; set; }
        }
        

        public override string Name { get => "Reversing List"; }

        public override string Description { get => @"Given a Single-linked list on numbers. 
 Reverse the list without using extra storage."; }
        
        protected override dynamic ExecuteCore(Args input)
        {            
            return new { ReversedList = TestReverseList.Reverse(input.List) };            
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){List = GetList1()},
                new Args(){List = GetList2()},
                new Args(){List = GetList3()},
                new Args(){List = null}
            };
        }

        ListNode<int> GetList1()
        {
            var lst = new ListNode<int>() { Value = 10 };
            lst.Add(11).Add(9).Add(0).Add(7).Add(5).Add(22);
            return lst;
        }
        ListNode<int> GetList2()
        {
            var lst = new ListNode<int>() { Value = 2 };            
            return lst;
        }
        ListNode<int> GetList3()
        {
            var lst = new ListNode<int>() { Value = 2 };
            lst.Add(3);
            return lst;
        }        
    }

    static class TestReverseList
    {
        public static ListNode<int> Reverse (ListNode<int> lst)
        {
            if (lst == null || lst.Next == null)
                return lst;

            ListNode<int> current = lst, prev = null;
            while (current != null)
            {
                var next = (ListNode<int>)current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }

            return prev;
        }
    }
}

