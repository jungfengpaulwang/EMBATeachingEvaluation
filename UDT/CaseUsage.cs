using System;
using FISCA.UDT;

namespace TeachingEvaluation.UDT
{
    /// <summary>
    /// 使用個案
    /// </summary>
    [FISCA.UDT.TableName("ischool.emba.case_management.case_usage")]
    public class CaseUsage : ActiveRecord
    {
        /// <summary>
        /// 個案系統編號
        /// </summary>
        [Field(Field = "ref_case_id", Indexed = true, Caption = "個案系統編號")]
        public int CaseID { get; set; }

        /// <summary>
        /// 開課系統編號
        /// </summary>
        [Field(Field = "ref_course_id", Indexed = true, Caption = "開課系統編號")]
        public int CourseID { get; set; }

        /// <summary>
        /// 教師系統編號
        /// </summary>
        [Field(Field = "ref_teacher_id", Indexed = true, Caption = "教師系統編號")]
        public int TeacherID { get; set; }

        /// <summary>
        /// 淺層複製物件
        /// </summary>
        /// <returns></returns>
        public CaseUsage Clone()
        {
            return this.MemberwiseClone() as CaseUsage;
        }
    }
}
