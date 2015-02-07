using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WinBaton.Service;

namespace WinBaton.Control
{
    public class MenuItem : DynamicObject, IControl
    {
        private AutomationElement element = null;
        public MenuItem(AutomationElement element) { this.element = element; }

        public string Name
        {
            get
            {
                return this.element.Current.Name;
            }
        }


        public Menu Expand()
        {
            this.ExpandCollapse(true);
            AutomationElement menu = null;
            Sync.TryWaitFor(() =>
            {
                menu = element.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Menu));
                return menu != null;
            });
            return (Menu)Converter.ConverUIAElementToBatonCtrl(menu);
        }

        public void Collapse()
        {
            this.ExpandCollapse(false);
        }

        private void ExpandCollapse(bool isexpand)
        {
            Sync.TryWaitFor(() =>
            {
                var pattern = (ExpandCollapsePattern)this.element.GetCurrentPattern(ExpandCollapsePattern.Pattern);
                if (isexpand)
                {
                    if (pattern.Current.ExpandCollapseState == ExpandCollapseState.Expanded) return true;
                    pattern.Expand();
                    return false;
                }
                else
                {
                    if (pattern.Current.ExpandCollapseState == ExpandCollapseState.Collapsed) return true;
                    pattern.Collapse();
                    return false;
                }
            });

        }

        public void Invoke()
        {
            ((InvokePattern)this.element.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            dynamic menu = this.Expand();
            result = menu[indexes[0]];
            return true;
        }
    }
}
