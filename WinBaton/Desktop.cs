using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace WinBaton
{
    public class Desktop
    {
        public static WinBaton.Control.Window FindWindow(string autoId)
        {
            return new Control.Window(autoId);
        }
    }
}
