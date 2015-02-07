using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace WinBaton.Control
{
    public enum TextInputMode
    {
        Paste,
        SendKey
    }

    public class Document : IControl
    {
        private AutomationElement element = null;

        public Document(AutomationElement element) { this.element = element; }

        public string Text
        {
            get
            {
                var pattern = (TextPattern)this.element.GetCurrentPattern(TextPattern.Pattern);
                return pattern.DocumentRange.GetText(-1);
            }
        }

        [STAThread]
        public void SetText(string value, TextInputMode mode)
        {
            var pattern = (TextPattern)this.element.GetCurrentPattern(TextPattern.Pattern);
            this.element.SetFocus();
            pattern.DocumentRange.Select();
            System.Windows.Forms.SendKeys.SendWait("{BS}");
            switch (mode)
            {
                case TextInputMode.Paste:
                    Thread thread = new Thread(() => { Clipboard.SetText(value); })
                    { ApartmentState = ApartmentState.STA };
                    thread.Start();
                    while (thread.ThreadState == ThreadState.Running) Thread.Sleep(100);
                    SendKeys.SendWait("^v");
                    break;
                case TextInputMode.SendKey:
                    string text = "";
                    for (int i = 0; i < value.Length; i++)
                    {
                        var item = value[i];
                        if (item == '{') { text += "{{}"; continue; }
                        if (item == '}') { text += "{}}"; continue; }
                        if (item == '~') { text += "{~}"; continue; }
                        if (item == '+') { text += "{+}"; continue; }
                        if (item == '^') { text += "{^}"; continue; }
                        if (item == '%') { text += "{%}"; continue; }
                        if (item == '\r' && value[i + 1] == '\n') { text += "\n"; i++; continue; }
                        text += item;
                    }
                    SendKeys.SendWait(text);
                    break;
                default:
                    break;
            }
        }
    }
}
