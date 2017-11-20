using Homework05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework05.DTOs
{
    public class ResponseDTO
    {
        //Unique identifiers 
        public string ResponseId { get; set; }

        public string SurveyId { get; set; }

        public string QuestionId { get; set; }

        
        public string UserName { get; set; }
        public string StudyGroupName { get; set; }

        //Input properties
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Options { get; set; }
        public string QuestionFrequency { get; set; }

        //Response properties
        public string ResponseText { get; set; }

        public string ResponseReceivedTime { get; set; }

    }
}