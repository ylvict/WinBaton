using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            Document editor = win[AutomationId: "15"];
            var path = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system.ini");
            var text = File.ReadAllText(path);
            editor.SetText(text, TextInputMode.SendKey);
            editor.SetText(text, TextInputMode.Paste);
            var currentText = editor.Text;

            var menubar = win.MenuBar;
            menubar["File"]["Open..."].Invoke();

            var confirmDlg = win["Notepad", "#32770", ControlType.Window];
            if (confirmDlg != null)
                confirmDlg.CommandButton_7.Click();

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

            confirmDlg = win["Notepad", "#32770", ControlType.Window];
            if (confirmDlg != null)
                confirmDlg.CommandButton_7.Click();
        }
    }
}
