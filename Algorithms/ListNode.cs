using System;
using System.Linq;
using System.Text;


namespace Algorithms
{
    public static class ListBuilder {
        public static ListNode<T> Build<T>(string list)
        {
            if (list == null)
                return null;

            ListNode<T> prevInstance = null, head = null;
            foreach (var v in list.Split(';').Select(it => it.Trim()))
            {
                T value;
                if (v == string.Empty)
                    value = default(T);
                else
                    value = (T)Convert.ChangeType(v, typeof(T));

                if (prevInstance == null)
                {
                    head = prevInstance = new ListNode<T>() { Value = value };
                    prevInstance.Value = value;
                }
                else
                {
                    prevInstance.Next = new ListNode<T>() { Value = value };
                    prevInstance = prevInstance.Next;
                }
            }

            return head;
        }
    }

    public class ListNode<T>
    {
        public ListNode<T> Next;
        public T Value;

        public ListNode()
        {
        }

        public ListNode<T> Add(T val)
        {
            Next = new ListNode<T>() { Value = val };
            return Next;
        }

        public override string ToString()
        {
            var curr = this;
            var b = new StringBuilder();
            while (curr != null)
            {
                if (b.Length > 0)
                    b.Append("; ");
                b.Append(object.Equals(curr.Value, default(T)) ? string.Empty:curr.Value.ToString() );

                curr = curr.Next;
            }
            return b.ToString();
        }
    }

    public sealed class ListNode: ListNode<int>
    {
    }
}
