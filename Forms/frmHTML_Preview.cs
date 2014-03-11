using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using mshtml;
using System.Threading.Tasks;
using System.IO;

namespace TeachingEvaluation.Forms
{
    public partial class frmHTML_Preview : BaseForm
    {
        private UDT.Survey _Survey;
        private AccessHelper Access;
        public frmHTML_Preview(UDT.Survey Survey)
        {
            InitializeComponent();

            this._Survey = Survey;
            this.Access = new AccessHelper();
            this.Load += new System.EventHandler(this.Form_Load);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            List<UDT.Question> Questions = new List<UDT.Question>();
            List<UDT.QuestionOption> QuestionOptions = new List<UDT.QuestionOption>();
            Dictionary<UDT.Hierarchy, List<UDT.Question>> dicHierarchies = new Dictionary<UDT.Hierarchy, List<UDT.Question>>();
            Dictionary<int, List<UDT.QuestionOption>> dicQuestionOptions = new Dictionary<int, List<UDT.QuestionOption>>();
            Dictionary<string, UDT.Question> dicQuestions = new Dictionary<string, UDT.Question>();
            Dictionary<UDT.Hierarchy, List<UDT.Question>> dicHierarchies_New = new Dictionary<UDT.Hierarchy, List<UDT.Question>>();
            this.HTML_Show_Wait();

            Task task = Task.Factory.StartNew(() =>
            {
                List<int> exits_question_ids = new List<int>();
                Questions = Access.Select<UDT.Question>(string.Format("ref_survey_id ={0}", this._Survey.UID));
                if (Questions.Count > 0)
                {
                    QuestionOptions = Access.Select<UDT.QuestionOption>(string.Format("ref_question_id in ({0})", string.Join(",", Questions.Select(x => x.UID))));
                    Questions = Questions.OrderBy(x => x.DisplayOrder).ToList();
                    dicQuestions = Questions.ToDictionary(x=>x.UID);
                }
                if (QuestionOptions.Count > 0)
                    QuestionOptions = QuestionOptions.OrderBy(x => x.QuestionID).ThenBy(x => x.DisplayOrder).ToList(); 

                List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
                List<UDT.Hierarchy> Hierarchies = Access.Select<UDT.Hierarchy>();
                UDT.Hierarchy uHierarchy = new UDT.Hierarchy();
                dicHierarchies.Add(uHierarchy, new List<UDT.Question>());
                if (Hierarchies.Count > 0)
                {
                    Hierarchies = Hierarchies.OrderBy(x=>x.DisplayOrder).ToList();
                    Hierarchies.ForEach((x) =>
                    {
                        if (!dicHierarchies.ContainsKey(x))
                            dicHierarchies.Add(x, new List<UDT.Question>());
                    });
                }
                if (QHRelations.Count > 0)
                {
                    QHRelations.ForEach((x) =>
                    {
                        foreach (UDT.Hierarchy Hierarchy in dicHierarchies.Keys)
                        {
                            if (x.HierarchyTitle == Hierarchy.Title && dicQuestions.ContainsKey(x.QuestionID.ToString()))
                            {
                                dicHierarchies[Hierarchy].Add(dicQuestions[x.QuestionID.ToString()]);
                                exits_question_ids.Add(x.QuestionID);
                            }
                        }
                    });
                }
                foreach (UDT.QuestionOption QuestionOption in QuestionOptions)
                {
                    if (!dicQuestionOptions.ContainsKey(QuestionOption.QuestionID))
                        dicQuestionOptions.Add(QuestionOption.QuestionID, new List<UDT.QuestionOption>());

                    dicQuestionOptions[QuestionOption.QuestionID].Add(QuestionOption);
                }
                foreach (UDT.Question Q in Questions)
                {
                    if (!exits_question_ids.Contains(int.Parse(Q.UID)))
                    {
                        exits_question_ids.Add(int.Parse(Q.UID));
                        dicHierarchies[uHierarchy].Add(Q);
                    }
                }
                foreach (UDT.Hierarchy Hierarchy in dicHierarchies.Keys)
                {
                    dicHierarchies_New.Add(Hierarchy, dicHierarchies[Hierarchy].OrderBy(y => y.DisplayOrder).ToList());
                }
            });
            task.ContinueWith((x) =>
            {
                if (Questions.Count == 0)
                {
                    MessageBox.Show("沒有題目。");
                    this.Close();
                    return;
                }
                this.HTML_Preview(Questions, dicQuestionOptions, dicHierarchies_New);

            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private string ReadImageInBase64()
        {
            MemoryStream ms = new MemoryStream(); 
            Properties.Resources.loading_gif.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            byte[] byteArray = ms.ToArray();

            return Convert.ToBase64String(byteArray);
        }

        private void HTML_Show_Wait()
        {
            webBrowser1.DocumentText = "<!doctype html><html><body><img src=\"data:image/gif;base64," + ReadImageInBase64() + "\"  style='display: block; margin-left: auto; margin-right: auto; margin-top:180px;'></body></html>";
            //webBrowser1.DocumentText = "資料讀取中，請稍候………".ToString();      
        }

        private void HTML_Preview(List<UDT.Question> Questions, Dictionary<int, List<UDT.QuestionOption>> dicQuestionOptions, Dictionary<UDT.Hierarchy, List<UDT.Question>> dicHierarchies)
        {
            StringBuilder SB_Question = new StringBuilder(@"<!doctype html><html><body style='margin:0px;'><table border='1' cellspacing='0' cellpadding='5'><thead><tr style='background-color:#DDDDE1'><th>題目標題</th><th>題號</th><th>題目</th><th>選項</th><th>必填</th><th>自評</th><th>個案</th><th>不列入評鑑值計算</th></tr></thead><tbody>");

            int i = 0;
            foreach (UDT.Hierarchy Hierarchy in dicHierarchies.Keys)
            {
                foreach (UDT.Question Question in dicHierarchies[Hierarchy])
                {
                    i++;

                    StringBuilder SB_QuestionOption = new StringBuilder();
                    if (dicQuestionOptions.ContainsKey(int.Parse(Question.UID)))
                    {
                        List<UDT.QuestionOption> QuestionOptions = dicQuestionOptions[int.Parse(Question.UID)];
                        foreach (UDT.QuestionOption QuestionOption in QuestionOptions)
                        {
                            SB_QuestionOption.AppendFormat(@"<label>({0}){1}</label>", QuestionOption.DisplayOrder, QuestionOption.Title);
                        }
                    }
                    else
                        SB_QuestionOption.Append("&nbsp;");

                    SB_Question.AppendFormat(@"<tr style='background-color:{0}'><td>{7}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{8}</td></tr>", (i % 2) == 0 ? "#f0f9f9" : "#ffddff", Question.DisplayOrder, Question.Title, SB_QuestionOption.ToString(), Question.IsRequired ? "✔" : "&nbsp;", Question.IsSelfAssessment ? "✔" : "&nbsp;", Question.IsCase ? "✔" : "&nbsp;", string.IsNullOrWhiteSpace(Hierarchy.Title) ? "&nbsp;" : GetMingGhoNumber(Hierarchy.DisplayOrder) + "、" + Hierarchy.Title, Question.IsNoneCalculated ? "✔" : "&nbsp;");
                }
            }
            SB_Question.Append("</tbody></body></html>");
            webBrowser1.DocumentText = SB_Question.ToString();            
        }

        private string GetMingGhoNumber(int display_order)
        {
            string[] MingGhoNumber = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

            return MingGhoNumber.ElementAt(display_order);
        }
    }
}