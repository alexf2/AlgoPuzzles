using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using Algorithms;


namespace Algorithms.Implementation
{
    public class GivingChange : AlgoBase<GivingChange.Args>
    {
        static readonly int[] EmptyArray = new int[] { };

        public sealed class Args
        {
            [Display(Name = "Required summ")]
            public int Amount { get; set; } = 0;

            [Display(Name = "Money in the wallet")]
            public int[] WalletMoney { get; set; } = EmptyArray;
        }

        public override string Name { get => "Giving change (greedy algo)"; }

        public override string Description { get => @"Given the list of banknotes in the wallet and the amount of required money to gather from.
No fractional parts and samll coins are used: only whole numbers. Provide the list of banknotes to get exactly required amount.
<br/><pre>Example: W = [1;9;7;3;5], Amount = 12 --> [9;3]</pre>"; }

        protected override dynamic ExecuteCore(Args input)
        {
            var w = new Wallet();
            foreach (var bn in input.WalletMoney)
                w = w + new MoneyItem(bn);
            var res = w.Withdraw(input.Amount);

            return new { Money = ToArray(res.Item1), IsGathered = res.Item2 };
        }

        public override IEnumerable<Args> TestSet
        {
            get => new[] {
                new Args(){WalletMoney = new []{50, 70, 120, 150}, Amount = 100},
                new Args(){WalletMoney = new []{1, 9, 7, 3, 5}, Amount = 12},
                new Args(){WalletMoney = new []{1, 2}, Amount = 30},
                new Args(){WalletMoney = new []{100}, Amount = 100},
                new Args(){WalletMoney = new []{60, 15, 9, 9, 9, 8, 8, 5, 5, 3, 3, 3, 3, 3, 2, 2, 2, 1}, Amount = 30}
            };
        }

        public override string FileName { get => base.GetFileName(); }

        List<int> ToArray(IEnumerable<MoneyItem> money)
        {
            var res = new List<int>();
            if (money != null)
                foreach (var item in money)
                    res.AddRange(Enumerable.Repeat(item.Nominal, item.Amount));

            return res;
        }
    }


    class Wallet
    {
        List<MoneyItem> _items = new List<MoneyItem>(); //is kept sorted DESC by nominal

        public Tuple<IList<MoneyItem>, bool> Withdraw(int summ)
        {

            if (summ < 1)
                throw new Exception("Invalid amount to withdraw");
            var res = new List<MoneyItem>();

            //use greedy algorithm
            for (int i = 0; i < _items.Count && summ > 0; ++i)
            {
                var it = _items[i];
                if (it.Nominal <= summ)
                {
                    var count = Math.Min(summ / it.Nominal, it.Amount);
                    summ -= it.CashOut(count);
                    res.Add(new MoneyItem(it.Nominal, count));
                }
            }

            return new Tuple<IList<MoneyItem>, bool>(res, summ == 0);
        }

        public static int Summ(IEnumerable<MoneyItem> items)
        {
            return items.Sum(x => x.Nominal * x.Amount);
        }

        public static string Visualize(IEnumerable<MoneyItem> items)
        {
            var bld = new StringBuilder();
            foreach (var i in items)
                for (int j = 0; j < i.Amount; ++j)
                {
                    if (bld.Length > 0)
                        bld.Append(";");

                    bld.Append(i.Nominal);
                }

            return bld.ToString();
        }

        public void Deposit(IEnumerable<MoneyItem> items)
        {
            foreach (var i in items)
            {
                var tmp = this + i;
            }
        }

        public static Wallet operator +(Wallet w, MoneyItem item)
        {
            int i = w._items.BinarySearch(item);
            if (i < 0)
                w._items.Insert(~i, item);
            else
                w._items[i].CashIn(item.Amount);
            return w;
        }

        public static Wallet operator -(Wallet w, MoneyItem item)
        {
            int i = w._items.BinarySearch(item);
            if (i < 0)
                throw new Exception("There is no any item " + item.Nominal);
            else
            {
                var it = w._items[i];
                if (it.Amount >= item.Amount)
                    it.CashOut(item.Amount);
                else
                    throw new Exception("There is not enough items to withdraw " + item.Nominal + ". Asked " + item.Amount + ", available " + it.Amount);
            }
            return w;
        }

        public override string ToString()
        {
            var bld = new StringBuilder();
            foreach (var i in _items)
            {
                if (bld.Length > 0)
                    bld.Append("\r\n");
                bld.AppendFormat("{0}: {1}", i.Nominal, i.Amount);
            }
            return bld.ToString();
        }
    }

    class MoneyItem : IComparable<MoneyItem>, IComparable, IEquatable<MoneyItem>
    {
        public int Nominal { get; private set; }
        public int Amount { get; private set; }

        public MoneyItem(int nominal, int amt = 1)
        {
            if (nominal < 1)
                throw new Exception("Invalid nominal");
            Nominal = nominal;
            Amount = amt;
        }

        public int CashIn(int amount = 1)
        {
            if (amount < 1)
                throw new Exception("Invalid amount to in");

            Amount = Amount + amount;

            return amount * Nominal;
        }

        public int CashOut(int amount = 1)
        {
            if (amount < 1)
                throw new Exception("Invalid amount to out");

            if (amount > Amount)
                throw new Exception("Not enough items of " + Nominal + " to out " + amount);

            Amount = Amount - amount;

            return amount * Nominal;
        }

        public bool IsEmpty { get { return Amount == 0; } }

        #region Object
        public override int GetHashCode()
        {
            return 381 ^ Nominal;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MoneyItem);
        }

        public override string ToString()
        {
            return string.Format("Nominal {0}, count {1}", Nominal, Amount);
        }
        #endregion Object

        #region Operators
        public static bool operator ==(MoneyItem n1, MoneyItem n2)
        {
            return Equals(n1, n2);
        }
        public static bool operator !=(MoneyItem n1, MoneyItem n2)
        {
            return !Equals(n1, n2);
        }

        public static bool operator <(MoneyItem n1, MoneyItem n2)
        {
            if (n1 == null || n2 == null)
                return false;

            return n1.CompareTo(n2) < 0;
        }

        public static bool operator >(MoneyItem n1, MoneyItem n2)
        {
            if (n1 == null || n2 == null)
                return false;

            return n1.CompareTo(n2) > 0;
        }
        #endregion Operators

        #region IComparable<T>
        public int CompareTo(MoneyItem other)
        {
            if (other == null)
                return 1;

            return other.Nominal.CompareTo(Nominal);
        }
        #endregion IComparable<T>

        #region IComparable
        int IComparable.CompareTo(object other)
        {
            if (other == null)
                return 1;
            if (!(other is MoneyItem))
                throw new InvalidOperationException("CompareTo: Not a MoneyItem");

            return CompareTo((MoneyItem)other);
        }
        #endregion IComparable

        #region Equatable<T>
        public bool Equals(MoneyItem val)
        {
            if ((object)val == null)
                return false;
            if (ReferenceEquals(this, val))
                return true;

            return Nominal == val.Nominal;
        }
        #endregion Equatable<T>

        public static bool Equals(MoneyItem val1, MoneyItem val2)
        {
            if (ReferenceEquals(val1, val2))
                return true;
            if ((object)val1 == null && (object)val2 != null)
                return false;

            return val1.Equals(val2);
        }
    }
}

