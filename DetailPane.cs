using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using FISCA.Presentation;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace TeachingEvaluation.PrivateControl
{
    partial class DetailPane : UserControl
    {
        static DetailPane()
        {
            //DotNetBarReferenceFixer.FixIt();
        }
        private Dictionary<string, DetailItemContainer> _Containers = new Dictionary<string, DetailItemContainer>();
        private Dictionary<string, CheckBoxX> _CheckBoxs = new Dictionary<string, CheckBoxX>();
        private Dictionary<string, LinkLabel> _Labels = new Dictionary<string, LinkLabel>();
        private List<string> DetailItemOriginOrder = new List<string>();
        private DescriptionPane _DescriptionPane = null;
        private string _PrimaryKey = "";
        public DetailPane()
        {
            InitializeComponent();
            DetailItemContainers.Dock = DockStyle.Fill;
            controlContainerItem1.Control = panelEx3;
            //ContentLinks.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            Event.DeliverDetailContent.CopyDetailContent += new EventHandler<Event.DeliverDetailContentEventArgs>(CopyDetailContent);
            Event.DeliverDetailContent.DeleteDetailContent += new EventHandler<Event.DeliverDetailContentEventArgs>(DeleteDetailContent);
            Event.DeliverDetailContent.PassDetailContent += new EventHandler<Event.DeliverDetailContentEventArgs>(PassDetailContent);
        }

        private void PassDetailContent(object sender, Event.DeliverDetailContentEventArgs e)
        {
            this.SetCheckBoxText(e.DetailContent.Group, e.DetailContent.Text);
        }

        private void CopyDetailContent(object sender, Event.DeliverDetailContentEventArgs e)
        {
            this.AddDetailItem(e.DetailContent);
        }

        private void DeleteDetailContent(object sender, Event.DeliverDetailContentEventArgs e)
        {
            this.DeleteDetailItem(e.DetailContent);
        }       

        //  未使用
        public void SetDescriptionPane(DescriptionPane pane)
        {
            _DescriptionPane = pane;
            pane.Dock = DockStyle.Top;
            labelX1.Visible = false;
            pane.BackColor = Color.Transparent;
            panelEx1.Controls.Add(pane);
            pane.Location = new Point(0, 0);
            if (panelEx1.Height > pane.Height)
            {
                pane.Top = (panelEx1.Height - pane.Height) / 2;
            }
            else
            {
                panelEx1.Height = pane.Height;
            }
            buttonX1.BringToFront();
            buttonX1.Top = (panelEx1.Height - buttonX1.Height > buttonX1.Height ? 6 : (panelEx1.Height - buttonX1.Height) / 2);
        }

        /// <summary>
        /// 用於決定 PopupForm 的 DetailItem 順序。
        /// </summary>
        public XElement LayoutXml { get; set; }

        public void SetLayout(XElement layout)
        {
            if (layout == null) return;

            ItemComparer comparer = new ItemComparer(layout, DetailItemOriginOrder);
            LayoutXml = layout;

            List<string> groups = new List<string>(_Containers.Keys);
            //groups.Sort((x, y) =>
            //{
            //    return comparer.Compare(x, y);
            //});

            List<System.Windows.Forms.Control> controls = new List<System.Windows.Forms.Control>(_Containers.Values);
            Dictionary<string, FlowLayoutPanel> links = new Dictionary<string, FlowLayoutPanel>();
            foreach (FlowLayoutPanel link in ContentLinks.Controls)
            {
                string name = (link.Controls[1] as LinkLabel).Text; //寫死了 @@''
                links.Add(name, link);
            }

            DetailItemContainers.Controls.Clear();
            ContentLinks.Controls.Clear();

            int count = 0;
            foreach (string group in groups)
            {
                DetailItemContainers.Controls.Add(_Containers[group]);
                AddContentLink(links[group], count++);
            }
        }

        public void DeleteDetailItem(PrivateControl.DetailContent content)
        {
            if (content == null) return;

            if (_Containers.ContainsKey(content.Group))
            {
                //_Containers[content.Group].RemoveContent(content);
                DetailItemContainers.Controls.Remove(_Containers[content.Group]);
                _Containers.Remove(content.Group);
            }
            if (_CheckBoxs.ContainsKey(content.Group))
                _CheckBoxs.Remove(content.Group);
            if (_Labels.ContainsKey(content.Group))
                _Labels.Remove(content.Group);
            if (DetailItemOriginOrder.Contains(content.Group))
                DetailItemOriginOrder.Remove(content.Group);

            Dictionary<string, FlowLayoutPanel> links = new Dictionary<string, FlowLayoutPanel>();
            foreach (FlowLayoutPanel link in ContentLinks.Controls)
            {
                string name = (link.Controls[1] as LinkLabel).Text;
                if (name == content.Text)
                    continue;

                links.Add(name, link);
            }
            //You need to set AutoScroll = false, remove rows, then set AutoScroll = true.  This will force the panel to recalculate the correct size.
            //ContentLinks.AutoScroll = true;
            ContentLinks.Controls.Clear();
            ContentLinks.RowCount = 0;
            int count = 0;
            foreach (FlowLayoutPanel link in links.Values)
            {
                AddContentLink(link, count++);
            }
            //ContentLinks.AutoScroll = false;
        }

        public void Clear()
        {
            //this.SuspendLayout();
            //foreach (string Group in this._Containers.Keys.ToList())
            //{
            //    DetailItemContainers.Controls.Remove(_Containers[Group]);
            //    _Containers.Remove(Group);
            //}
            _Containers.Clear();
            this.DetailItemContainers.Controls.Clear();           
            this._CheckBoxs.Clear();
            this._Labels.Clear();
            this.DetailItemOriginOrder.Clear();
            this.PrimaryKey = string.Empty;
            this.SetDescription(string.Empty);
            this.ContentLinks.Controls.Clear();
            this.ContentLinks.RowCount = 0;
            //this.ResumeLayout(false);
        }

        public void AddDetailItem(PrivateControl.DetailContent content)
        {
            if (content == null) return;
            //this.SuspendLayout();
            DetailItemContainer container;
            if (!_Containers.ContainsKey(content.Group))
            {
                DetailItemOriginOrder.Add(content.Group);

                container = new DetailItemContainer();
                CheckBoxX checkbox = new CheckBoxX();
                LinkLabel label = new LinkLabel();
                FlowLayoutPanel panel = new FlowLayoutPanel();
                checkbox.Tag = content.Group;
                checkbox.Checked = true;
                label.Tag = content.Group;
                label.Text = content.Caption;
                label.Click += new EventHandler(label_Click);
                checkbox.CheckedChanged += new EventHandler(checkbox_CheckedChanged);
                //container.Visible = false;
                //panel.SuspendLayout();
                //DetailItemContainers.SuspendLayout();

                panel.Controls.Add(checkbox);
                panel.Controls.Add(label);
                panel.AutoSize = true;
                panel.Dock = System.Windows.Forms.DockStyle.Fill;
                panel.Margin = new System.Windows.Forms.Padding(2, 0, 10, 0);
                panel.Padding = new System.Windows.Forms.Padding(2, 2, 2, 0);

                checkbox.Anchor = System.Windows.Forms.AnchorStyles.None;
                checkbox.Size = new System.Drawing.Size(20, 15);
                checkbox.FocusCuesEnabled = false;
                checkbox.TextVisible = false;
                checkbox.Margin = new System.Windows.Forms.Padding(0);

                label.Anchor = System.Windows.Forms.AnchorStyles.None;
                label.AutoSize = true;
                label.Margin = new System.Windows.Forms.Padding(0);

                AddContentLink(panel, _Containers.Count);

                DetailItemContainers.Controls.Add(container);

                _Containers.Add(content.Group, container);
                _CheckBoxs.Add(content.Group, checkbox);
                _Labels.Add(content.Group, label);

                //panel.ResumeLayout(false);
                //DetailItemContainers.ResumeLayout(false);
                //checkbox.Checked = (PreferenceProvider["ContentsVisible"].GetAttribute(fixName(content.Group)) == "True");
                //checkbox.Checked = Preference.GetBoolean(content.Group, false);
            }
            else
                container = _Containers[content.Group];
            
            List<PrivateControl.DetailContent> contents = new List<DetailContent>();
            _Containers.Values.ToList().ForEach((x) =>
            {
                x.GetContents().ForEach(y => contents.Add(y));
            });
            if (content.DisplayOrder == null)
            {
                decimal Max_Display_Order = 0;
                contents.ForEach((x) =>
                {
                    if ((x.DisplayOrder.HasValue ? x.DisplayOrder.Value : 0) > Max_Display_Order)
                        Max_Display_Order = (x.DisplayOrder.HasValue ? x.DisplayOrder.Value : 0);
                });
                content.DisplayOrder = Max_Display_Order + 1;
            }
            //container.SuspendLayout();
            container.AddContent(content);
            //container.ResumeLayout(false);
            //this.ResumeLayout(false);
        }

        private void SetCheckBoxText(string key, string text)
        {
            if (this._Labels.ContainsKey(key))
                this._Labels[key].Text = text;
        }

        private void AddContentLink(FlowLayoutPanel link, int currentItemCount)
        {
            if (currentItemCount / 2 > ContentLinks.RowCount)
            {
                ContentLinks.RowCount++;
                this.ContentLinks.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            }

            ContentLinks.Controls.Add(link, currentItemCount % 2, currentItemCount / 2);
        }

        public string PrimaryKey
        {
            get { return _PrimaryKey; }
            set
            {
                //if (value != _PrimaryKey)
                //{
                    _PrimaryKey = value;
                    foreach (var item in _Containers.Values)
                    {
                        item.SetPrimaryKey(_PrimaryKey);
                    }
                    //if (_DescriptionPane != null)
                    //    _DescriptionPane.SetPrimaryKey(_PrimaryKey);
                //}
            }
        }
        public void ReloadAll()
        {
            foreach (var item in _Containers.Values)
            {
                item.Reload();
            }
            //if (_DescriptionPane != null)
            //    _DescriptionPane.SetPrimaryKey(_PrimaryKey);
        }
        public void SetDescription(string description)
        {
            this.labelX1.Text = description;
            this.labelX1.Visible = true;
            this.labelX1.AutoSize = true;
            this.labelX1.TextAlignment = StringAlignment.Center;
            this.labelX1.Top = (panelEx1.Height - this.labelX1.Height) / 2;
            this.labelX1.Left = (panelEx1.Width - this.labelX1.Width) / 2;
        }

        //public IPreferenceProvider PreferenceProvider
        //{
        //    get { return _PreferenceProvider; }
        //    set
        //    {
        //        _PreferenceProvider = value;
        //        LoadPreference();
        //    }
        //}

        //private XPreference _preference = null;
        //internal XPreference Preference
        //{
        //    get
        //    {
        //        if (_preference == null)
        //            _preference = PreferenceRequire();

        //        return _preference;
        //    }
        //}

        //internal Func<XPreference> PreferenceRequire;

        private string fixName(string p)
        {
            string fixname = p.Replace("/", "_").Replace("(", "_").Replace(")", "_").Replace("[", "_").Replace("]", "_").Replace("^", "_").Replace("!", "_").Replace("?", "_").Replace(" ", "_").Replace("　", "_");
            if (fixname == "")
                fixname = "_";
            return fixname;
        }

        //private void LoadPreference()
        //{
        //    foreach (var item in _CheckBoxs.Keys)
        //    {
        //        //_CheckBoxs[item].Checked = (PreferenceProvider["ContentsVisible"].GetAttribute(fixName(item)) == "True");
        //        _CheckBoxs[item].Checked = Preference.GetBoolean(item, true);
        //    }
        //}

        //private void UpdatePreference()
        //{
        //    //XmlElement element = PreferenceProvider["ContentsVisible"];
        //    foreach (var item in _CheckBoxs.Keys)
        //    {
        //        Preference.SetBoolean(item, _CheckBoxs[item].Checked);
        //        //element.SetAttribute(fixName(item), _CheckBoxs[item].Checked.ToString());
        //    }
        //    //PreferenceProvider["ContentsVisible"] = element;
        //}

        private void label_Click(object sender, EventArgs e)
        {
            string key = "" + ((System.Windows.Forms.Control)sender).Tag;
            _CheckBoxs[key].Checked = true;
            if (DetailItemContainers.VerticalScroll.Visible && DetailItemContainers.VerticalScroll.Enabled)
            {
                int newScrollValue = DetailItemContainers.VerticalScroll.Value + _Containers[key].Top;
                newScrollValue = newScrollValue > DetailItemContainers.VerticalScroll.Maximum ? DetailItemContainers.VerticalScroll.Maximum : newScrollValue < DetailItemContainers.VerticalScroll.Minimum ? DetailItemContainers.VerticalScroll.Minimum : newScrollValue;
                if (DetailItemContainers.VerticalScroll.Value != newScrollValue)
                {
                    DetailItemContainers.VerticalScroll.Value = newScrollValue;
                    DetailItemContainers.AutoScrollPosition = new Point(DetailItemContainers.AutoScrollPosition.X, newScrollValue);
                    DetailItemContainers.ScrollControlIntoView(_Containers[key]);
                }
            }
        }

        private void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            string key = "" + ((System.Windows.Forms.Control)sender).Tag;
            _Containers[key].Visible = _CheckBoxs[key].Checked;
        }

        private void cardPanelEx1_MouseEnter(object sender, EventArgs e)
        {
            if (DetailItemContainers.TopLevelControl.ContainsFocus && !DetailItemContainers.ContainsFocus)
                DetailItemContainers.Focus();
        }

        private void buttonX1_MouseEnter(object sender, EventArgs e)
        {
            //buttonX1.Expanded = true;
        }

        //private void buttonX1_PopupClose(object sender, EventArgs e)
        //{
        //    UpdatePreference();
        //}

        private void lnkAddQuestion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey))
            {
                MessageBox.Show("請先選擇問卷。");
                return;
            }
            PrivateControl.DetailContent DetailContent = new QuestionTemplate.SingleChoice();
            DetailContent.PrimaryKey = this.PrimaryKey;
            this.AddDetailItem(DetailContent);
        }

        private void lnkAddEssay_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey))
            {
                MessageBox.Show("請先選擇問卷。");
                return;
            }
            PrivateControl.DetailContent DetailContent = new QuestionTemplate.Essay();
            DetailContent.PrimaryKey = this.PrimaryKey;
            this.AddDetailItem(DetailContent);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {

        }

        private void lnkAddMultiChoice_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.PrimaryKey))
            {
                MessageBox.Show("請先選擇問卷。");
                return;
            }
            PrivateControl.DetailContent DetailContent = new QuestionTemplate.MultiChoice();
            DetailContent.PrimaryKey = this.PrimaryKey;
            this.AddDetailItem(DetailContent);
        }

        private void panProgress_Click(object sender, EventArgs e)
        {

        }
    }
}
