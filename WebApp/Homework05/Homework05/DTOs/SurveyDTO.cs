using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.DTOs
{
    public class SurveyDTO
    {
        public string SurveyId { get; set; }
        public string QuestionText { get; set; }
        public string StudyGroupId { get; set; }
        public string SurveyCreatedTime { get; set; }
        public string StudyGroupName { get; set; }
        public string QuestionId { get; set; }       
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }

        public string QuestionFrequency { get; set; }
        public string Time1 { get; set; }
        public string Time2 { get; set; }
    }
}