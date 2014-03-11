using System;

namespace TeachingEvaluation.PrivateControl
{
    /// <summary>
    /// 選課開放時間更新通知
    /// </summary>
    public class Observer
    {
        public static void RaiseUpdateLayoutEvent(object sender, EventArgs e)
        {
            if (Observer.OnUpdateLayout != null)
                Observer.OnUpdateLayout(sender, e);
        }

        public static void RaiseDataBindEvent(object sender, EventArgs e)
        {
            if (Observer.OnDataBind != null)
                Observer.OnDataBind(sender, e);
        }

        public static event EventHandler<EventArgs> OnUpdateLayout;
        public static event EventHandler<EventArgs> OnDataBind;
    }
}