using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeachingEvaluation.BusinessLogic
{
    class Answer
    {
        public Case Case { get; set; }
        public string Score { set; get; }
        public string Content { set; get; }

        public Answer() { }
    }
}
/// <Answers>
///     <Question QuestionID="">
///         <Answer CaseID="" Score="3">尚可</Answer>
///         <Answer CaseID="123" Score="5">很滿意</Answer>
///     </Question>
/// </Answers>