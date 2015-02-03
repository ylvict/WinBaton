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

            var menubar = win.MenuBar;
            menubar["File"]["Open..."].Invoke();
            var dialog = win["Open", "#32770", ControlType.Window, LocalizedControlType: "Dialog"];
            dialog.Close.Click();

            foreach (var item in menubar.Items)
            {
                Console.WriteLine(item.Name);
                var menu = item.Expand();
                foreach (var m in menu.Items)
                {
                    Console.WriteLine("\t" + m.Name);
                }
            }

            Button btnClose = win.Close;
            btnClose.Click();
        }
    }
}
