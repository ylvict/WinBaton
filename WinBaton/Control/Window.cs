using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Automation;
using WinBaton.Service;

namespace WinBaton.Control
{
    public class Window : Container
    {
        public Window(AutomationElement window) { this.element = window; }

        public Window(int procId)
        {
            this.element = null;
            Sync.TryWaitFor(() =>
            {
                this.element = AutomationElement.FromHandle(Process.GetProcessById(procId).MainWindowHandle);
                return this.element != null;
            });
        }

        public Window(string autoId)
        {
            this.element = null;
            Sync.TryWaitFor(() =>
            {
                this.element = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, autoId));
                return this.element != null;
            });
        }

        public Window(Process proc)
        {
            this.element = null;
            Sync.TryWaitFor(() =>
            {
                this.element = AutomationElement.FromHandle(proc.MainWindowHandle);
                return this.element != null;
            });
        }
    }
}
