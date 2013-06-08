using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class AudioQueue
    {
        Dictionary<DateTime, AudioCapture> _captures;
        readonly object _syncCaptures;

        public AudioQueue()
        {
            _syncCaptures = new object();
            _captures = new Dictionary<DateTime, AudioCapture>();
        }

        public void AddCapture(AudioCapture capture)
        {
            lock (_syncCaptures)
            {
                _captures.Add(capture.ReceiveTimestamp, capture);
            }
        }

        public AudioCapture PopCapture()
        {
            lock (_syncCaptures)
            {
                // todo: use linq to peek oldest capture
                
                AudioCapture oldestCapture = null;
                DateTime oldestTimestamp = DateTime.Now;
                foreach (KeyValuePair<DateTime, AudioCapture> capture in _captures)
                {
                    if (capture.Value.ReceiveTimestamp < oldestTimestamp)
                    {
                        oldestTimestamp = capture.Value.ReceiveTimestamp;
                        oldestCapture = capture.Value;
                    }
                }
                _captures.Remove(oldestTimestamp);
                return oldestCapture;
            }
        }
    }
}
