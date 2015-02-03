using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using WinBaton.Service;

namespace WinBaton.Control
{
    public abstract class Container : DynamicObject, IControl
    {
        protected AutomationElement element = null;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var ctrlList = this.element.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, binder.Name));
            if (ctrlList.Count <= 0)
            {
                throw new KeyNotFoundException("Cannot found element by autoId.");
            }
            if (ctrlList.Count > 1)
            {
                throw new DuplicateWaitObjectException("Duplicate item found by autoId.");
            }
            result = Converter.ConverUIAElementToBatonCtrl(ctrlList[0]);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length <= 0)
            {
                result = null;
                return false;
            }

            List<PropertyCondition> cond = new List<PropertyCondition>();
            int i = 0;
            for (; i < indexes.Length - binder.CallInfo.ArgumentNames.Count; i++)
            {
                switch (i)
                {
                    case 0: //First param without name, by default NameProperty
                        cond.Add(new PropertyCondition(AutomationElement.NameProperty, indexes[i]));
                        break;
                    case 1: //Second param without name, by default ClassNameProperty
                        cond.Add(new PropertyCondition(AutomationElement.ClassNameProperty, indexes[i]));
                        break;
                    case 2: //Third param without name, by default ControlTypeProperty
                        cond.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, indexes[i]));
                        break;
                    default:
                        break;
                }

            }
            for (var j = 0; j < binder.CallInfo.ArgumentNames.Count; j++, i++)
            {
                var propertyName = binder.CallInfo.ArgumentNames[j];
                var property = (AutomationProperty)typeof(AutomationElement)
                    .GetField(propertyName + "Property").GetValue(null);
                cond.Add(new PropertyCondition(property, indexes[i]));
            }

            AutomationElementCollection ctrlList = null;
            Sync.TryWaitFor(() =>
            {
                if (ctrlList != null && ctrlList.Count > 0) return true;
                if (cond.Count == 1)
                    ctrlList = this.element.FindAll(TreeScope.Children, cond[0]);
                else
                    ctrlList = this.element.FindAll(TreeScope.Children, new AndCondition(cond.ToArray()));
                return false;
            });
            result = ctrlList
                .Cast<AutomationElement>()
                .Select(x => Converter.ConverUIAElementToBatonCtrl(x)).FirstOrDefault();
            return true;
        }
    }
}
