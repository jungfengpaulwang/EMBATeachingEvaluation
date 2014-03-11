using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace TeachingEvaluation.PrivateControl
{
    /// <summary>
    /// NavContentPresentation中DetailPane的顯示項目
    /// </summary>
    public partial class DescriptionPane : UserControl
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public DescriptionPane()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if ( DesignMode )
                this.ResizeRedraw = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if ( DesignMode )
            {
                e.Graphics.DrawRectangle(new Pen(Brushes.Gray, 1.0f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            }
        }
        /// <summary>
        /// 取得資料擁有者的索引鍵值
        /// </summary>
        public string PrimaryKey { get; private set; }
        /// <summary>
        /// 當PrimaryKey屬性變更時
        /// </summary>
        public event EventHandler PrimaryKeyChanged;
        /// <summary>
        /// 引發PrimaryKeyChanged事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnPrimaryKeyChanged(EventArgs e)
        {
            if ( PrimaryKeyChanged != null )
                PrimaryKeyChanged(this, e);
        }
        internal void SetPrimaryKey(string primaryKey)
        {
            PrimaryKey = primaryKey;
            OnPrimaryKeyChanged(new EventArgs());
        }
    }
}
