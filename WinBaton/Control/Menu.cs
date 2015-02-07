using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WinBaton.Service;

namespace WinBaton.Control
{
    public class Menu : Container
    {
        public Menu(AutomationElement element) { this.element = element; }

        public IEnumerable<MenuItem> Items
        {
            get
            {
                IEnumerable<MenuItem> menu = null;
                Sync.TryWaitFor(() =>
                {
                    menu = this.element.FindAll(TreeScope.Children,
                        new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.MenuItem))
                    .Cast<AutomationElement>()
                    .Select(x => (MenuItem)Converter.ConverUIAElementToBatonCtrl(x));
                    return menu != null && menu.Count() > 0;
                });
                return menu;
            }
        }
    }
}
