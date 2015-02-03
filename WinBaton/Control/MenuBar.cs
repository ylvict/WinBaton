using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WinBaton.Service;

namespace WinBaton.Control
{
    public class MenuBar : Menu
    {
        public MenuBar(AutomationElement menubar) : base(menubar)
        {
            this.element = menubar;
        }
    }
}
