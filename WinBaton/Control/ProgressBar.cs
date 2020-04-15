using System.Windows.Automation;

namespace WinBaton.Control
{
    public class ProgressBar : IControl
    {
        private readonly AutomationElement progressBar = null;

        private readonly RangeValuePattern RangeValue = null;

        public ProgressBar(AutomationElement button)
        {
            this.progressBar = button;
            this.RangeValue = progressBar.GetCurrentPattern(RangeValuePattern.Pattern) as RangeValuePattern;
        }

        public double Value => RangeValue?.Current.Value ?? 0;

        public bool IsCompleted => RangeValue?.Current.Value >= RangeValue?.Current.Maximum;
    }
}