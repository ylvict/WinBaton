using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinBaton.Service
{
    public static class Sync
    {
        public static void TryWaitFor(Func<bool> action, int timeout = 30 * 1000, int interval = 500)
        {
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
                if (currentSpan >= TimeSpan.FromMilliseconds(timeout)) break;
                System.Threading.Thread.Sleep(interval);
            }
            if (finalResult) return;
            if (ex != null) throw ex;
        }
    }
}
