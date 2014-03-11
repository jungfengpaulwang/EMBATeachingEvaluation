using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeachingEvaluation.Event
{
    class DeliverDetailContent
    {
        public DeliverDetailContent()
        {

        }

        internal static void RaiseCopyDetailContentEvent(object sender, DeliverDetailContentEventArgs e)
        {
            if (DeliverDetailContent.CopyDetailContent != null)
                DeliverDetailContent.CopyDetailContent(sender, e);
        }

        internal static void RaiseDeleteDetailContentEvent(object sender, DeliverDetailContentEventArgs e)
        {
            if (DeliverDetailContent.DeleteDetailContent != null)
                DeliverDetailContent.DeleteDetailContent(sender, e);
        }

        internal static void RaisePassDetailContentEvent(object sender, DeliverDetailContentEventArgs e)
        {
            if (DeliverDetailContent.PassDetailContent != null)
                DeliverDetailContent.PassDetailContent(sender, e);
        }

        internal static event EventHandler<DeliverDetailContentEventArgs> CopyDetailContent;
        internal static event EventHandler<DeliverDetailContentEventArgs> DeleteDetailContent;
        internal static event EventHandler<DeliverDetailContentEventArgs> PassDetailContent;
    }
}
