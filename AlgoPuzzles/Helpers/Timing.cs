using System;
using System.Diagnostics;


namespace AlgoPuzzles.Helpers
{
    public sealed class Timing : IDisposable
    {
        readonly dynamic _parent;
        readonly Stopwatch _stopWatch = new Stopwatch();
        readonly Action<dynamic, TimeSpan> _setter = (d, ts) => d.Duration = ts;

        public Timing(dynamic parent, Action<dynamic, TimeSpan> setter = null)
        {
            _parent = parent;
            _stopWatch.Start();
            if (setter != null)
                _setter = setter;
        }

        public void Dispose()
        {
            _stopWatch.Stop();            
            _setter(_parent, _stopWatch.Elapsed);
        }
    }
}
