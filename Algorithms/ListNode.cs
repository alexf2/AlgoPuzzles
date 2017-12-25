using System;
using System.Collections;

namespace Algorithms
{    
    public class ListNode<T>: ICollection
    {
        public sealed class ListEnumerator : IEnumerator
        {
            readonly ListNode<T> _root;
            ListNode<T> _current;

            public ListEnumerator(ListNode<T> root)
            {
                _root = root;
            }

            object IEnumerator.Current => _current.Value;

            bool IEnumerator.MoveNext()
            {
                if (_current == null)
                {
                    _current = _root;
                    return true;
                }
                if (_current.Next != null)
                {
                    _current = _current.Next;
                    return true;
                }
                return false;
            }

            void IEnumerator.Reset()
            {
                _current = _root;
            }
        }

        public ListNode<T> Next;
        public T Value;

        public ListNode()
        {
        }

        int ICollection.Count
        {
            get
            {
                var curr = this;
                var cnt = 0;
                do
                {
                    cnt++;
                } while ((curr = curr.Next) != null);
                return cnt;
            }
        }


        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => null;

        void ICollection.CopyTo(Array array, int index)
        {
            var curr = this;
            for (var i = index; i < array.Length; ++i, curr = curr.Next)
                array.SetValue(curr.Value, i);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator(this);
        }

        public ListNode<T> Add(T val)
        {
            Next = new ListNode<T>() { Value = val };
            return Next;
        }
    }

    
    public sealed class ListNode: ListNode<int>
    {
    }
}
