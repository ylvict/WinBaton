using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WinBaton.Control;

namespace WinBatonTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = Process.Start("notepad.exe");
            dynamic win = new Window(proc);
            var con = win[AutomationIdProperty: "MenuBar", ControlTypeProperty: ControlType.MenuBar];
            //var menu = win.TitleBar;
        }
    }
}
