using System;
using System.Collections.Generic;
using System.Text;
using DevComponents.DotNetBar;
using System.Drawing;
using System.Windows.Forms;
using FISCA.Presentation;

namespace TeachingEvaluation.PrivateControl
{
    class DetailItemContainer : ExpandablePanel
    {
        static DetailItemContainer()
        {
            //DotNetBarReferenceFixer.FixIt();
        }
        private LinkLabel savebutton;
        private LinkLabel undobutton;
        private LinkLabel copybutton;
        private LinkLabel deletebutton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        protected System.Windows.Forms.PictureBox pictureBox1;
        //private PictureBox
        private List<TeachingEvaluation.PrivateControl.DetailContent> items = new List<TeachingEvaluation.PrivateControl.DetailContent>();
        private string _PrimaryKey = "";
        private bool _Loaded = false;

        public DetailItemContainer()
        {
            Style.BackColor1.Color = Color.Red;
            InitializeComponent();
            this.flowLayoutPanel1.SizeChanged += delegate
            {
                this.Height = this.TitleHeight + flowLayoutPanel1.Height;
                int x = ( this.Width - pictureBox1.Width ) / 2;
                int y = ( this.Height - this.TitleHeight - pictureBox1.Height ) / 2 + this.TitleHeight;
                if ( x < 0 ) x = 0;
                if ( y < 0 ) y = 0;
                pictureBox1.Location = new Point(x, y);
            };
            this.ExpandedChanged += delegate
            {
                if ( this.Expanded )
                {
                    item_LoadingChanged(null, null);
                }
                else
                {
                    pictureBox1.Visible = false;
                }
            };
            this.VisibleChanged += delegate
            {
                if ( !_Loaded ) Load();
            };
        }

        public void SetPrimaryKey(string key)
        {
            if ( key != _PrimaryKey )
            {
                _PrimaryKey = key;
                Load();
            }
        }

        public void Reload()
        {
            Load();
        }

        private void Load()
        {
            _Loaded = false;
            if ( Visible == false )
                return;
            foreach ( var item in items )
            {
                item.PrimaryKey = _PrimaryKey;
            }
            _Loaded = true;
        }

        public void RemoveContent(PrivateControl.DetailContent item)
        {
            if (item == null) return;

            flowLayoutPanel1.Controls.Remove(item);
        }


        public void AddContent(PrivateControl.DetailContent item)
        {
            if ( item == null ) return;

            this.TitleText = item.Caption;

            item.Margin = new System.Windows.Forms.Padding(0);
            item.ContentValidatedChanged += new EventHandler(item_SaveButtonVisibleChanged);
            item.SaveButtonVisibleChanged += new EventHandler(item_SaveButtonVisibleChanged);
            item.CancelButtonVisibleChanged += new EventHandler(item_CancelButtonVisibleChanged);
            item.LoadingChanged += new EventHandler(item_LoadingChanged);
            item.LoadingChanged += new EventHandler(item_SaveButtonVisibleChanged);
            item.LoadingChanged += new EventHandler(item_CancelButtonVisibleChanged);
            //item.BackColor = Color.Transparent;

            items.Add(item);

            flowLayoutPanel1.Controls.Add(item);
        }

        public List<PrivateControl.DetailContent> GetContents()
        {
            return this.items;
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.savebutton = new System.Windows.Forms.LinkLabel();
            this.undobutton = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.copybutton = new System.Windows.Forms.LinkLabel();
            this.deletebutton = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::TeachingEvaluation.Properties.Resources.Loading;
            this.pictureBox1.Location = new System.Drawing.Point(77, 208);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // savebutton
            // 
            this.savebutton.AutoSize = true;
            this.savebutton.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(133)))), ((int)(((byte)(2)))));
            this.savebutton.Location = new System.Drawing.Point(2, 6);
            this.savebutton.Name = "savebutton";
            this.savebutton.Size = new System.Drawing.Size(0, 17);
            this.savebutton.TabIndex = 19;
            this.savebutton.TabStop = true;
            this.savebutton.Visible = false;
            this.savebutton.Click += new System.EventHandler(this.savebutton_Click);
            // 
            // undobutton
            // 
            this.undobutton.AutoSize = true;
            this.undobutton.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(133)))), ((int)(((byte)(2)))));
            this.undobutton.Location = new System.Drawing.Point(35, 6);
            this.undobutton.Name = "undobutton";
            this.undobutton.Size = new System.Drawing.Size(0, 17);
            this.undobutton.TabIndex = 20;
            this.undobutton.TabStop = true;
            this.undobutton.Visible = false;
            this.undobutton.Click += new System.EventHandler(this.undobutton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 30);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(554, 10);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // copybutton
            // 
            this.copybutton.AutoSize = true;
            this.copybutton.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(133)))), ((int)(((byte)(2)))));
            this.copybutton.Location = new System.Drawing.Point(68, 6);
            this.copybutton.Name = "copybutton";
            this.copybutton.Size = new System.Drawing.Size(0, 17);
            this.copybutton.TabIndex = 19;
            this.copybutton.TabStop = true;
            this.copybutton.Visible = true;
            this.copybutton.Text = "複製";
            this.copybutton.Click += new System.EventHandler(this.copybutton_Click);
            // 
            // deletebutton
            // 
            this.deletebutton.AutoSize = true;
            this.deletebutton.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(133)))), ((int)(((byte)(2)))));
            this.deletebutton.Location = new System.Drawing.Point(101, 6);
            this.deletebutton.Name = "deletebutton";
            this.deletebutton.Size = new System.Drawing.Size(0, 17);
            this.deletebutton.TabIndex = 19;
            this.deletebutton.TabStop = true;
            this.deletebutton.Visible = true;
            this.deletebutton.Text = "移除";
            this.deletebutton.Click += new System.EventHandler(this.deletebutton_Click);
            // 
            // DetailItemContainer
            // 
            this.AutoSize = true;
            this.ButtonImageCollapse = global::TeachingEvaluation.Properties.Resources.expandablebuttonimagecollapse;
            this.ButtonImageExpand = global::TeachingEvaluation.Properties.Resources.expandablebuttonimageexpand;
            this.CanvasColor = System.Drawing.SystemColors.Control;
            this.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.savebutton);
            this.Controls.Add(this.undobutton);
            this.Controls.Add(this.copybutton);
            this.Controls.Add(this.deletebutton);
            this.Controls.Add(this.flowLayoutPanel1);
            this.ExpandOnTitleClick = true;
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "expandablePanel1";
            this.Size = new System.Drawing.Size(554, 54);
            this.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.Style.BackColor1.Color = System.Drawing.Color.White;
            this.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.Style.BorderColor.Color = System.Drawing.Color.White;
            this.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.Style.GradientAngle = 90;
            this.Style.WordWrap = true;
            this.TitleHeight = 30;
            this.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.TitleStyle.BackColor1.Color = System.Drawing.Color.Transparent;
            this.TitleStyle.BackColor2.Color = System.Drawing.Color.Transparent;
            this.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.TitleStyle.BorderSide = DevComponents.DotNetBar.eBorderSide.Bottom;
            this.TitleStyle.ForeColor.Color = System.Drawing.Color.DimGray;
            this.TitleStyle.GradientAngle = 90;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void item_LoadingChanged(object sender, EventArgs e)
        {
            bool loading = false;
            foreach ( var item in items )
            {
                if ( item.Loading )
                {
                    loading = true;
                    break;
                }
            }
            if ( loading )
            {
                //this.flowLayoutPanel1.Enabled = false;
                //this.flowLayoutPanel1.Visible=false;
                this.pictureBox1.Visible = this.Expanded;
            }
            else
            {
                //this.flowLayoutPanel1.Enabled = true;
                //this.flowLayoutPanel1.Visible = true;
                this.pictureBox1.Visible = false;
            }
        }
        private void item_SaveButtonVisibleChanged(object sender, EventArgs e)
        {
            bool visible = false;
            foreach ( var item in items )
            {
                if ( !item.ContentValidated | item.Loading )
                {
                    visible = false;
                    break;
                }
                if ( item.SaveButtonVisible )
                {
                    visible = true;
                }
            }
            savebutton.Visible = visible;
            if ( visible )
            {
                savebutton.Text = "儲存";
            }
            else
            {
                savebutton.Text = "";
            }
        }
        private void item_CancelButtonVisibleChanged(object sender, EventArgs e)
        {
            bool visible = false;
            foreach ( var item in items )
            {
                if ( item.Loading )
                {
                    visible = false;
                    break;
                }
                if ( item.CancelButtonVisible )
                {
                    visible = true;
                }
            }
            undobutton.Visible = visible;
            if ( visible )
            {
                undobutton.Text = "取消";
            }
            else
            {
                undobutton.Text = "";
            }
        }
        private void savebutton_Click(object sender, System.EventArgs e)
        {
            foreach ( var item in items )
            {
                if ( item.SaveButtonVisible )
                    item.Save();
            }
        }
        private void undobutton_Click(object sender, System.EventArgs e)
        {
            foreach ( var item in items )
            {
                if ( item.CancelButtonVisible )
                    item.Undo();
            }
        }
        private void copybutton_Click(object sender, System.EventArgs e)
        {
            foreach (var item in items)
            {
                item.Copy();
            }
        }
        private void deletebutton_Click(object sender, System.EventArgs e)
        {
            foreach (var item in new List<DetailContent>(items))
            {
                item.Delete();
            }
        }
    }
}
