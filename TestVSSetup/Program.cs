using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using WinBaton.Control;
using WinBaton.Service;

namespace TestVSSetup
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = Process.Start(@"H:\vs_enterprise.exe");

            dynamic win = new Window(proc);
            
            //Button btnUpdate = win[AutomationId: "UpdateButton"];
            Button btnCancel = win[AutomationId: "CancelButton"];

            Sync.TryWaitFor(() =>
            {
                var vsProc = Process.GetProcessesByName("vs_enterprise");
                win = new Window(vsProc.First());
                btnCancel = win[AutomationId: "CancelButton"];
                return btnCancel != null;
            }, timeout: 1000 * 60 * 60 * 2, interval: 1000 * 60);

            btnCancel.Click();

            //btn.Click();
        }
    }
}
