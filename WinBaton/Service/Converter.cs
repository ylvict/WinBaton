using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WinBaton.Control;

namespace WinBaton.Service
{
    public static class Converter
    {
        public static IControl ConverUIAElementToBatonCtrl(AutomationElement element)
        {
            string ctrlName = element.Current.ControlType.ProgrammaticName.Split('.').Last().Trim();
            var obj = (IControl)Activator.CreateInstance(Type.GetType("WinBaton.Control." + ctrlName), element);
            return obj;
            #region controls
            //  *"Button"
            //  "Calendar"
            //  "CheckBox"
            //  "ComboBox"
            //  "Custom"
            //  "DataGrid"
            //  "DataItem"
            //  "Document"
            //  "Edit"
            //  "Group"
            //  "Header"
            //  "HeaderItem"
            //  "Hyperlink"
            //  "Image"
            //  "List"
            //  "ListItem"
            //  "Menu"
            //  *"MenuBar"
            //  "MenuItem"
            //  "Pane"
            //  "ProgressBar"
            //  "RadioButton"
            //  "ScrollBar"
            //  "Separator"
            //  "Slider"
            //  "Spinner"
            //  "SplitButton"
            //  "StatusBar"
            //  "Tab"
            //  "TabItem"
            //  "Table"
            //  *"Text"
            //  "Thumb"
            //  "TitleBar"
            //  "ToolBar"
            //  "ToolTip"
            //  "Tree"
            //  "TreeItem"
            //  *"Window"
            #endregion
        }
    }
}
