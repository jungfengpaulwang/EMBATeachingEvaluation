using Aspose.Cells;
using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TeachingEvaluation.Export
{
    class ExportEvaluation_ComplexQueries_Handler
    {
        private AccessHelper Access;

        private ExportEvaluation_ComplexQueries _User_Form;
        private List<UDT.TeacherStatistics> _Statistics;
        private int _FileType;

        public ExportEvaluation_ComplexQueries_Handler()
        {
            this.Access = new AccessHelper();

            this._User_Form = new ExportEvaluation_ComplexQueries();
            this._Statistics = new List<UDT.TeacherStatistics>();
        }
        private Workbook GetSurveyTemplate(string SurveyID)
        {
            List<UDT.ReportTemplate> ReportTemplates = this.Access.Select<UDT.ReportTemplate>(string.Format("ref_survey_id = {0}", SurveyID));

            byte[] _buffer = Convert.FromBase64String(ReportTemplates.ElementAt(0).Template);
            MemoryStream template = new MemoryStream(_buffer);

            Workbook wb = new Workbook();
            wb.Open(template);

            return wb;
        }

        public void Execute()
        {
            try
            {
                DialogResult dialogResult = this._User_Form.ShowDialog();

                if (dialogResult != DialogResult.OK)
                    return;

                this._Statistics = this._User_Form.SelectedStatistics;
                this._FileType = this._User_Form.FileType;

                Task<Dictionary<string, Workbook>> task = Task<Dictionary<string, Workbook>>.Factory.StartNew(() =>
                {
                    List<UDT.QHRelation> QHRelations = Access.Select<UDT.QHRelation>();
                    List<UDT.Hierarchy> Hierarchies = Access.Select<UDT.Hierarchy>();
                    Dictionary<string, Workbook> dicSurveyIDs = new Dictionary<string, Workbook>();
                    Dictionary<string, List<Workbook>> dicWorkbooks = new Dictionary<string, List<Workbook>>();
                    //Parallel.ForEach<UDT.TeacherStatistics>(this._Statistics, x =>
                    //{
                    this._Statistics.ForEach((x) =>
                    {
                        try
                        {
                            XDocument xDocument = XDocument.Parse(x.StatisticsList, LoadOptions.None);
                            XElement xStatistics = xDocument.Element("Statistics");
                            string SurveyID = xStatistics.Attribute("SurveyID").Value;
                            lock (dicSurveyIDs)
                            {
                                if (!dicSurveyIDs.ContainsKey(SurveyID))
                                    dicSurveyIDs.Add(SurveyID, this.GetSurveyTemplate(SurveyID));
                            }

                            ExcelDocumentMaker excelDocumentMaker = null;// new ExcelDocumentMaker(QHRelations, Hierarchies, x, dicSurveyIDs[SurveyID]);
                            Workbook wb = excelDocumentMaker.Produce();
                            if (wb != null)
                            { 
                                string key = string.Empty;
                                if (this._FileType == 1)
                                    key = excelDocumentMaker.SubjectName + "-課程教學評鑑統計表";
                                if (this._FileType == 2)
                                    key = excelDocumentMaker.CourseName + "-開課教學評鑑統計表";
                                if (this._FileType == 3)
                                    key = excelDocumentMaker.TeacherName + "-授課教師教學評鑑統計表";
                            
                                lock (dicWorkbooks)
                                {
                                    if (!dicWorkbooks.ContainsKey(key))
                                        dicWorkbooks.Add(key, new List<Workbook>());

                                    dicWorkbooks[key].Add(wb);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    });
                    Dictionary<string, Workbook> dicFiles = new Dictionary<string, Workbook>();
                    try
                    {
                        foreach (string key in dicWorkbooks.Keys)
                        {
                            Workbook new_workbook = new Workbook();
                            foreach (Workbook wb in dicWorkbooks[key])
                            {
                                new_workbook.Combine(wb);
                                wb.Worksheets.Cast<Worksheet>().ToList().ForEach(x => wb.Worksheets.RemoveAt(x.Index));
                            }
                            new_workbook.Worksheets.Cast<Worksheet>().ToList().ForEach((x) =>
                            {
                                if (x.Cells.MaxDataColumn == 0 && x.Cells.MaxDataRow == 0)
                                    new_workbook.Worksheets.RemoveAt(x.Index);
                            });
                            dicFiles.Add(key, new_workbook);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    if (dicFiles.Count == 0)
                        throw new Exception("無檔案產生。");

                    return dicFiles;
                });
                task.ContinueWith((x) =>
                {
                    if (x.Exception != null)
                    {
                        MessageBox.Show(x.Exception.InnerException.Message);
                        return;
                    }
                    string filePath = string.Empty;
                    System.Windows.Forms.FolderBrowserDialog folder = new FolderBrowserDialog();
                    do
                    {
                        DialogResult dr = folder.ShowDialog();
                        if (dr == DialogResult.OK)
                            filePath = folder.SelectedPath;
                        if (dr == DialogResult.Cancel)
                            return;
                    } while (!System.IO.Directory.Exists(filePath));

                    foreach (string fileName in x.Result.Keys)
                    {
                        try
                        {
                            //  檔案名稱不能有下列字元<>:"/\|?*
                            string new_fileName = fileName.Replace("：", "꞉").Replace(":", "꞉").Replace("/", "⁄").Replace("／", "⁄").Replace(@"\", "∖").Replace("＼", "∖").Replace("?", "_").Replace("？", "_").Replace("*", "✻").Replace("＊", "✻").Replace("<", "〈").Replace("＜", "〈").Replace(">", "〉").Replace("＞", "〉").Replace("\"", "''").Replace("”", "''").Replace("|", "ㅣ").Replace("｜", "ㅣ");
                            x.Result[fileName].Save(Path.Combine(filePath, new_fileName + ".xls"), FileFormatType.Excel2003);
                            System.Diagnostics.Process.Start(filePath);
                        }
                        catch
                        {
                            MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

                return;
            }
        }
    }
}
