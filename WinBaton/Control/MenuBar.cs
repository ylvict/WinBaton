using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WinBaton.Control
{
    public class MenuBar : IControl
    {
        private AutomationElement menubar = null;
        public MenuBar(AutomationElement menubar)
        {
            this.menubar = menubar;
        }
    }
}
