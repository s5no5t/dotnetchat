using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DotNetChatServer
{
    /// <summary>
    /// Implements a simple SynchronizationContext. Taken from http://blogs.msdn.com/b/pfxteam/archive/2012/01/20/10259049.aspx .
    /// </summary>
    internal sealed class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> _queue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            _queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        public void RunOnCurrentThread()
        {
            KeyValuePair<SendOrPostCallback, object> workItem;
            while (_queue.TryTake(out workItem, Timeout.Infinite))
                workItem.Key(workItem.Value);
        }

        public void Complete()
        {
            _queue.CompleteAdding();
        }
    }
}