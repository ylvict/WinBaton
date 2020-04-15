using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBaton.Service
{
    public static class Sync
    {
        public static int DefaultTimeout = 30 * 1000;

        public static bool TryWaitFor(Func<bool> action, int? timeout = null, int interval = 500)
        {
            timeout = timeout ?? Sync.DefaultTimeout;
            var startTime = DateTime.Now;
            Exception ex = null;
            bool finalResult = false;
            while (true)
            {
                try
                {
                    finalResult = action();
                    if (finalResult) break;
                }
                catch (Exception e) { ex = e; }
                var currentSpan = DateTime.Now - startTime;
                if (currentSpan >= TimeSpan.FromMilliseconds(timeout.Value)) break;
                System.Threading.Thread.Sleep(interval);
            }
            if (finalResult) return true;
            if (ex != null) throw ex;
            return false;
        }
    }
}
