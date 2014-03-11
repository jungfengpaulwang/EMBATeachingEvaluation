using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//using System.Windows.Forms.Design.Behavior;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace TeachingEvaluation.PrivateControl
{
    /// <summary>
    /// NavContentPresentation中DetailPane的顯示項目
    /// </summary>
    public partial class DetailContent : UserControl
    {
        private bool _SaveButtonVisible = false;
        private bool _CancelButtonVisible = false;
        private bool _Validated = true;
        private bool _Loading = false;
        //private string _PrimaryKey = "";
        /// <summary>
        /// 建構子
        /// </summary>
        public DetailContent()
        {
            InitializeComponent();
            Group = this.GetType().Name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (DesignMode)
                this.ResizeRedraw = true;
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        protected internal bool Is_Required { set; get; }

        /// <summary>
        /// 是否為個案題
        /// </summary>
        protected internal bool Is_Case { set; get; }

        /// <summary>
        /// 是否為問答題
        /// </summary>
        protected internal bool Is_SelfAssessment { set; get; }

        /// <summary>
        /// 是否不列入評鑑值計算
        /// </summary>
        protected internal bool Is_NoneCalculated { set; get; }

        /// <summary>
        /// 題目內容
        /// </summary>
        protected internal UDT.Question Record { get; set; }

        /// <summary>
        /// 答案選項內容
        /// </summary>
        protected internal List<UDT.QuestionOption> RecordDetails { set; get; }

        /// <summary>
        /// 題目層次標題
        /// </summary>
        protected internal string HierarchyTitle { set; get; }

        /// <summary>
        /// 題目標題資料來源
        /// </summary>
        protected internal List<string> HierarchyTitleSource { set; get; }

        /// <summary>
        /// 資料異動監控
        /// </summary>
        protected internal Campus.Windows.ChangeListen DataListener { get; set; }

        /// <summary>
        /// 問卷樣版
        /// </summary>
        protected internal int SurveyID { set; get; }

        /// <summary>
        /// 題型
        /// </summary>
        protected internal string  Type { get; set; }

        /// <summary>
        /// 題號
        /// </summary>
        protected internal decimal? DisplayOrder { get; set; }

        /// <summary>
        /// 是否已使用
        /// </summary>
        protected internal bool Dirty { get; set; }

        /// <summary>
        /// 取得或設訂資料項目所屬類別
        /// </summary>
        protected internal string Group { get; set; }

        /// <summary>
        /// 取代原Group，資料項目之標題
        /// </summary>
        protected internal string Caption { get; set; }

        /// <summary>
        /// 資料項目副標題
        /// </summary>
        protected internal string Title { get; set; }

        /// <summary>
        /// 資料繫結
        /// </summary>
        public virtual void DataBind() { }

        /// <summary>
        /// 取得或設定儲存按鈕顯示或隱藏
        /// </summary>
        public bool SaveButtonVisible
        {
            get
            {
                return _SaveButtonVisible;
            }
            set
            {
                if (_SaveButtonVisible != value)
                {
                    _SaveButtonVisible = value;
                    OnSaveButtonVisibleChanged(new EventArgs());
                }
            }
        }
        /// <summary>
        /// 取得或設定取消按鈕顯示或隱藏
        /// </summary>
        public bool CancelButtonVisible
        {
            get
            {
                return _CancelButtonVisible;
            }
            set
            {
                if (_CancelButtonVisible != value)
                {
                    _CancelButtonVisible = value;
                    OnCancelButtonVisibleChanged(new EventArgs());
                }
            }
        }
        /// <summary>
        /// 取得或設定內容驗證結果
        /// </summary>
        public bool ContentValidated
        {
            get { return _Validated; }
            set
            {
                if (_Validated != value)
                {
                    _Validated = value;
                    OnContentValidatedChanged(new EventArgs());
                }
            }
        }
        /// <summary>
        /// 取得資料擁有者的索引鍵值
        /// </summary>
        protected internal string PrimaryKey { get; set; }

        /// <summary>
        /// 取得或設定，資料正在讀取中
        /// </summary>
        public bool Loading
        {
            get
            {
                return _Loading;
            }
            set
            {
                if (_Loading != value)
                {
                    _Loading = value;
                    OnLoadingChanged(new EventArgs());
                }
            }
        }
        /// <summary>
        /// 儲存
        /// </summary>
        internal void Save()
        {
            OnSaveButtonClick(new EventArgs());
        }
        /// <summary>
        /// 取消變更
        /// </summary>
        internal void Undo()
        {
            OnCancelButtonClick(new EventArgs());
        }
        /// <summary>
        /// 複製
        /// </summary>
        internal void Copy()
        {
            OnCopyButtonClick(new EventArgs());
        }
        /// <summary>
        /// 移除
        /// </summary>
        internal void Delete()
        {
            OnDeleteButtonClick(new EventArgs());
        }
        /// <summary>
        /// 當SaveButtonVisible屬性改變時
        /// </summary>
        public event EventHandler SaveButtonVisibleChanged;
        /// <summary>
        /// 當CancelButtonVisible屬性改變時
        /// </summary>
        public event EventHandler CancelButtonVisibleChanged;
        ///// <summary>
        ///// 當PrimaryKey屬性變更時
        ///// </summary>
        //public event EventHandler PrimaryKeyChanged;
        /// <summary>
        /// 當Validated屬性變更時
        /// </summary>
        public event EventHandler ContentValidatedChanged;
        /// <summary>
        /// 當Loading屬性變更時
        /// </summary>
        public event EventHandler LoadingChanged;
        /// <summary>
        /// 按下SaveButton時
        /// </summary>
        public event EventHandler SaveButtonClick;
        /// <summary>
        /// 按下CancelButton時
        /// </summary>
        public event EventHandler CancelButtonClick;
        /// <summary>
        /// 按下CopyButton時
        /// </summary>
        public event EventHandler CopyButtonClick;
        /// <summary>
        /// 按下DeleteButton時
        /// </summary>
        public event EventHandler DeleteButtonClick;

        /// <summary>
        /// 引發SaveButtonVisibleChanged事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnSaveButtonVisibleChanged(EventArgs e)
        {
            if (SaveButtonVisibleChanged != null)
                SaveButtonVisibleChanged(this, e);
        }
        /// <summary>
        /// 引發CancelButtonVisibleChanged事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnCancelButtonVisibleChanged(EventArgs e)
        {
            if (CancelButtonVisibleChanged != null)
                CancelButtonVisibleChanged(this, e);
        }
        /// <summary>
        /// 引發ContentValidatedChanged事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnContentValidatedChanged(EventArgs e)
        {
            if (ContentValidatedChanged != null)
                ContentValidatedChanged(this, e);
        }
        ///// <summary>
        ///// 引發PrimaryKeyChanged事件
        ///// </summary>
        ///// <param name="e">包含事件的資料</param>
        //protected virtual void OnPrimaryKeyChanged(EventArgs e)
        //{
        //    if (PrimaryKeyChanged != null)
        //        PrimaryKeyChanged(this, e);
        //}
        /// <summary>
        /// 引發LoadingChanged事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnLoadingChanged(EventArgs e)
        {
            if (LoadingChanged != null)
                LoadingChanged(this, e);
        }
        /// <summary>
        /// 引發SaveButtonClick事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnSaveButtonClick(EventArgs e)
        {
            if (SaveButtonClick != null)
                SaveButtonClick(this, e);
        }
        /// <summary>
        /// 引發CancelButtonClick事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnCancelButtonClick(EventArgs e)
        {
            if (CancelButtonClick != null)
                CancelButtonClick(this, e);
        }
        /// <summary>
        /// 引發CopyButtonClick事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnCopyButtonClick(EventArgs e)
        {
            if (CopyButtonClick != null)
                CopyButtonClick(this, e);
        }
        /// <summary>
        /// 引發DeleteButtonClick事件
        /// </summary>
        /// <param name="e">包含事件的資料</param>
        protected virtual void OnDeleteButtonClick(EventArgs e)
        {
            if (DeleteButtonClick != null)
                DeleteButtonClick(this, e);
        }

        private void DetailContent_SizeChanged(object sender, EventArgs e)
        {
            this.Width = 550;
            if (this.DesignMode)
            {
                this.Height -= this.Height % 5;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            if (DesignMode)
            {
                e.Graphics.DrawRectangle(new Pen(Brushes.Gray, 1.0f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            }
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    Graphics g = e.Graphics;

        //    //注意，这里千万不可用Graphics g = this.CreateGraphics() 获得绘图变量。否则闪烁将非常厉害。

        //    Bitmap b = new Bitmap(this.Width, this.Height);
        //    Graphics dc = Graphics.FromImage((System.Drawing.Image)b);

        //    //将要绘制的内容绘制到dc上

        //    g.DrawImage(b, 0, 0);
        //    dc.Dispose();
        //}
    }
}
