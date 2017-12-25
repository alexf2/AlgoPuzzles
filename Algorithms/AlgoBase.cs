using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;

namespace Algorithms
{
    public abstract class AlgoBase<T>: IAlgo<T>
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        protected abstract dynamic ExecuteCore(T input);

        public Task<dynamic> Execute(T input)
        {
            return Task.Run(() => {
                return ExecuteCore(input);
            });
        }

        public abstract IEnumerable<T> TestSet
        {
            get;        
        }

        Task<dynamic> IAlgo.Execute(object input) => Execute((T)input);
        
        IEnumerable IAlgo.TestSet { get => TestSet; }
        Type IAlgo.ParamsType { get => typeof(T); }
    }
}
