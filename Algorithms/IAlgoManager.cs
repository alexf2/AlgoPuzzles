using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Algorithms
{
    public interface IAlgoManager
    {
        Task<string> LoadCode(IAlgo algo);
    }

    public sealed class AlgoManager: IAlgoManager
    {
        public async Task<string> LoadCode (IAlgo algo)
        {
            var codeBaseUrl = Assembly.GetExecutingAssembly().CodeBase;
            var filePathToCodeBase = new Uri(codeBaseUrl).LocalPath;
            var directoryPath = Path.GetDirectoryName(filePathToCodeBase);

            using (var f = File.OpenRead(Path.Combine(directoryPath, $"Implementation\\{algo.FileName}")))
            using (var stm = new StreamReader(f))
                return await stm.ReadToEndAsync();                                
        }
    }
}
