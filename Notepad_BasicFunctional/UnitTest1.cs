using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using WinBaton.Control;
using System.IO;
using System.Windows.Automation;
using WinBaton.Service;

namespace Notepad_BasicFunctional
{
    [TestClass]
    public class UnitTest1
    {
        Window MainWin = null;
        Document Editor = null;
        MenuBar MenuBar = null;
        string SampleText = "";

        [TestInitialize]
        public void Init()
        {
            Sync.DefaultTimeout = 5 * 1000;
            var proc = Process.Start("notepad.exe");
            this.MainWin = new Window(proc);
            this.Editor = ((dynamic)this.MainWin)[AutomationId: "15"];
            this.MenuBar = ((dynamic)this.MainWin).MenuBar;

            var path = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system.ini");
            this.SampleText = File.ReadAllText(path);
        }

        [TestMethod]
        public void InputText()
        {
            this.Editor.SetText(this.SampleText, TextInputMode.SendKey);
            Assert.IsTrue(this.Editor.Text.Equals(this.SampleText));
        }

        [TestMethod]
        public void PasteText()
        {
            this.Editor.SetText(this.SampleText, TextInputMode.Paste);
            Assert.IsTrue(this.Editor.Text.Equals(this.SampleText));
        }

        [TestMethod]
        public void InvokeMenuTest()
        {
            ((dynamic)this.MenuBar)["File"]["Open..."].Invoke();

            var confirmDlg = ((dynamic)this.MainWin)["Notepad", "#32770", ControlType.Window];
            if (confirmDlg != null)
                confirmDlg.CommandButton_7.Click();

            var dialog = ((dynamic)this.MainWin)["Open", "#32770", ControlType.Window, LocalizedControlType: "Dialog"];
            Assert.IsTrue(dialog != null);

            dialog.Close.Click();
            Sync.TryWaitFor(() =>
            {
                dialog = ((dynamic)this.MainWin)["Open", "#32770", ControlType.Window, LocalizedControlType: "Dialog"];
                return dialog == null;
            });
            Assert.IsTrue(dialog == null);
        }

        [TestMethod]
        public void MenuBarTest()
        {
            foreach (var item in this.MenuBar.Items)
            {
                Assert.IsTrue(true, item.Name);
                var menu = item.Expand();
                foreach (var m in menu.Items)
                {
                    Assert.IsTrue(true, "\t" + m.Name);
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            dynamic win = this.MainWin;
            Button btnClose = win.Close;
            btnClose.Click();

            var confirmDlg = win["Notepad", "#32770", ControlType.Window];
            if (confirmDlg != null)
                confirmDlg.CommandButton_7.Click();
        }

    }
}
