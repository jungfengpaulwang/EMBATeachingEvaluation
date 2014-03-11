namespace TeachingEvaluation.PrivateControl
{
    partial class PupopDetailPane
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.expandablePanel1 = new DevComponents.DotNetBar.ExpandablePanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstSurvey = new System.Windows.Forms.ListBox();
            this.Add = new DevComponents.DotNetBar.ButtonX();
            this.Update = new DevComponents.DotNetBar.ButtonX();
            this.Delete = new DevComponents.DotNetBar.ButtonX();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.detailPane1 = new TeachingEvaluation.PrivateControl.DetailPane();
            this.expandablePanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // expandablePanel1
            // 
            this.expandablePanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.expandablePanel1.CollapseDirection = DevComponents.DotNetBar.eCollapseDirection.RightToLeft;
            this.expandablePanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.expandablePanel1.Controls.Add(this.panel1);
            this.expandablePanel1.Controls.Add(this.Add);
            this.expandablePanel1.Controls.Add(this.Update);
            this.expandablePanel1.Controls.Add(this.Delete);
            this.expandablePanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.expandablePanel1.ExpandOnTitleClick = true;
            this.expandablePanel1.Location = new System.Drawing.Point(0, 0);
            this.expandablePanel1.Name = "expandablePanel1";
            this.expandablePanel1.ShowFocusRectangle = true;
            this.expandablePanel1.Size = new System.Drawing.Size(222, 729);
            this.expandablePanel1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.expandablePanel1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.expandablePanel1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandablePanel1.Style.GradientAngle = 90;
            this.expandablePanel1.TabIndex = 1;
            this.expandablePanel1.TabStop = true;
            this.expandablePanel1.TitleStyle.Alignment = System.Drawing.StringAlignment.Center;
            this.expandablePanel1.TitleStyle.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandablePanel1.TitleStyle.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.expandablePanel1.TitleStyle.Border = DevComponents.DotNetBar.eBorderType.RaisedInner;
            this.expandablePanel1.TitleStyle.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandablePanel1.TitleStyle.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.expandablePanel1.TitleStyle.GradientAngle = 90;
            this.expandablePanel1.TitleText = "問卷樣版";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstSurvey);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(222, 634);
            this.panel1.TabIndex = 7;
            // 
            // lstSurvey
            // 
            this.lstSurvey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSurvey.FormattingEnabled = true;
            this.lstSurvey.ItemHeight = 17;
            this.lstSurvey.Location = new System.Drawing.Point(0, 0);
            this.lstSurvey.Name = "lstSurvey";
            this.lstSurvey.Size = new System.Drawing.Size(222, 634);
            this.lstSurvey.TabIndex = 6;
            this.lstSurvey.SelectedIndexChanged += new System.EventHandler(this.lstSurvey_SelectedIndexChanged);
            // 
            // Add
            // 
            this.Add.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Add.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Add.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Add.Location = new System.Drawing.Point(0, 660);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(222, 23);
            this.Add.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Add.TabIndex = 4;
            this.Add.Text = "新增";
            this.Add.Click += new System.EventHandler(this.chkAdd_Click);
            // 
            // Update
            // 
            this.Update.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Update.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Update.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Update.Location = new System.Drawing.Point(0, 683);
            this.Update.Name = "Update";
            this.Update.Size = new System.Drawing.Size(222, 23);
            this.Update.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Update.TabIndex = 3;
            this.Update.Text = "修改";
            this.Update.Click += new System.EventHandler(this.Update_Click);
            // 
            // Delete
            // 
            this.Delete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Delete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Delete.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Delete.Location = new System.Drawing.Point(0, 706);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(222, 23);
            this.Delete.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.Delete.TabIndex = 2;
            this.Delete.Text = "刪除";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(222, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 729);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // detailPane1
            // 
            this.detailPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailPane1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.detailPane1.LayoutXml = null;
            this.detailPane1.Location = new System.Drawing.Point(227, 0);
            this.detailPane1.Name = "detailPane1";
            this.detailPane1.PrimaryKey = "";
            this.detailPane1.Size = new System.Drawing.Size(781, 729);
            this.detailPane1.TabIndex = 3;
            // 
            // PupopDetailPane
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.detailPane1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.expandablePanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "PupopDetailPane";
            this.Text = "問卷樣版管理";
            this.Resize += new System.EventHandler(this.PupopDetailPane_Resize);
            this.expandablePanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ExpandablePanel expandablePanel1;
        private DevComponents.DotNetBar.ButtonX Add;
        private DevComponents.DotNetBar.ButtonX Update;
        private DevComponents.DotNetBar.ButtonX Delete;
        private System.Windows.Forms.Splitter splitter1;
        private DetailPane detailPane1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lstSurvey;



    }
}