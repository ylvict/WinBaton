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
    public class Window : DynamicObject, IControl
    {
        private AutomationElement window = null;
        public Window(int procId)
        {
            this.window = null;
            Sync.TryWaitFor(() =>
            {
                this.window = AutomationElement.FromHandle(Process.GetProcessById(procId).MainWindowHandle);
                return window != null;
            });
        }
        public Window(AutomationElement window)
        {
            this.window = null;
            Sync.TryWaitFor(() =>
            {
                this.window = window;
                return window != null;
            });
        }
        public Window(string autoId)
        {
            this.window = null;
            Sync.TryWaitFor(() =>
            {
                this.window = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.AutomationIdProperty, autoId));
                return window != null;
            });
        }
        public Window(Process proc)
        {
            this.window = null;
            Sync.TryWaitFor(() =>
            {
                this.window = AutomationElement.FromHandle(proc.MainWindowHandle);
                return window != null;
            });
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var ctrlList = this.window.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, binder.Name));
            if (ctrlList.Count <= 0)
            {
                throw new KeyNotFoundException("Cannot found element by autoId.");
            }
            if (ctrlList.Count > 1)
            {
                throw new DuplicateWaitObjectException("Duplicate item found by autoId.");
            }
            result = Converter.ConverUIAElementToBatonCtrl(ctrlList[0]);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            List<PropertyCondition> cond = new List<PropertyCondition>();
            for (int i = 0; i < binder.CallInfo.ArgumentNames.Count; i++)
            {
                var propertyName = binder.CallInfo.ArgumentNames[i];
                var property = (AutomationProperty)typeof(AutomationElement)
                    .GetField(propertyName + "Property").GetValue(null);
                cond.Add(new PropertyCondition(property, indexes[i]));
            }

            List<AutomationElement> ctrlList = new List<AutomationElement>();
            Sync.TryWaitFor(() =>
            {
                if (ctrlList.Count > 0) return true;
                ctrlList = this.window.FindAll(TreeScope.Children, new AndCondition(cond.ToArray()))
                    .Cast<AutomationElement>()
                    .ToList();
                return false;
            });
            result = ctrlList.Select(x => Converter.ConverUIAElementToBatonCtrl(x)).ToList();
            return true;
        }
    }
}
