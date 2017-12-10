using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Homework05.Models
{
    public class SurveyViewModel
    {
        public string SurveyName { get; set; }
        public SurveyType SurveyType { get; set; }        
        public List<int> QuestionIds { get; set; }        
    }

    public class PublishSurveyViewModel
    {
        public int SurveyId { get; set; }
        public int StudyGroupId { get; set; }
        public string SurveyCreatedTime { get; set; }

        //If survey type is message;these are valid
        [EnumDataType(typeof(Frequency))]
        public Frequency FrequencyOfNotifications { get; set; }
        public string Time1 { get; set; }
        public string Time2 { get; set; }
    }
}