using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FISCA;
using FISCA.UDT;
using FISCA.Permission;
using FISCA.Presentation;
using K12.Presentation;

namespace TeachingEvaluation
{
    public class Program
    {
        [MainMethod("TeachingEvaluation")]
        public static void Main()
        {
            SyncDBSchema();
            InitDetailContent();
        }

        public static void SyncDBSchema()
        {
            #region 模組啟用先同步Schmea

            SchemaManager Manager = new SchemaManager(FISCA.Authentication.DSAServices.DefaultConnection);

            Manager.SyncSchema(new UDT.AssignedSurvey());
            Manager.SyncSchema(new UDT.Case());
            Manager.SyncSchema(new UDT.Question());
            Manager.SyncSchema(new UDT.QuestionOption());
            Manager.SyncSchema(new UDT.Reply());
            Manager.SyncSchema(new UDT.Section());
            Manager.SyncSchema(new UDT.Survey());
            Manager.SyncSchema(new UDT.CaseUsage());
            Manager.SyncSchema(new UDT.MandrillSendLog());
            Manager.SyncSchema(new UDT.AchievingRate());
            Manager.SyncSchema(new UDT.ReportTemplate());

            if (!FISCA.RTContext.IsDiagMode)
                ServerModule.AutoManaged("https://module.ischool.com.tw/module/140/NTU_EMBA_TeachingEvaluation/udm.xml");        

            #endregion
        }

        public static void InitDetailContent()
        {
            #region 資料項目

            /*  註冊權限  */

            Catalog detail = RoleAclSource.Instance["教師"]["資料項目"];

            detail.Add(new DetailItemFeature(typeof(DetailItems.Teaching_EvaluationProgressEnquiry)));

            if (UserAcl.Current[typeof(DetailItems.Teaching_EvaluationProgressEnquiry)].Viewable)
                NLDPanels.Teacher.AddDetailBulider<DetailItems.Teaching_EvaluationProgressEnquiry>();

            #endregion

            #region 課程>評鑑>管理>評鑑樣版管理

            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_TeachingEvaluation_SurveyTemplateManagement", "管理評鑑樣版"));

            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"].Image = Properties.Resources.network_lock_64;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["評鑑樣版"].Enable = UserAcl.Current["Button_TeachingEvaluation_SurveyTemplateManagement"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["評鑑樣版"].Click += delegate
            {
                (new PrivateControl.PupopDetailPane()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>管理>教學意見表管理

            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_TeachingEvaluation_SurveyTeacherReportTemplateManagement", "管理教學意見表樣版"));

            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["教學意見表樣版"].Enable = UserAcl.Current["Button_TeachingEvaluation_SurveyTeacherReportTemplateManagement"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["教學意見表樣版"].Click += delegate
            {
                (new Forms.TemplateManagement()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>管理>題目標題
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_Evaluation_HierarchyMangagement", "管理題目標題"));

            //MotherForm.RibbonBarItems["課程", "評鑑"]["管理"].Size = RibbonBarButton.MenuButtonSize.Large;
            //MotherForm.RibbonBarItems["課程", "評鑑"]["管理"].Image = Properties.Resources.searchHistory;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["題目標題"].Enable = UserAcl.Current["Button_Evaluation_HierarchyMangagement"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["題目標題"].Click += delegate
            {
                (new Forms.frmHierarchyMangagement()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>管理>教學評鑑意見調查EMAIL提醒
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_Evaluation_Emailing", "管理教學評鑑意見調查EMAIL提醒"));

            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["教學評鑑意見調查EMAIL提醒"].Enable = UserAcl.Current["Button_Evaluation_Emailing"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["教學評鑑意見調查EMAIL提醒"].Click += delegate
            {
                (new Forms.frmEvaluationEMailing()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>問卷組態
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_EvaluationConfiguration_Set", "設定問卷組態"));

            var templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager.Size = RibbonBarButton.MenuButtonSize.Large;
            templateManager.Image = Properties.Resources.sandglass_unlock_64;
            templateManager["問卷組態"].Enable = UserAcl.Current["Button_EvaluationConfiguration_Set"].Executable;
            templateManager["問卷組態"].Click += delegate
            {
                (new Forms.frmEvaluationConfiguration(false)).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>評鑑值統計群組
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_EvaluationStatisticsGroup_Set", "設定評鑑值統計群組"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["評鑑值統計群組"].Enable = UserAcl.Current["Button_EvaluationStatisticsGroup_Set"].Executable;
            templateManager["評鑑值統計群組"].Click += delegate
            {
                (new Forms.frmCalculateEvaluation()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>樣版設定>教學評鑑意見調查EMAIL提醒
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("teaching_evaluation_email_reminder_template_1", "設定教學評鑑意見調查EMAIL提醒文字樣版"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["樣版設定"]["教學評鑑意見調查EMAIL提醒"].Enable = UserAcl.Current["teaching_evaluation_email_reminder_template_1"].Executable;
            templateManager["樣版設定"]["教學評鑑意見調查EMAIL提醒"].Click += delegate
            {
                (new Forms.Email_Content_Template(UDT.CSConfiguration.TemplateName.教學評鑑意見調查EMAIL提醒)).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>樣版設定>教學評鑑意見調查EMAIL再次提醒
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("teaching_evaluation_email_reminder_template_2", "設定教學評鑑意見調查EMAIL再次提醒文字樣版"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["樣版設定"]["教學評鑑意見調查EMAIL再次提醒"].Enable = UserAcl.Current["teaching_evaluation_email_reminder_template_2"].Executable;
            templateManager["樣版設定"]["教學評鑑意見調查EMAIL再次提醒"].Click += delegate
            {
                (new Forms.Email_Content_Template(UDT.CSConfiguration.TemplateName.教學評鑑意見調查EMAIL再次提醒)).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>樣版設定>評鑑注意事項
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("teaching_evaluation_precautions", "設定評鑑注意事項文字樣版"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["樣版設定"]["評鑑注意事項"].Enable = UserAcl.Current["teaching_evaluation_precautions"].Executable;
            templateManager["樣版設定"]["評鑑注意事項"].Click += delegate
            {
                (new Forms.CS_Template_NoSubject(UDT.CSConfiguration.TemplateName.評鑑注意事項)).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>樣版設定>歷史填答記錄說明
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("teaching_evaluation_survey_history_description", "設定歷史填答記錄說明文字樣版"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["樣版設定"]["歷史填答記錄說明"].Enable = UserAcl.Current["teaching_evaluation_survey_history_description"].Executable;
            templateManager["樣版設定"]["歷史填答記錄說明"].Click += delegate
            {
                (new Forms.CS_Template_Survey_History()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>MandrillApiKey
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("teaching_evaluation_email_sending_mandrillapikey", "MandrillApiKey"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["MandrillApiKey"].Enable = UserAcl.Current["teaching_evaluation_email_sending_mandrillapikey"].Executable;
            templateManager["MandrillApiKey"].Click += delegate
            {
                (new Forms.frmMandrillApiKey()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>設定>問卷達標百分比

            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("teaching_evaluation_achievingrate", "設定問卷達標百分比"));

            templateManager = MotherForm.RibbonBarItems["課程", "評鑑"]["設定"];
            templateManager["問卷達標百分比"].Enable = UserAcl.Current["teaching_evaluation_achievingrate"].Executable;
            templateManager["問卷達標百分比"].Click += delegate
            {
                (new Forms.AchievingRateSetting()).ShowDialog();
            };

            #endregion

            #region 課程>評鑑>計算>計算評鑑結果
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_CalculateEValuation", "計算評鑑結果"));
            MotherForm.RibbonBarItems["課程", "評鑑"]["計算評鑑結果"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "評鑑"]["計算評鑑結果"].Image = Properties.Resources.calc_save_64;
            MotherForm.RibbonBarItems["課程", "評鑑"]["計算評鑑結果"].Enable = UserAcl.Current["Button_CalculateEValuation"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["計算評鑑結果"].Click += delegate
            {
                (new Forms.frmEvaluationCalc()).ShowDialog();
            };
            #endregion

            //#region 課程>評鑑>報表>列印授課教師評鑑值
            //RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_TeacherEvaluation_Print", "列印授課教師評鑑值"));


            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印授課教師評鑑值"].Enable = UserAcl.Current["Button_TeacherEvaluation_Print"].Executable;
            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印授課教師評鑑值"].Click += delegate
            //{
            //    (new Export.ExportEvaluation_Teacher()).ShowDialog();
            //};
            //#endregion

            //#region 課程>評鑑>報表>列印開課評鑑值
            //RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_CourseEvaluation_Print", "列印開課評鑑值"));

            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"].Image = Properties.Resources.paste_64;
            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印開課評鑑值"].Enable = UserAcl.Current["Button_CourseEvaluation_Print"].Executable;
            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印開課評鑑值"].Click += delegate
            //{
            //    (new Export.ExportEvaluation_Course()).ShowDialog();
            //};
            //#endregion

            #region 課程>評鑑>管理>寄信測試
            //RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_SubjectEvaluation_Print", "列印課程評鑑值"));

            //MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印課程評鑑值"].Enable = UserAcl.Current["Button_SubjectEvaluation_Print"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["管理"]["寄信測試"].Click += delegate
            {
                (new Forms.TestEmailing()).ShowDialog();
            };
            #endregion

            MotherForm.RibbonBarItems["課程", "評鑑"]["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "評鑑"]["報表"].Image = Properties.Resources.paste_64;
            #region 課程>評鑑>報表>列印個案評鑑值
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_CaseEvaluation_Print", "列印個案評鑑值"));

            MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印個案評鑑值"].Enable = UserAcl.Current["Button_CaseEvaluation_Print"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印個案評鑑值"].Click += delegate
            {
                (new Export.ExportEvaluation_UseCase()).ShowDialog();
            };
            #endregion

            #region 課程>評鑑>報表>列印教師評鑑值
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_ComplexEvaluation_Print", "列印教師評鑑值"));

            MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印教師評鑑值"].Enable = UserAcl.Current["Button_ComplexEvaluation_Print"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["報表"]["列印教師評鑑值"].Click += delegate
            {
                (new Export.ExportEvaluation_ComplexQueries()).ShowDialog();
            };
            #endregion         
            
            #region 課程>評鑑>查詢>填答記錄
            RoleAclSource.Instance["課程"]["功能按鈕"].Add(new RibbonFeature("Button_Query_SurveyHistory", "查詢問卷填答記錄"));

            MotherForm.RibbonBarItems["課程", "評鑑"]["查詢"].Size = RibbonBarButton.MenuButtonSize.Large;
            MotherForm.RibbonBarItems["課程", "評鑑"]["查詢"].Image = Properties.Resources.searchHistory;
            MotherForm.RibbonBarItems["課程", "評鑑"]["查詢"]["填答記錄"].Enable = UserAcl.Current["Button_Query_SurveyHistory"].Executable;
            MotherForm.RibbonBarItems["課程", "評鑑"]["查詢"]["填答記錄"].Click += delegate
            {
                (new Forms.frmQuerySurveyHistory()).ShowDialog();
            };
            #endregion
        }
    }
}
