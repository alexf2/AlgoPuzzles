using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Algorithms;

namespace AlgoPuzzles.Models
{
    public sealed class Registry
    {
        readonly IAlgo[] _algos;
        readonly int? _index;

        public Registry(IAlgo[] algos, int? index = null)
        {
            _algos = algos;
            _index = index;
        }

        public IAlgo[] Algorithms => _algos;

        public IAlgo Selected => _algos[_index.Value];

        public int Index => _index.Value;
    }
}
