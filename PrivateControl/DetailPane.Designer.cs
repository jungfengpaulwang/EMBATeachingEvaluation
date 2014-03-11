namespace TeachingEvaluation.PrivateControl
{
    partial class DetailPane
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.lnkAddMultiChoice = new System.Windows.Forms.LinkLabel();
            this.btnPreview = new DevComponents.DotNetBar.ButtonX();
            this.itemContainer2 = new DevComponents.DotNetBar.ItemContainer();
            this.lnkAddEssay = new System.Windows.Forms.LinkLabel();
            this.lnkAddSingleChoice = new System.Windows.Forms.LinkLabel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
            this.controlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.panel1 = new DevComponents.DotNetBar.PanelEx();
            this.panProgress = new DevComponents.DotNetBar.PanelEx();
            this.circleProjress = new System.Windows.Forms.PictureBox();
            this.DetailItemContainers = new TeachingEvaluation.PrivateControl.CardPanelEx();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.ContentLinks = new System.Windows.Forms.TableLayoutPanel();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.panelEx1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circleProjress)).BeginInit();
            this.panelEx3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.lnkAddMultiChoice);
            this.panelEx1.Controls.Add(this.btnPreview);
            this.panelEx1.Controls.Add(this.lnkAddEssay);
            this.panelEx1.Controls.Add(this.lnkAddSingleChoice);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(815, 36);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 270;
            this.panelEx1.TabIndex = 0;
            // 
            // lnkAddMultiChoice
            // 
            this.lnkAddMultiChoice.AutoSize = true;
            this.lnkAddMultiChoice.Location = new System.Drawing.Point(176, 10);
            this.lnkAddMultiChoice.Name = "lnkAddMultiChoice";
            this.lnkAddMultiChoice.Size = new System.Drawing.Size(73, 17);
            this.lnkAddMultiChoice.TabIndex = 7;
            this.lnkAddMultiChoice.TabStop = true;
            this.lnkAddMultiChoice.Text = "新增複選題";
            this.lnkAddMultiChoice.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddMultiChoice_LinkClicked);
            // 
            // btnPreview
            // 
            this.btnPreview.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPreview.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnPreview.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPreview.Location = new System.Drawing.Point(664, 7);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.ShowSubItems = false;
            this.btnPreview.Size = new System.Drawing.Size(63, 23);
            this.btnPreview.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPreview.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer2});
            this.btnPreview.TabIndex = 6;
            this.btnPreview.Text = "預覽";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // itemContainer2
            // 
            // 
            // 
            // 
            this.itemContainer2.BackgroundStyle.Class = "";
            this.itemContainer2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer2.Name = "itemContainer2";
            // 
            // lnkAddEssay
            // 
            this.lnkAddEssay.AutoSize = true;
            this.lnkAddEssay.Location = new System.Drawing.Point(94, 10);
            this.lnkAddEssay.Name = "lnkAddEssay";
            this.lnkAddEssay.Size = new System.Drawing.Size(73, 17);
            this.lnkAddEssay.TabIndex = 5;
            this.lnkAddEssay.TabStop = true;
            this.lnkAddEssay.Text = "新增問答題";
            this.lnkAddEssay.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddEssay_LinkClicked);
            // 
            // lnkAddSingleChoice
            // 
            this.lnkAddSingleChoice.AutoSize = true;
            this.lnkAddSingleChoice.Location = new System.Drawing.Point(12, 10);
            this.lnkAddSingleChoice.Name = "lnkAddSingleChoice";
            this.lnkAddSingleChoice.Size = new System.Drawing.Size(73, 17);
            this.lnkAddSingleChoice.TabIndex = 4;
            this.lnkAddSingleChoice.TabStop = true;
            this.lnkAddSingleChoice.Text = "新增單選題";
            this.lnkAddSingleChoice.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddQuestion_LinkClicked);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelX1.Location = new System.Drawing.Point(6, 5);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(0, 0);
            this.labelX1.TabIndex = 3;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonX1.AutoExpandOnClick = true;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(743, 6);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(63, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer1});
            this.buttonX1.TabIndex = 2;
            this.buttonX1.Text = "項目";
            this.buttonX1.MouseEnter += new System.EventHandler(this.buttonX1_MouseEnter);
            // 
            // itemContainer1
            // 
            // 
            // 
            // 
            this.itemContainer1.BackgroundStyle.Class = "";
            this.itemContainer1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemContainer1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemContainer1.Name = "itemContainer1";
            this.itemContainer1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.controlContainerItem1});
            // 
            // controlContainerItem1
            // 
            this.controlContainerItem1.AllowItemResize = true;
            this.controlContainerItem1.AutoCollapseOnClick = false;
            this.controlContainerItem1.GlobalItem = false;
            this.controlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem1.Name = "controlContainerItem1";
            this.controlContainerItem1.ShowSubItems = false;
            this.controlContainerItem1.Text = "controlContainerItem1";
            // 
            // panel1
            // 
            this.panel1.CanvasColor = System.Drawing.Color.Transparent;
            this.panel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panel1.Controls.Add(this.panProgress);
            this.panel1.Controls.Add(this.DetailItemContainers);
            this.panel1.Controls.Add(this.panelEx3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(815, 484);
            this.panel1.TabIndex = 3;
            // 
            // panProgress
            // 
            this.panProgress.CanvasColor = System.Drawing.Color.Transparent;
            this.panProgress.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panProgress.Controls.Add(this.circleProjress);
            this.panProgress.Dock = System.Windows.Forms.DockStyle.Top;
            this.panProgress.Location = new System.Drawing.Point(0, 0);
            this.panProgress.Name = "panProgress";
            this.panProgress.Size = new System.Drawing.Size(815, 400);
            this.panProgress.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panProgress.Style.GradientAngle = 90;
            this.panProgress.TabIndex = 7;
            // 
            // circleProjress
            // 
            this.circleProjress.BackColor = System.Drawing.Color.Transparent;
            this.circleProjress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.circleProjress.Image = global::TeachingEvaluation.Properties.Resources.loading_gif;
            this.circleProjress.InitialImage = null;
            this.circleProjress.Location = new System.Drawing.Point(0, 0);
            this.circleProjress.Name = "circleProjress";
            this.circleProjress.Size = new System.Drawing.Size(815, 400);
            this.circleProjress.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.circleProjress.TabIndex = 0;
            this.circleProjress.TabStop = false;
            // 
            // DetailItemContainers
            // 
            this.DetailItemContainers.AutoScroll = true;
            this.DetailItemContainers.CanvasColor = System.Drawing.SystemColors.Control;
            this.DetailItemContainers.CardWidth = 554;
            this.DetailItemContainers.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DetailItemContainers.Location = new System.Drawing.Point(139, 233);
            this.DetailItemContainers.Margin = new System.Windows.Forms.Padding(4);
            this.DetailItemContainers.MinWidth = 2;
            this.DetailItemContainers.Name = "DetailItemContainers";
            this.DetailItemContainers.Padding = new System.Windows.Forms.Padding(7);
            this.DetailItemContainers.Size = new System.Drawing.Size(305, 186);
            this.DetailItemContainers.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.DetailItemContainers.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.DetailItemContainers.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.DetailItemContainers.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.DetailItemContainers.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.DetailItemContainers.Style.GradientAngle = 90;
            this.DetailItemContainers.TabIndex = 1;
            this.DetailItemContainers.MouseEnter += new System.EventHandler(this.cardPanelEx1_MouseEnter);
            // 
            // panelEx3
            // 
            this.panelEx3.AutoSize = true;
            this.panelEx3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx3.Controls.Add(this.ContentLinks);
            this.panelEx3.Location = new System.Drawing.Point(180, 57);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(0, 0);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 5;
            // 
            // ContentLinks
            // 
            this.ContentLinks.AutoSize = true;
            this.ContentLinks.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ContentLinks.ColumnCount = 2;
            this.ContentLinks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ContentLinks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ContentLinks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentLinks.Location = new System.Drawing.Point(0, 0);
            this.ContentLinks.Margin = new System.Windows.Forms.Padding(0);
            this.ContentLinks.Name = "ContentLinks";
            this.ContentLinks.RowCount = 1;
            this.ContentLinks.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ContentLinks.Size = new System.Drawing.Size(0, 0);
            this.ContentLinks.TabIndex = 0;
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Blue;
            // 
            // DetailPane
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "DetailPane";
            this.Size = new System.Drawing.Size(815, 520);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panProgress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.circleProjress)).EndInit();
            this.panelEx3.ResumeLayout(false);
            this.panelEx3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.PanelEx panel1;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.ControlContainerItem controlContainerItem1;
        private System.Windows.Forms.TableLayoutPanel ContentLinks;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.LinkLabel lnkAddEssay;
        private System.Windows.Forms.LinkLabel lnkAddSingleChoice;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ItemContainer itemContainer2;
        protected internal DevComponents.DotNetBar.ButtonX btnPreview;
        private System.Windows.Forms.LinkLabel lnkAddMultiChoice;
        public CardPanelEx DetailItemContainers;
        public System.Windows.Forms.PictureBox circleProjress;
        public DevComponents.DotNetBar.PanelEx panProgress;
    }
}
