using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Algorithms
{
    public interface IAlgo
    {
        string Name { get; }
        string Description { get; }
        Task<dynamic> Execute(object input);
        IEnumerable TestSet { get; }
        Type ParamsType { get; }
        string FileName { get; }
    }
    
    public interface IAlgo<T>: IAlgo
    {        
        Task<dynamic> Execute(T input);
        new IEnumerable<T> TestSet { get; }
    }
}
