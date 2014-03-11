using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeachingEvaluation.Event
{
    public class DeliverDetailContentEventArgs : EventArgs
    {
        private PrivateControl.DetailContent Detail_Content;
        public PrivateControl.DetailContent DetailContent
        {
            get
            {
                return Detail_Content;
            }
        }
        public DeliverDetailContentEventArgs(PrivateControl.DetailContent detail_content)
        {
            this.Detail_Content = detail_content;
        }
    }
}
