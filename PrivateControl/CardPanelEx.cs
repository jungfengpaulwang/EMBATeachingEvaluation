using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.DotNetBar;
using System.Windows.Forms.Layout;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace TeachingEvaluation.PrivateControl
{
    /// <summary>
    /// 以卡片形式Layout子控制項的Form
    /// </summary>
    class CardPanelEx : PanelEx
    {
        //static CardPanelEx()
        //{
        //    //DotNetBarReferenceFixer.FixIt();
        //}
        private CardLayoutEngine _layoutEngine;
        public override LayoutEngine LayoutEngine
        {
            get
            {
                if (_layoutEngine == null)
                {
                    _layoutEngine = new CardLayoutEngine();
                }

                return _layoutEngine;
            }
        }
        /// <summary>
        /// 建構子
        /// </summary>
        public CardPanelEx()
        {
            //this.SuspendPaint = false;
            this.Click += new EventHandler(CardPanelEx_Click);
            //this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer & ControlStyles.UserPaint & ControlStyles.AllPaintingInWmPaint, true);
            //this.UpdateStyles();
        }

        protected override Point ScrollToControl(System.Windows.Forms.Control activeControl)
        {
            if ( activeControl is FISCA.Presentation.DetailContent )//取消DetailContent引發的自動捲動事件
                return this.AutoScrollPosition;
            return base.ScrollToControl(activeControl);
        }

        private void CardPanelEx_Click(object sender, EventArgs e)
        {
            this.Focus();
        }
        /// <summary>
        /// 每個子控制項的寬度
        /// </summary>
        public int CardWidth
        {
            get { return _layoutEngine.CardWidth; }
            set { _layoutEngine.CardWidth = value; }
        }
        /// <summary>
        /// Layout的最小寬度
        /// </summary>
        public int MinWidth
        {
            get { return _layoutEngine.MinWidth; }
            set { _layoutEngine.MinWidth = value; }
        }

        //protected override void OnControlAdded(ControlEventArgs e)
        //{
        //    base.OnControlAdded(e);
        //    LayoutControls(this);
        //}

        //protected override void OnControlRemoved(ControlEventArgs e)
        //{
        //    base.OnControlRemoved(e);
        //    LayoutControls(this);
        //}

        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    LayoutControls(this);
        //}

        //protected override void OnVisibleChanged(EventArgs e)
        //{
        //    base.OnVisibleChanged(e);
        //    LayoutControls(this);
        //}

        //protected override void OnAutoSizeChanged(EventArgs e)
        //{
        //    base.OnAutoSizeChanged(e);
        //    LayoutControls(this);
        //}

        //private void LayoutControls(object container)
        //{
        //    int Columns, ColumnWidth, i;
        //    CardPanelEx parent = container as CardPanelEx; 
        //    Rectangle parentDisplayRectangle = parent.DisplayRectangle;
        //    Point nextControlLocation = parentDisplayRectangle.Location;
        //    Dictionary<System.Windows.Forms.Control, Point> controlLocation = new Dictionary<System.Windows.Forms.Control, Point>();
        //    //計算可容量的行數
        //    Columns = CardWidth == 0 ? 1 : (parentDisplayRectangle.Width - MinWidth) / (CardWidth + MinWidth);
        //    if (Columns == 0)
        //    {
        //        Columns = 1;
        //        ColumnWidth = 0;
        //    }
        //    else
        //    {
        //        //計算每行行距
        //        ColumnWidth = (parentDisplayRectangle.Width - Columns * CardWidth - 5) / (Columns + 1);
        //    }
        //    //每行各自獨立Layout
        //    int[] nextColumnY = new int[Columns];
        //    for (i = 0; i < Columns; i++)
        //    {
        //        nextColumnY[i] = parentDisplayRectangle.Y + MinWidth;
        //    }
        //    //移至第一行
        //    nextControlLocation.Offset(ColumnWidth, MinWidth);

        //    i = 0;
        //    //開始排列每一個包含的控制項
        //    foreach (System.Windows.Forms.Control c in parent.Controls)
        //    {
        //        //控制項不顯示當沒看到
        //        if (!c.Visible)
        //        {
        //            continue;
        //        }
        //        //控制項位置需要變更
        //        if (c.Location.X != nextControlLocation.X || c.Location.Y != nextControlLocation.Y)
        //        {
        //            //如果顯示區域步購大則加大顯示區域
        //            if (parentDisplayRectangle.Height < nextControlLocation.Y + c.Height + MinWidth)
        //            {
        //                parentDisplayRectangle.Inflate(0, c.Height + MinWidth);
        //            }
        //            //設定控制項位置
        //            controlLocation.Add(c, nextControlLocation);
        //            nextControlLocation = new Point(nextControlLocation.X, nextControlLocation.Y);
        //        }
        //        //將第i行的高度往下增加並將i推進至下一行
        //        nextColumnY[i++] += c.Height + MinWidth;

        //        if (i == Columns)
        //        {
        //            i = 0;
        //            nextControlLocation.X = parentDisplayRectangle.Location.X + ColumnWidth;
        //        }
        //        else
        //        {
        //            nextControlLocation.X += (ColumnWidth + CardWidth);
        //        }
        //        nextControlLocation.Y = nextColumnY[i];
        //    }
        //    //真的把位置設定上去
        //    foreach (System.Windows.Forms.Control c in controlLocation.Keys)
        //    {
        //        c.Location = controlLocation[c];
        //    }
        //}

        private class CardLayoutEngine : LayoutEngine
        {
            private int _CardWidth;

            private int _MinWidth = 2;

            public int CardWidth
            {
                get { return _CardWidth; }
                set { _CardWidth = value; }
            }

            public int MinWidth
            {
                get { return _MinWidth; }
                set { _MinWidth = value; }
            }

            public CardLayoutEngine()
            {
            }

            private List<System.Windows.Forms.Control> _SuspendLayoutList = new List<System.Windows.Forms.Control>();

            public override bool Layout(
                object container,
                LayoutEventArgs layoutEventArgs)
            {
                //return false;
                if ( !( (CardPanelEx)container ).Visible ) return false;
                if ((layoutEventArgs.AffectedControl == null) || (layoutEventArgs.AffectedProperty == null)) return false;
                if (layoutEventArgs.AffectedProperty != "Visible" && layoutEventArgs.AffectedProperty != "Bounds" && layoutEventArgs.AffectedProperty != "Parent") return false;

                //Thread.SpinWait(1);
                CardPanelEx parent = container as CardPanelEx;
                //if (parent.SuspendPaint)
                // Use DisplayRectangle so that parent.Padding is honored.
                Rectangle parentDisplayRectangle = parent.DisplayRectangle;
                Point nextControlLocation = parentDisplayRectangle.Location;
                Dictionary<System.Windows.Forms.Control, Point> controlLocation = new Dictionary<System.Windows.Forms.Control, Point>();
                int Columns, ColumnWidth, i;
                //計算可容量的行數
                Columns = _CardWidth == 0 ? 1 : ( parentDisplayRectangle.Width - _MinWidth ) / ( _CardWidth + _MinWidth );
                if ( Columns == 0 )
                {
                    Columns = 1;
                    ColumnWidth = 0;
                }
                else
                {
                    //計算每行行距
                    ColumnWidth = ( parentDisplayRectangle.Width - Columns * _CardWidth - 5 ) / ( Columns + 1 );
                }
                //每行各自獨立Layout
                int[] nextColumnY = new int[Columns];
                for ( i = 0 ; i < Columns ; i++ )
                {
                    nextColumnY[i] = parentDisplayRectangle.Y + _MinWidth;
                }
                //移至第一行
                nextControlLocation.Offset(ColumnWidth, _MinWidth);

                i = 0;
                //開始排列每一個包含的控制項
                foreach (System.Windows.Forms.Control c in parent.Controls)
                {
                    //控制項不顯示當沒看到
                    if ( !c.Visible )
                    {
                        continue;
                    }
                    //控制項位置需要變更
                    if ( c.Location.X != nextControlLocation.X || c.Location.Y != nextControlLocation.Y )
                    {
                        //如果顯示區域步購大則加大顯示區域
                        if ( parentDisplayRectangle.Height < nextControlLocation.Y + c.Height + _MinWidth )
                        {
                            parentDisplayRectangle.Inflate(0, c.Height + _MinWidth);
                        }
                        //設定控制項位置
                        controlLocation.Add(c, nextControlLocation);
                        nextControlLocation = new Point(nextControlLocation.X, nextControlLocation.Y);
                    }
                    //將第i行的高度往下增加並將i推進至下一行
                    nextColumnY[i++] += c.Height + _MinWidth;

                    if ( i == Columns )
                    {
                        i = 0;
                        nextControlLocation.X = parentDisplayRectangle.Location.X + ColumnWidth;
                    }
                    else
                    {
                        nextControlLocation.X += ( ColumnWidth + _CardWidth );
                    }
                    nextControlLocation.Y = nextColumnY[i];
                }
                //真的把位置設定上去
                foreach (System.Windows.Forms.Control c in controlLocation.Keys)
                {
                    c.Location = controlLocation[c];
                }
                return false;
            }
        }

    }
}
