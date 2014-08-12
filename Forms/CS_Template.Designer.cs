namespace TeachingEvaluation.Forms
{
    partial class Email_Content_Template
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
            if (disposing && (components != null))
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
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.colorPickerDropDown1 = new DevComponents.DotNetBar.ColorPickerDropDown();
			this.btnParameter = new DevComponents.DotNetBar.ButtonItem();
			this.itemContainer1 = new DevComponents.DotNetBar.ItemContainer();
			this.btnStudentName = new DevComponents.DotNetBar.ButtonItem();
			this.btnSubjectName = new DevComponents.DotNetBar.ButtonItem();
			this.btnCourseName = new DevComponents.DotNetBar.ButtonItem();
			this.btnTeacherName = new DevComponents.DotNetBar.ButtonItem();
			this.btnSchoolYear = new DevComponents.DotNetBar.ButtonItem();
			this.btnSemester = new DevComponents.DotNetBar.ButtonItem();
			this.btnBeginTime = new DevComponents.DotNetBar.ButtonItem();
			this.btnEndTime = new DevComponents.DotNetBar.ButtonItem();
			this.panelButtom = new DevComponents.DotNetBar.PanelEx();
			this.btnExit = new DevComponents.DotNetBar.ButtonX();
			this.btnSave = new DevComponents.DotNetBar.ButtonX();
			this.panelContent = new DevComponents.DotNetBar.PanelEx();
			this.labelX2 = new DevComponents.DotNetBar.LabelX();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.txtSubject = new DevComponents.DotNetBar.Controls.TextBoxX();
			this.labelX1 = new DevComponents.DotNetBar.LabelX();
			this.panel1 = new DevComponents.DotNetBar.PanelEx();
			this.bar1 = new DevComponents.DotNetBar.Bar();
			this.panelButtom.SuspendLayout();
			this.panelContent.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
			this.SuspendLayout();
			// 
			// colorPickerDropDown1
			// 
			this.colorPickerDropDown1.AutoExpandOnClick = true;
			this.colorPickerDropDown1.Name = "colorPickerDropDown1";
			this.colorPickerDropDown1.Text = "A";
			this.colorPickerDropDown1.SelectedColorChanged += new System.EventHandler(this.colorPickerDropDown1_SelectedColorChanged);
			// 
			// btnParameter
			// 
			this.btnParameter.AutoExpandOnClick = true;
			this.btnParameter.Name = "btnParameter";
			this.btnParameter.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.itemContainer1});
			this.btnParameter.Text = "變數";
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
            this.btnStudentName,
            this.btnSubjectName,
            this.btnCourseName,
            this.btnTeacherName,
            this.btnSchoolYear,
            this.btnSemester,
            this.btnBeginTime,
            this.btnEndTime});
			// 
			// btnStudentName
			// 
			this.btnStudentName.Name = "btnStudentName";
			this.btnStudentName.Text = "學生姓名";
			// 
			// btnSubjectName
			// 
			this.btnSubjectName.Name = "btnSubjectName";
			this.btnSubjectName.Text = "課程";
			// 
			// btnCourseName
			// 
			this.btnCourseName.Name = "btnCourseName";
			this.btnCourseName.Text = "開課";
			// 
			// btnTeacherName
			// 
			this.btnTeacherName.Name = "btnTeacherName";
			this.btnTeacherName.Text = "授課教師";
			// 
			// btnSchoolYear
			// 
			this.btnSchoolYear.Name = "btnSchoolYear";
			this.btnSchoolYear.Text = "學年度";
			// 
			// btnSemester
			// 
			this.btnSemester.Name = "btnSemester";
			this.btnSemester.Text = "學期";
			// 
			// btnBeginTime
			// 
			this.btnBeginTime.Name = "btnBeginTime";
			this.btnBeginTime.Text = "問卷填寫開始時間";
			// 
			// btnEndTime
			// 
			this.btnEndTime.Name = "btnEndTime";
			this.btnEndTime.Text = "問卷填寫結束時間";
			// 
			// panelButtom
			// 
			this.panelButtom.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelButtom.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.panelButtom.Controls.Add(this.btnExit);
			this.panelButtom.Controls.Add(this.btnSave);
			this.panelButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelButtom.Location = new System.Drawing.Point(0, 519);
			this.panelButtom.Name = "panelButtom";
			this.panelButtom.Size = new System.Drawing.Size(784, 42);
			this.panelButtom.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelButtom.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelButtom.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.panelButtom.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.panelButtom.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.panelButtom.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelButtom.Style.GradientAngle = 90;
			this.panelButtom.TabIndex = 9;
			// 
			// btnExit
			// 
			this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.btnExit.Location = new System.Drawing.Point(688, 9);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 25);
			this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnExit.TabIndex = 3;
			this.btnExit.Text = "離開";
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnSave
			// 
			this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			this.btnSave.Location = new System.Drawing.Point(596, 9);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 25);
			this.btnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "儲存";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// panelContent
			// 
			this.panelContent.CanvasColor = System.Drawing.SystemColors.Control;
			this.panelContent.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.panelContent.Controls.Add(this.labelX2);
			this.panelContent.Controls.Add(this.webBrowser1);
			this.panelContent.Controls.Add(this.txtSubject);
			this.panelContent.Controls.Add(this.labelX1);
			this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelContent.Location = new System.Drawing.Point(0, 38);
			this.panelContent.Name = "panelContent";
			this.panelContent.Size = new System.Drawing.Size(784, 481);
			this.panelContent.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.panelContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.panelContent.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.panelContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelContent.Style.GradientAngle = 90;
			this.panelContent.TabIndex = 10;
			// 
			// labelX2
			// 
			this.labelX2.AutoSize = true;
			// 
			// 
			// 
			this.labelX2.BackgroundStyle.Class = "";
			this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX2.Location = new System.Drawing.Point(17, 48);
			this.labelX2.Name = "labelX2";
			this.labelX2.Size = new System.Drawing.Size(34, 21);
			this.labelX2.TabIndex = 6;
			this.labelX2.Text = "內容";
			// 
			// webBrowser1
			// 
			this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.webBrowser1.Location = new System.Drawing.Point(59, 46);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(704, 429);
			this.webBrowser1.TabIndex = 5;
			// 
			// txtSubject
			// 
			this.txtSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			// 
			// 
			// 
			this.txtSubject.Border.Class = "TextBoxBorder";
			this.txtSubject.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.txtSubject.Location = new System.Drawing.Point(59, 15);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.Size = new System.Drawing.Size(704, 25);
			this.txtSubject.TabIndex = 3;
			this.txtSubject.Enter += new System.EventHandler(this.txtSubject_Enter);
			// 
			// labelX1
			// 
			this.labelX1.AutoSize = true;
			// 
			// 
			// 
			this.labelX1.BackgroundStyle.Class = "";
			this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.labelX1.Location = new System.Drawing.Point(17, 17);
			this.labelX1.Name = "labelX1";
			this.labelX1.Size = new System.Drawing.Size(34, 21);
			this.labelX1.TabIndex = 2;
			this.labelX1.Text = "主旨";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.bar1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(784, 38);
			this.panel1.TabIndex = 4;
			// 
			// bar1
			// 
			this.bar1.AntiAlias = true;
			this.bar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bar1.DockSide = DevComponents.DotNetBar.eDockSide.Top;
			this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.colorPickerDropDown1,
            this.btnParameter});
			this.bar1.Location = new System.Drawing.Point(0, 0);
			this.bar1.Name = "bar1";
			this.bar1.Size = new System.Drawing.Size(784, 26);
			this.bar1.Stretch = true;
			this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.bar1.TabIndex = 0;
			this.bar1.TabStop = false;
			this.bar1.Text = "bar1";
			// 
			// Email_Content_Template
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.panelContent);
			this.Controls.Add(this.panelButtom);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.MaximizeBox = true;
			this.Name = "Email_Content_Template";
			this.Text = "編輯email內容文字樣版";
			this.Load += new System.EventHandler(this.Email_Content_Template_Load);
			this.panelButtom.ResumeLayout(false);
			this.panelContent.ResumeLayout(false);
			this.panelContent.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private DevComponents.DotNetBar.ColorPickerDropDown colorPickerDropDown1;
        private DevComponents.DotNetBar.ButtonItem btnParameter;
        private DevComponents.DotNetBar.ItemContainer itemContainer1;
        private DevComponents.DotNetBar.ButtonItem btnStudentName;
        private DevComponents.DotNetBar.ButtonItem btnSubjectName;
        private DevComponents.DotNetBar.ButtonItem btnCourseName;
        private DevComponents.DotNetBar.ButtonItem btnTeacherName;
        private DevComponents.DotNetBar.ButtonItem btnSchoolYear;
        private DevComponents.DotNetBar.ButtonItem btnSemester;
        private DevComponents.DotNetBar.ButtonItem btnBeginTime;
        private DevComponents.DotNetBar.ButtonItem btnEndTime;
        private DevComponents.DotNetBar.PanelEx panelButtom;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnSave;
        private DevComponents.DotNetBar.PanelEx panelContent;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSubject;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.PanelEx panel1;
        private DevComponents.DotNetBar.Bar bar1;
    }
}