using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.DTOs
{
    public class ResponseDTO
    {
        public string UserName { get; set; }
        public string StudyGroupName { get; set; }

        public string SurveyId { get; set; }
        //public string SurveyComments { get; set; }
        public string ResponseReceivedTime { get; set; }

        public string SurveyQuestion { get; set; }
        public string QuestionFrequency { get; set; }

        public string ResponseText { get; set; }

        public string ResponseId { get; set; }



    }
}