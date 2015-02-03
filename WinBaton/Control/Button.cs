using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WinBaton.Control
{
    public class Button : IControl
    {
        private AutomationElement button = null;

        public Button(AutomationElement button) { this.button = button; }

        public void Click()
        {
            var pattern = (InvokePattern)button.GetCurrentPattern(InvokePattern.Pattern);
            pattern.Invoke();
        }
    }
}
